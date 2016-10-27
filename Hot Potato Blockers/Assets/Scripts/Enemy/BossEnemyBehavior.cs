using UnityEngine;
using System.Collections;

public class BossEnemyBehavior : EnemyBaseScript {

	private Rigidbody2D rb2D;

	public float speed = 1.0f;
	public float hitShakespeed = 2.0f;
	public float hitShakeDuration = 0.3f;
	private float hitShakeTimer = 0.0f;
	public float range = 1.0f;
	public float shakeRange = 0.2f;

	private Vector3 posVec;
	private float startY;
	private float startX;

	public int health = 3;
	private bool gotHit = false;
	private GameObject destroyParticle;

	private const string DESTROY_PARTICLE = "DestroyParticle";
	private const string ENEMY_ORGANIZER = "Enemies";

	void Awake(){
		rb2D = GetComponent<Rigidbody2D>();
		posVec = transform.localPosition;
		startY = posVec.y;
		startX = posVec.x;
		destroyParticle = Resources.Load(DESTROY_PARTICLE) as GameObject;
		transform.parent = GameObject.Find(ENEMY_ORGANIZER).transform;
	}

	//The boss normally moves up and down. When hit, it shakes left and right.
	private void FixedUpdate(){
		if (!gotHit){
			rb2D.MovePosition(BobUpAndDown());
		} else {
			Debug.Log("shaking");
			rb2D.MovePosition(BackAndForth());
		}
	}

	private void Update (){
		if (gotHit){
			gotHit = RunHitTimer();
		}
	}

	private Vector3 BobUpAndDown(){
		posVec.y = startY + range * Mathf.Sin(Time.time * speed); //using Time.time means all bobbing things will
		//be at the same place in their bob

		return posVec;
	}

	private Vector3 BackAndForth(){
		posVec.x = startX + shakeRange * Mathf.Sin(Time.time * speed);

		return posVec;
	}

	public void GetHit(){
		health--;
		gotHit = true;
		if (health <= 0){
			GetDestroyed();
		}
	}

	private bool RunHitTimer(){
		hitShakeTimer += Time.deltaTime;

		if (hitShakeTimer >= hitShakeDuration){
			hitShakeTimer = 0.0f;
			return false;
		} else {
			return true;
		}
	}

	public override void GetDestroyed(){
		Instantiate(destroyParticle, transform.position, destroyParticle.transform.rotation);
		Destroy(gameObject);
	}
}
