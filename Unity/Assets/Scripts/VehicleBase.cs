using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class VehicleBase : MonoBehaviour
{
    public static float DEFAULT_MAX_SPEED = 5f;
    public CarStyle carStyle = CarStyle.Car5;
    public CarColor carColor = CarColor.Blue;
    public UpgradeLevel upgradeLevel = UpgradeLevel.Basic;

    protected float _carSpeed;
    protected bool _paused = false;
    protected bool _stopped = false;

    protected RaceTrackController _trackController;

    protected void Setup()
    {
        _trackController = FindObjectOfType<RaceTrackController>();
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
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void Release()
    {
        _stopped = false;
    }
}
