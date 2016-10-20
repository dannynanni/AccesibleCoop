using UnityEngine;
using System.Collections;

public class CameraMidpoint : MonoBehaviour {

    public Transform p1;
    public Transform p2;

	void Update () {

        if (p1 != null && p2 != null)
        {
            transform.position = Vector3.Lerp(p1.position, p2.position, .5f);
        }
        else if (p1 != null)
        {
            transform.position = p1.position;
        }
        else if (p2 != null)
        {
            transform.position = p2.position;
        }
        else
        {
            return;
        }
	
	}
}
