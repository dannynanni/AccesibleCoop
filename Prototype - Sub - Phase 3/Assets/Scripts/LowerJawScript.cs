using UnityEngine;
using System.Collections;

public class LowerJawScript : MonoBehaviour {

    float lowerLim = -14;
    float upperLim = 23;

    public float jawAng;

    private float timer;
    public float period;

	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        jawAng = (Mathf.Cos(timer * period) * (upperLim - lowerLim)/2) + lowerLim;

        transform.rotation = Quaternion.Euler(0, 0, jawAng);

	}
}
