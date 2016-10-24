using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CarDataLookup : MonoBehaviour
{
    private static CarDataLookup _instance = null;
    public static CarDataLookup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CarDataLookup>();
            }
            return _instance;
        }
    }

    public List<CarData> _dataList = new List<CarData>();
    private Dictionary<string, CarData> _dataLookup;

    public void Start()
    {
        _dataLookup = new Dictionary<string, CarData>();
        for( int i = 0; i < _dataList.Count; ++i)
        {
            _dataLookup[_dataList[i].name] = _dataList[i];
        }
    }

    public CarData FindCarData(CarStyle style)
    {
        string lookup = style.ToString();
        if (_dataLookup.ContainsKey(lookup))
        {
            return _dataLookup[lookup];
        }
        return null;
    }
}

public enum CarStyle
{
    Car1 = 0,
    Car2,
    Car3,
    Car4,
    Car5
}

public enum CarColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Black
}

