using UnityEngine;
using System.Collections;

public class ObjectLookAtScript : MonoBehaviour {

    public Transform tgt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (tgt == null)
        {
            tgt = GameObject.Find("Players").transform;
        }

        transform.LookAt(tgt);
	
	}
}
