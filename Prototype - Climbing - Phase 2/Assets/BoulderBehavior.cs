using UnityEngine;
using System.Collections;

public class BoulderBehavior : MonoBehaviour {

    public float speed;

	void Update () {

        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y <= - 5)
        {
            Destroy(this.gameObject);
        }
	
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.name.Contains("player")){
			other.gameObject.GetComponent<SimplePlatformController>().Stunned = true;
		}
	}
}
