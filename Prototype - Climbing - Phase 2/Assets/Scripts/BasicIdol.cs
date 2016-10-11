using UnityEngine;
using System.Collections;

public class BasicIdol : MonoBehaviour {

	public GameObject explosion;
    public GameObject soundSmash;

	public virtual void CheckForDestruction(GameObject player){
		if (player.tag == gameObject.tag){
			Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)));
            Instantiate(soundSmash, transform.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)));
            Destroy(gameObject);
            Debug.Log("Basic Idol Check For Destruction");
		}
	}
}
