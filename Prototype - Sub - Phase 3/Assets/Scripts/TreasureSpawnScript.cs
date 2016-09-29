using UnityEngine;
using System.Collections;

public class TreasureSpawnScript : MonoBehaviour {

    public GameObject treasure;

    public int numTreasure;

    public float xMin;
    public float xMax;

    public float yMin;
    public float yMax;

	// Use this for initialization
	void Start () {
	
        for (int i = 0; i < numTreasure; i++)
        {
            Instantiate(treasure, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0), Quaternion.identity);
        }

	}
	
}
