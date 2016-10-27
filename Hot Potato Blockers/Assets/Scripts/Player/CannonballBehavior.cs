using UnityEngine;
using System.Collections;

public class CannonballBehavior : MonoBehaviour {

	public float speed = 0.5f;
	private Rigidbody2D rb2D;

	public float maxXDist = 20.0f; //get rid of cannonballs at this x-axis location; they're definitely off the screen

	private const string BOSS_FIGHT_ORGANIZER = "BossFight";
	private const string BOSS_MONSTER = "BossEnemy";

	private void Start(){
		transform.parent = GameObject.Find(BOSS_FIGHT_ORGANIZER).transform;
		rb2D = GetComponent<Rigidbody2D>();
	}

	private void Update(){
		if (transform.position.x >= maxXDist){
			Destroy(gameObject);
		}
	}

	private void FixedUpdate(){
		rb2D.MovePosition(new Vector3(transform.position.x + speed,
									  transform.position.y,
									  0.0f));
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.transform.name.Contains(BOSS_MONSTER)){
			other.GetComponent<BossEnemyBehavior>().GetHit();
			Destroy(gameObject);
		}
	}
}
