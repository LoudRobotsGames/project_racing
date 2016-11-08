using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AICarController : VehicleBase
{
    public float speed;
    public Vector3 offset;

    private TrackNavigation _prevLink;

    // Use this for initialization
    void Start()
    {
        // Default
        Setup();
        _radiusModifier = 0.35f;
        _slerpFactor = 12f;

        offset = UnityEngine.Random.insideUnitCircle * 0.55f;
    }

    protected override void PreUpdate()
    {
        _prevLink = _trackLink;
    }

    protected override void PostUpdate()
    {
        if (_trackLink != _prevLink)
        {
            offset = UnityEngine.Random.insideUnitCircle * 0.55f;
            _carSpeed = _carSpeed + UnityEngine.Random.Range(-0.1f, 0.1f);
        }
        Vector3 position = transform.position + offset;

        Vector3 dir = _trackLink.position - position;

        dir.Normalize();
        dir = Vector3.Lerp(dir, transform.right, 2f * Time.deltaTime);
        dir.Normalize();
        _input.x = dir.x;
        _input.y = dir.y;

        speed = _rigidBody.velocity.magnitude;
    }
}
