using UnityEngine;
using System.Collections;

public class PlatformAutoDraw : MonoBehaviour {

    public bool ground;

    public GameObject GndFlatDirt;

	// Use this for initialization
	void Start () {
	
        if (ground)
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(GndFlatDirt, transform.position - Vector3.up * (i+1), Quaternion.identity, transform);
            }
        }

	}
}
