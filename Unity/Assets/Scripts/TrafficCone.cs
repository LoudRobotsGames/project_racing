using UnityEngine;
using System.Collections;
using System;

[SelectionBase]
public class TrafficCone : MonoBehaviour
{
    public float drag = 4f;
    public float angularDrag = 2f;
    // Use this for initialization
    void Start()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.drag = drag;
        rb.angularDrag = angularDrag;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
