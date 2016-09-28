using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

    public Transform CamFollowTgt;
    public Transform LookTarget;
	
	// Update is called once per frame
	void LateUpdate () {

        transform.position = Vector3.Lerp(transform.position, CamFollowTgt.position, .01f);
        transform.LookAt(LookTarget);
	
	}
}
