using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RaceTrackController : MonoBehaviour, ITiledComponent
{
    private List<StartPoint> _startPoints;

    public void SetupFromProperties(IDictionary<string, string> props)
    {
    }

    // Use this for initialization
    void Start ()
    {
        _startPoints = new List<StartPoint>();
        StartPoint[] points = FindObjectsOfType<StartPoint>();
        for( int i = 0; i < points.Length; ++i)
        {
            _startPoints.Add(points[i]);
        }
        _startPoints.Sort((x, y) => x.startIndex.CompareTo(y.startIndex));
	}
	
    public Vector3 SpawnPoint(int index)
    {
        return _startPoints[index].spawnPoint;
    }
}
