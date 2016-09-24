using UnityEngine;
using System.Collections;

public class PlanesAnimationScript : MonoBehaviour {

    public Transform target;
    private float corrected;
    private float ang;
	
	// Update is called once per frame
	void FixedUpdate () {

        corrected = target.localRotation.eulerAngles.z;
        if (corrected >= 180)
        {
            corrected -= 360;
        }

        //Debug.Log(corrected);

        ang = Mathf.Clamp(corrected, -90, 90) / 3;

        transform.localRotation = Quaternion.Euler(0, 0, ang);
	
	}
}
