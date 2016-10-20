using UnityEngine;
using System.Collections;

public class TestMovementScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Y))
        {
            transform.position = new Vector3(transform.position.x + .01f, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.T))
        {
            transform.position = new Vector3(transform.position.x - .01f, transform.position.y, transform.position.z);
        }

    }
}
