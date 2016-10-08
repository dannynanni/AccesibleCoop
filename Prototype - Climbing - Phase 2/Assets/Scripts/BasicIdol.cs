using UnityEngine;
using System.Collections;

public class BasicIdol : MonoBehaviour {

	public GameObject explosion;

	public void CheckForDestruction(GameObject player)
	{
		if (player.tag == gameObject.tag){
			Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)));
			Destroy(gameObject);
		}
	}
}
