using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class VehicleBase : MonoBehaviour
{
    public Tire[] tires;
    public string id;

    public static float DEFAULT_MAX_SPEED = 5f;
    public CarStyle carStyle = CarStyle.Car5;
    public CarColor carColor = CarColor.Blue;
    public UpgradeLevel upgradeLevel = UpgradeLevel.Basic;

    protected float _carSpeed;
    protected bool _paused = false;
    protected bool _stopped = false;
    protected Vector2 _input;
    protected float _radiusModifier = 1f;
    protected float _slerpFactor = 8f;

    protected Rigidbody2D _rigidBody;
    protected RaceTrackController _trackController;
    protected TrackNavigation _trackLink;
    protected int lastNavIndex = 0;

    public void Setup()
    {
        _trackController = FindObjectOfType<RaceTrackController>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _input = Vector2.zero;
    }

    public void SpawnAtStart(int index)
    {
        transform.position = _trackController.SpawnPoint(index);
        _trackLink = _trackController.StartingLink;
    }

    public void SetCarChoice(int carNum, bool isMultiplayer)
    {
        CarData data = CarDataLookup.Instance.FindCarData(carStyle);
        upgradeLevel = (UpgradeLevel)carNum;
        if (data != null)
        {
            SpriteRenderer visual = GetComponentInChildren<SpriteRenderer>();
            if (visual)
            {
                visual.sprite = data.GetVisual(carColor);
            }
            PolygonCollider2D collider = GetComponentInChildren<PolygonCollider2D>();
            if (collider != null)
            {
                collider.SetPath(0, data.points.ToArray());
            }

            if (isMultiplayer)
            {
                // Car choice has only a visual effect in multiplayer games
                _carSpeed = data.GetMaxSpeed(0);
            }
            else
            {
                _carSpeed = data.GetMaxSpeed(carNum - 1);
            }
        }
    }

    public void SetPaused(bool isPaused)
    {
        _paused = isPaused;
    }

    public void Stop()
    {
        _stopped = true;
        _rigidBody.velocity = Vector2.zero;
    }

    public void Release()
    {
        _stopped = false;
    }

    protected virtual void PreUpdate()
    {

    }

    protected virtual void PostUpdate()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        PreUpdate();
        if (_trackController != null)
        {
            if (_trackLink == null)
            {
                return;
            }

            // Get the distance to the registered link
            float distance = Vector3.Distance(this.transform.position, _trackLink.position);
            // Find the closest link if there is one
            TrackNavigation link = _trackController.GetClosestLink(transform.position);
            // Is it a different link than the one we have registered?
            // This only matters if its a non-null link, and we are outside the range of the current link
            if (link != _trackLink && link != null && distance > _trackLink.navigationRadius * _radiusModifier)
            {
                // Going backwards?
                if (link.index < lastNavIndex)
                {
                    // Show wrong way text? have to work out wrapping indices
                }
                else if (link.index > lastNavIndex + 1 && !_trackLink.crossesFinishLine)
                {
                    transform.position = _trackLink.previousNavigationLink.position;
                }
            }
            else
            {
                if (distance < _trackLink.navigationRadius * _radiusModifier)
                {
                    _trackLink = _trackLink.nextNavigationLink;
                    lastNavIndex = _trackLink.index;
                }
            }
        }
        PostUpdate();
    }

    void FixedUpdate()
    {
        if (_paused || _stopped) return;

        float carSpeed = _carSpeed;
        float drag = carSpeed / (tires.Length + 1);
        for (int i = 0; i < tires.Length; ++i)
        {
            if (tires[i].isOnTrack == false)
            {
                carSpeed -= drag;
            }
        }
        // Add a vector related to the steering
        _rigidBody.velocity = _input * carSpeed;

        // Turn towards the direction we're traveling
        if (_rigidBody.velocity.magnitude > 0.2f)
        {
            float targetAngle = Mathf.Atan2(_rigidBody.velocity.y, _rigidBody.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), _slerpFactor * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameController.FINISH_LINE_LAYER)
        {
            TrackNavigation link = _trackController.GetClosestLink(transform.position);
            if (link != null)
            {
                if (link.crossesFinishLine && _trackLink.previousNavigationLink == link)
                {
                    _trackController.CrossFinishLine(this);
                }
            }
        }
    }
}
