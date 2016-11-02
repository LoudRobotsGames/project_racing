using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RaceTrackController : MonoBehaviour, ITiledComponent
{
    public delegate void CarEvent(string id);

    private List<StartPoint> _startPoints;
    private List<TrackNavigation> _navPoints;
    private Dictionary<string, int> _lapsComplete;
    private TrackNavigation _startingNavLine;
    
    public int targetLaps;
    public CarEvent onRaceFinished;
    public CarEvent onLapFinished;
    
    public void SetupFromProperties(IDictionary<string, string> props)
    {
    }

    // Use this for initialization
    void Start ()
    {
        _lapsComplete = new Dictionary<string, int>();
        _startPoints = new List<StartPoint>();
        StartPoint[] points = FindObjectsOfType<StartPoint>();
        for( int i = 0; i < points.Length; ++i)
        {
            _startPoints.Add(points[i]);
        }
        _startPoints.Sort((x, y) => x.startIndex.CompareTo(y.startIndex));

        _navPoints = new List<TrackNavigation>();
        TrackNavigation[] navPoints = FindObjectsOfType<TrackNavigation>();
        for (int i = 0; i < navPoints.Length; ++i)
        {
            _navPoints.Add(navPoints[i]);
            if (navPoints[i].crossesFinishLine)
            {
                _startingNavLine = navPoints[i];
            }
        }

        if (_startingNavLine == null)
        {
            Debug.LogError("Missing start navigation lines!");
            return;
        }

        TrackNavigation nav = _startingNavLine.nextNavigationLink;
        int index = 0;

        _startingNavLine.index = index;
        while (nav != null && nav != _startingNavLine)
        {
            index++;
            nav.index = index;
            nav = nav.nextNavigationLink;

            if (index > 1000)
            {
                Debug.LogError("There is no way there are 1000+ nav markers... infinite loop is probable...");
                break;
            }
        }
	}
	
    public void CrossFinishLine(CarController car)
    {
        Vector3 heading = car.transform.right;
        float dot = Vector3.Dot(_startingNavLine.transform.right, heading);
        if (dot <= 0)
        {
            return;
        }

        if (_lapsComplete.ContainsKey(car.id))
        {
            int laps = _lapsComplete[car.id];
            laps += 1;
            if (onLapFinished != null)
            {
                onLapFinished(car.id);
            }
            // Report race finished if done
            if (laps >= targetLaps)
            {
                if (onRaceFinished != null)
                {
                    onRaceFinished(car.id);
                }
            }
            _lapsComplete[car.id] = laps;
        }
        else
        {
            _lapsComplete[car.id] = 0;
        }
    }

    public Vector3 SpawnPoint(int index)
    {
        return _startPoints[index].spawnPoint;
    }

    public TrackNavigation GetClosestLink(Vector3 position)
    {
        float minDistance = float.MaxValue;
        TrackNavigation closest = null;
        for (int i = 0; i < _navPoints.Count; ++i)
        {
            TrackNavigation nav = _navPoints[i];
            if (nav != null)
            {
                float distance = Vector3.Distance(position, nav.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = nav;
                }
            }
        }

        if (closest != null)
        {
            
        }

        return closest;
    }
}
