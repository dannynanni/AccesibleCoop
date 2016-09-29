namespace EnemyBehaviors
{
	using UnityEngine;
	using System.Collections;

	public class EnemyConstantFollow : EnemyBehavior {

		public Sprite[] sprites; //"resting" sprite must be at 0, "swimming" sprite at 1
		protected SpriteRenderer spriteRenderer;

		protected bool scared = false;
		public float scaredDuration = 10.0f;
		protected float scaredTimer = 0.0f;


		protected override void Start(){
			rigidBody = GetComponent<Rigidbody>();
			player = GameObject.Find(PLAYER_NAME).transform;
			playerWeapon = GameObject.Find(PLAYER_WEAPON_NAME).GetComponent<PlayerAbility.BasicLightWeapon>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		protected override void FixedUpdate(){
			if (Vector3.Distance(player.position, transform.position) <= spotPlayerDistance){
				ChasePlayer();
				spriteRenderer.sprite = ChaseOrWaitSprite("chase");
			} else {
				rigidBody.velocity = Vector3.zero;
				rigidBody.angularVelocity = Vector3.zero;
				spriteRenderer.sprite = ChaseOrWaitSprite("wait");
			}
		}

		/// <summary>
		/// Turn smoothly toward the player, accelerating up to a maximum speed as the enemy goes.
		/// </summary>
		protected override void ChasePlayer(){
			transform.rotation = Quaternion.RotateTowards(transform.rotation,
				GetRotationToward(player),
				turnRate * Time.deltaTime);

			rigidBody.AddForce(transform.right, ForceMode.Force);

			if (rigidBody.velocity.magnitude > speed){
				rigidBody.velocity = rigidBody.velocity.normalized * speed;
			}
		}

		protected virtual Sprite ChaseOrWaitSprite(string state){
			if (state == "chase"){
				return sprites[1];
			} else {
				return sprites[0];
			}
		}

		protected override Quaternion GetRotationToward(Transform target){
			Vector3 vectorRotation = (target.position - transform.position).normalized;

			float zRotation = Mathf.Atan2(vectorRotation.y, vectorRotation.x) * Mathf.Rad2Deg;

			vectorRotation.z = zRotation; //correction for difference between orientation of sprite v. gameobj axis

			return Quaternion.Euler(vectorRotation);
		}
	}
}

