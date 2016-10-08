using UnityEngine;
using System.Collections;

public class TimedCleanup : MonoBehaviour {

    public float SecondsUntilCleanup;

    private float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= SecondsUntilCleanup)
            Destroy(this.gameObject);
	}
}
