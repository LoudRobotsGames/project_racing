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

    public Sprite GetVisual(CarColor color)
    {
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
}
