using UnityEngine;
using System.Collections;

public class CameraFollowTgt : MonoBehaviour {

    public Transform tgt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

        transform.position = Vector3.Lerp(transform.position, tgt.position, .08f);
	
	}
}
