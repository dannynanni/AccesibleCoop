using UnityEngine;
using System.Collections;

public class TransformLookAtScript : MonoBehaviour {

    public GameObject lookTgt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(lookTgt.transform);
	
	}
}
