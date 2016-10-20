using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

    public float timeToDestroy;
    private float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer >= timeToDestroy)
        {
            if (gameObject.name == "DirtPatch(Clone)")
            {
                Instantiate(Resources.Load("DustWave"), transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }

    }
}
