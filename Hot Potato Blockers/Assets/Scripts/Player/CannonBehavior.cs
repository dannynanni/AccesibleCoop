using UnityEngine;
using System.Collections;

public class CannonBehavior : MonoBehaviour {

	private Rigidbody2D rb2D;

	public float speed = 1.3f;
	public float range = 1.3f;

	private Vector3 posVec;
	private float startX;

	private const string BALL = "Ball";

	private GameObject cannonball;
	private const string CANNONBALL = "Cannonball";
	private bool reloading = false;
	public float reloadDuration = 1.0f;
	private float reloadTimer = 0.0f;

	private void Start(){
		rb2D = GetComponent<Rigidbody2D>();
		posVec = transform.localPosition;
		startX = posVec.x;
		cannonball = Resources.Load(CANNONBALL) as GameObject;
	}

	private void FixedUpdate(){
		rb2D.MovePosition(BackAndForth());
	}

	private void Update(){
		if (reloading){
			reloading = RunReloadTimer();
		}
	}

	private Vector3 BackAndForth(){
		posVec.x = startX + range * Mathf.Sin(Time.time * speed); //using Time.time means all bobbing things will
		//be at the same place in their bob

		return posVec;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.transform.name.Contains(BALL) && !reloading){
			reloading = Shoot();
		}
	}

	private bool Shoot(){
		Instantiate(cannonball, transform.position, Quaternion.identity);

		return true;
	}

	private bool RunReloadTimer(){
		reloadTimer += Time.deltaTime;

		if (reloadTimer >= reloadDuration){
			reloadTimer = 0.0f;
			return false;
		} else {
			return true;
		}
	}
}
