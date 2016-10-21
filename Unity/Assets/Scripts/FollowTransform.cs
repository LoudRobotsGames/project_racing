using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {
    public Transform target;
    public int precision = 10;

    private Vector3 _offset;

	// Use this for initialization
	void Start () {
        _offset = this.transform.position - target.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 position = target.position + _offset;
        position.x = System.Convert.ToSingle(System.Math.Round(position.x, precision));
        position.y = System.Convert.ToSingle(System.Math.Round(position.y, precision));
        position.z = System.Convert.ToSingle(System.Math.Round(position.z, precision));
        this.transform.position = position;
	}
}
