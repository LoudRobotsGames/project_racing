using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public float[] carMaxSpeeds;
    public Tire[] tires;
    public string id;
    public CarStyle carStyle = CarStyle.Car5;
    public CarColor carColor = CarColor.Blue;

    public AnalogControl analogControl;
    public AudioClip[] soundEffects;

    private float _carSpeed;
    private bool _paused;
    private bool _stopped = false;
    private RaceTrackController _trackController;
    private TrackNavigation _trackLink;
    private int lastNavIndex = 0;

    // Use this for initialization
    void Start()
    {
        // Default
        _carSpeed = carMaxSpeeds[0];
        _trackController = FindObjectOfType<RaceTrackController>();
    }

    public void SpawnAtStart(int index)
    {
        transform.position = _trackController.SpawnPoint(index);
        _trackLink = _trackController.StartingLink;
    }

    public void SetPaused(bool isPaused)
    {
        _paused = isPaused;
    }

    public void SetCarChoice(int carNum, bool isMultiplayer)
    {
        if (isMultiplayer)
        {
            // Car choice has only a visual effect in multiplayer games
            _carSpeed = carMaxSpeeds[0];
        }
        else {
            _carSpeed = carMaxSpeeds[carNum - 1];
        }
        CarData data = CarDataLookup.Instance.FindCarData(carStyle);
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
        }
    }

    public void PlaySoundForLapFinished()
    {
        AudioSource.PlayClipAtPoint(soundEffects[0], transform.position);
    }

    public void Stop()
    {
        _stopped = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
        GetComponent<Rigidbody2D>().velocity = analogControl.GetNormalizedSteering() * carSpeed;

        // Turn towards the direction we're traveling
        if (GetComponent<Rigidbody2D>().velocity.magnitude > 0.4f)
        {
            float targetAngle = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), 8 * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
            if (link != _trackLink && link != null && distance > _trackLink.navigationRadius)
            {
                // Going backwards?
                if (link.index < lastNavIndex)
                {
                    return;
                }
                if (link.index > lastNavIndex + 1 && !_trackLink.crossesFinishLine)
                {
                    transform.position = _trackLink.previousNavigationLink.position;
                }
            }
            else
            {
                if (distance < _trackLink.navigationRadius)
                {
                    _trackLink = _trackLink.nextNavigationLink;
                    lastNavIndex = _trackLink.index;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Color c = Gizmos.color;
        if (_trackLink != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _trackLink.position);
            if (_trackLink.nextNavigationLink != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _trackLink.nextNavigationLink.position);
            }
            Color old = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(_trackLink.position, Vector3.forward, _trackLink.navigationRadius * 0.9f);
            UnityEditor.Handles.color = old;
        }
        
        Gizmos.color = c;
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
