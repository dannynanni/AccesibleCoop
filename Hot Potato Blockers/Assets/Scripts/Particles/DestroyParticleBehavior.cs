using UnityEngine;
using System.Collections;

public class DestroyParticleBehavior : MonoBehaviour {

	public float lifetime = 0.75f;
	protected float timer = 0.0f;

	protected void Update(){
		timer += Time.deltaTime;

		if (timer >= lifetime){
			Destroy(gameObject);
		}
	}
}
