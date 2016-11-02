using UnityEngine;
using System.Collections;

[SelectionBase]
public class TrackNavigation : MonoBehaviour
{
    public const int MAX_RAY_LENGTH = 12;
    public TrackNavigation previousNavigationLink;
    public TrackNavigation nextNavigationLink;
    public bool crossesFinishLine = false;
    public int index = -1;

    public Vector2 centerOffset
    {
        set
        {
            _centerOffset = new Vector3(value.x, value.y, 0);
        }
    }
    [SerializeField]
    private Vector3 _centerOffset;
    public Vector3 position
    {
        get
        {
            return transform.TransformPoint(_centerOffset);
        }
    }

    public void OnDrawGizmos()
    {
        Vector3 position = transform.TransformPoint(_centerOffset);
        if (nextNavigationLink == null)
        {
            Gizmos.DrawRay(position, transform.right * MAX_RAY_LENGTH);
        }
        else
        {
            Color c = Gizmos.color;
            if (crossesFinishLine)
            {
                Gizmos.color = Color.red;
            }
            Vector3 otherPosition = nextNavigationLink.transform.TransformPoint(_centerOffset);
            Gizmos.DrawLine(position, otherPosition);
            Gizmos.color = c;
        }
    }
}
