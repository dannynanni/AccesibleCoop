using UnityEngine;
using System.Collections;

public class PropRotationScript : MonoBehaviour {

    private float xRot;
    private float rateLerp;
    public float rate;

    public ParticleSystem bubbles;
	
	// Update is called once per frame
	void FixedUpdate () {

        rateLerp = Mathf.Lerp(rateLerp, rate, .1f);

        xRot += rateLerp;

        transform.localRotation = Quaternion.Euler(xRot, 0, 90);

        bubbles.Emit((int)rateLerp);
	
	}
}
