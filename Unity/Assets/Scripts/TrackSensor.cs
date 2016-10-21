using UnityEngine;
using System.Collections;

public class TrackSensor : MonoBehaviour
{
    public bool isOnTrack = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        CarController car = collision.gameObject.GetComponent<CarController>();
        if (car != null )
        {
            isOnTrack = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        CarController car = collision.gameObject.GetComponent<CarController>();
        if (car != null)
        {
            isOnTrack = false;
        }
    }
}
