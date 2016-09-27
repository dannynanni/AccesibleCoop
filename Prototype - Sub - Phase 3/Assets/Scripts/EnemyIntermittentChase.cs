namespace EnemyBehaviors
{
	using UnityEngine;
	using System.Collections;

	public class EnemyIntermittentChase : EnemyBehavior {

		public Sprite[] sprites; //"resting" sprite must be at 0, "swimming" sprite at 1
		protected SpriteRenderer spriteRenderer;
		public float restSpriteDist = 1.0f; //how close to the destination the enemy must be to display the rest sprite
		public float findDelay = 3.0f; //the time between the enemy re-updating its knowledge of the player's location
		protected float findTimer = 0.0f;
		public float movementDuration = 3.0f; //the time the enemy requires to cross the distance to its current destination
		protected float movementTimer = 0.0f;
		public float movementDistance = 25.0f; //how far the enemy goes in a single swim stroke
		protected Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
		protected Vector3 currentDestination = new Vector3(0.0f, 0.0f, 0.0f);
		public AnimationCurve movementCurve;


		protected override void Start(){
			rigidBody = GetComponent<Rigidbody>();
			player = GameObject.Find(PLAYER_NAME).transform;
			playerWeapon = GameObject.Find(PLAYER_WEAPON_NAME).GetComponent<PlayerAbility.BasicLightWeapon>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			startPosition = transform.position;
		}

		protected override void FixedUpdate(){
			if (Vector3.Distance(player.position, transform.position) <= spotPlayerDistance){
				CountdownToFindPlayer();
			} else {
				findTimer = 0.0f; //reset the timer when the player escapes from the monster
			}
				
			rigidBody.MovePosition(MoveToDestination());
			spriteRenderer.sprite = SwimOrRestSprite();
		}

		protected virtual void CountdownToFindPlayer(){
			findTimer += Time.deltaTime;

			if (findTimer >= findDelay){
				findTimer = 0.0f;
				movementTimer = 0.0f;
				startPosition = transform.position;
				currentDestination = FindPlayer();
				transform.rotation = GetRotationToward(player);
			}
		}

		protected virtual Vector3 FindPlayer(){

			Vector3 destinationVector = (player.position - transform.position).normalized;
			return transform.position + (destinationVector * movementDistance);
		}

		protected virtual Vector3 MoveToDestination(){
			movementTimer += Time.deltaTime;

			return Vector3.Lerp(startPosition,
								currentDestination,
								movementCurve.Evaluate(movementTimer/movementDuration));
		}

		protected virtual Sprite SwimOrRestSprite(){
			if (Vector3.Distance(transform.position, startPosition) > restSpriteDist){
				return sprites[1];
			} else {
				return sprites[0];
			}
		}

		protected virtual Quaternion GetRotationToward(Transform target){
			Vector3 vectorRotation = (target.position - transform.position).normalized;

			float zRotation = Mathf.Atan2(vectorRotation.y, vectorRotation.x) * Mathf.Rad2Deg;

			vectorRotation.z = zRotation - 90; //correction for difference between orientation of sprite * gameobj axis

			return Quaternion.Euler(vectorRotation);
		}
	}
}
