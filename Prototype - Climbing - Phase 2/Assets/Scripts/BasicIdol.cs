using UnityEngine;
using System.Collections;

public class BasicIdol : MonoBehaviour {

	public GameObject explosion;

	public void CheckForDestruction(GameObject player)
	{
		if (player.tag == gameObject.tag){
			Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
