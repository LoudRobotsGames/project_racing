using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {
    public Transform target;

    private Vector3 _offset;

	// Use this for initialization
	void Start () {
        _offset = this.transform.position - target.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        this.transform.position = target.position + _offset;
	}
}
