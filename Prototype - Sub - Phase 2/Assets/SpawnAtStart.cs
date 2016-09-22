using UnityEngine;
using System.Collections;

public class SpawnAtStart : MonoBehaviour {

    public GameObject basicEnemy;
    public GameObject player;

	// Use this for initialization
	void Start () {
	
        for (int i = 0; i < 50; i++)
        {
            Instantiate(
                basicEnemy, 
                new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0) + player.transform.position, 
                Quaternion.identity);
        }

	}
}
