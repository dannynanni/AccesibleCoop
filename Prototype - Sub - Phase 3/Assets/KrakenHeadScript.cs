using UnityEngine;
using System.Collections;

public class KrakenHeadScript : MonoBehaviour {

    public Transform tgt;

    public float xRate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (tgt == null)
        {
            tgt = GameObject.Find("Players").transform;
        }
        else
        {
            transform.position = new Vector3(transform.position.x + xRate, tgt.transform.position.y, 0);
        }
    }
}
