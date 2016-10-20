using UnityEngine;
using System.Collections;

public class ProceduralSpawnScript : MonoBehaviour {

    public GameObject whatToMake;
    public int num;

	void Start () {
	
        for (int i = 0; i < num; i++)
        {
            Instantiate(whatToMake, transform.position + Vector3.right * i, Quaternion.identity, transform);
        }
	}
}
