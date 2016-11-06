using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "Cars/Data", order = 1)]
public class CarData : ScriptableObject
{
    public Sprite red;
    public Sprite blue;
    public Sprite green;
    public Sprite yellow;
    public Sprite black;
    public List<Vector2> points;
    public float[] maxSpeeds;

    public Sprite GetVisual(CarColor color)
    {
        if( color == CarColor.Any)
        {
            color = (CarColor)UnityEngine.Random.Range(0, 5);
        }
        switch (color)
        {
            case CarColor.Red:
                return red;
            case CarColor.Blue:
                return blue;
            case CarColor.Green:
                return green;
            case CarColor.Yellow:
                return yellow;
        }
        return black;
    }

    public float GetMaxSpeed(int upgrade)
    {
        if (upgrade < 0 || upgrade >= maxSpeeds.Length)
        {
            return VehicleBase.DEFAULT_MAX_SPEED;
        }

        return maxSpeeds[upgrade];
    }
}
