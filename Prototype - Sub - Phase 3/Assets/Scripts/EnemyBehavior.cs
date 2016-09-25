using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	protected Rigidbody rigidBody;
	public float turnRate = 2.0f; //turn rate in degrees/second
	public float speed = 1.0f;	//maximum forward speed

	public float spotPlayerDistance = 10.0f; //how far away the enemy will see the player and respond
	protected Transform player;
	protected const string PLAYER_NAME = "Players";

	protected const string PLAYER_WEAPON_NAME = "LightCutout";
	protected PlayerAbility.BasicLightWeapon playerWeapon;

	public float health = 100.0f;

	protected virtual void Start(){
		rigidBody = GetComponent<Rigidbody>();
		player = GameObject.Find(PLAYER_NAME).transform;
		playerWeapon = GameObject.Find(PLAYER_WEAPON_NAME).GetComponent<PlayerAbility.BasicLightWeapon>();
	}

	protected virtual void FixedUpdate(){
		if (Vector3.Distance(player.position, transform.position) <= spotPlayerDistance){
			ChasePlayer();
		}
	}

	/// <summary>
	/// Turn smoothly toward the player, accelerating up to a maximum speed as the enemy goes.
	/// </summary>
	protected virtual void ChasePlayer(){
		transform.rotation = Quaternion.RotateTowards(transform.rotation,
													  GetRotationToward(player),
													  turnRate * Time.deltaTime);

		rigidBody.AddForce(transform.right, ForceMode.Force);

		if (rigidBody.velocity.magnitude > speed){
			rigidBody.velocity = rigidBody.velocity.normalized * speed;
		}
	}

	/// <summary>
	/// Get an angle toward a target.
	/// </summary>
	/// <returns>The angle pointing toward the target.</returns>
	/// <param name="target">The transform of the target.</param>
	protected Quaternion GetRotationToward(Transform target){
		Vector3 vectorRotation = (target.position - transform.position).normalized;

		float zRotation = Mathf.Atan2(vectorRotation.y, vectorRotation.x) * Mathf.Rad2Deg;

		vectorRotation.z = zRotation;

		return Quaternion.Euler(vectorRotation);
	}

	/// <summary>
	/// What happens when this enemy is getting hit?
	/// </summary>
	protected virtual void OnTriggerStay(Collider other){
		Debug.Log("OnTriggerStay() called");
		if (other.gameObject.name.Contains(PLAYER_WEAPON_NAME)){
			if (playerWeapon.Active) { health -= playerWeapon.Damage; }
			if (health <= 0.0f){ Destroy(gameObject); }
		}
	}

	/// <summary>
	/// What happens if this enemy contacts something?
	/// </summary>
	/// <param name="other">The collider of the thing the enemy hit.</param>
	protected void OnTriggerEnter(Collider other){
		if (other.gameObject.name.Contains(PLAYER_NAME)){
			other.gameObject.GetComponent<VesselBehavior>().GotHit = true;
		}
	}
}
