using UnityEngine;
using System.Collections;

public class CarController : VehicleBase
{
    public AnalogControl analogControl;
    public AudioClip[] soundEffects;

    // Use this for initialization
    void Start()
    {
        // Default
        Setup();
    }

    public void PlaySoundForLapFinished()
    {
        AudioSource.PlayClipAtPoint(soundEffects[0], transform.position);
    }

    protected override void PostUpdate()
    {
        _input = analogControl.GetNormalizedSteering();
    }

    public void OnDrawGizmos()
    {
        Color c = Gizmos.color;
        if (_trackLink != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _trackLink.position);
            if (_trackLink.nextNavigationLink != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _trackLink.nextNavigationLink.position);
            }
            Color old = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(_trackLink.position, Vector3.forward, _trackLink.navigationRadius * 0.9f);
            UnityEditor.Handles.color = old;
        }
        
        Gizmos.color = c;
    }
}
