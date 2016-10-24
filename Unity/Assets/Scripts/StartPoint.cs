using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[SelectionBase]
public class StartPoint : MonoBehaviour, ITiledComponent
{
    public int startIndex = -1;
    public Vector3 spawnPoint;

    public void Start()
    {
        Renderer r = GetComponentInChildren<Renderer>();
        spawnPoint = r.bounds.center;
    }

    public void SetupFromProperties(IDictionary<string, string> props)
    {
        if (props.ContainsKey("start_index"))
        {
            startIndex = int.Parse(props["start_index"]);
        }
    }
}
