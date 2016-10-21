using UnityEngine;
using System.Collections;

public class Tire : MonoBehaviour
{
    private int layer;
    public bool isOnTrack = false;

    public void Start()
    {
        layer = LayerMask.NameToLayer("Road_Track");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layer)
        {
            isOnTrack = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layer)
        {
            isOnTrack = false;
        }
    }
}
