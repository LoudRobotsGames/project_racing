﻿using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
	public float[] carMaxSpeeds;
    public Tire[] tires;
    public CarStyle carStyle = CarStyle.Car5;
    public CarColor carColor = CarColor.Blue;

	public AnalogControl analogControl;
	public AudioClip[] soundEffects;

	private float _carSpeed;
	private bool _paused;
	private bool _stopped = false;
	// Use this for initialization
	void Start () {

		// Default
		_carSpeed = carMaxSpeeds [0];
	}

	public void SetPaused(bool isPaused) {
		_paused = isPaused;
	}


	public void SetCarChoice (int carNum, bool isMultiplayer) {
		if (isMultiplayer) {
			// Car choice has only a visual effect in multiplayer games
			_carSpeed = carMaxSpeeds[0];
		} else {
			_carSpeed = carMaxSpeeds [carNum - 1];
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

	public void PlaySoundForLapFinished() {
		AudioSource.PlayClipAtPoint (soundEffects[0], transform.position);
	}


	public void Stop() {
		_stopped = true;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

	void FixedUpdate () {
		if (_paused || _stopped) return;

        float carSpeed = _carSpeed;
        float drag = carSpeed / (tires.Length + 1);
        for( int i = 0; i < tires.Length; ++i)
        {
            if (tires[i].isOnTrack == false)
            {
                carSpeed -= drag;
            }
        }
		// Add a vector related to the steering
		GetComponent<Rigidbody2D>().velocity = analogControl.GetNormalizedSteering () * carSpeed;

		// Turn towards the direction we're traveling
		if (GetComponent<Rigidbody2D>().velocity.magnitude > 0.4f) {
			float targetAngle = Mathf.Atan2 (GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, targetAngle), 8 * Time.deltaTime);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
