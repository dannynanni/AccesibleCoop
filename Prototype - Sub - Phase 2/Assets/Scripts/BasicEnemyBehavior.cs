using UnityEngine;
using System.Collections;

public class BasicEnemyBehavior : MonoBehaviour {

	protected SpriteRenderer spriteRenderer;
	protected const float MAX_ALPHA = 1.0f;
	protected const float LOW_ALPHA = 0f;
	protected float disappearTimer = 0.0f;
	public float disappearTime = 2.0f;
	public AnimationCurve disappearCurve;

	protected Rigidbody rigidBody;
	protected Transform movementTarget;
	const string MOVEMENT_TARGET = "Movement target";
	public float maxTargetShift = 5.0f; //max number of degrees the target will rotate around the enemy
	public float turnRate = 2.0f; //turn rate in degrees/second
	public float timeToTurn = 2.0f;
	protected float turnTimer = 0.0f;
	public float speed = 1.0f;

	protected const string PLAYER_NAME = "Players";
	protected const string SHIELD_NAME_FRAGMENT = "Sphere";

	protected void Start(){
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigidBody = GetComponent<Rigidbody>();
		movementTarget = transform.Find(MOVEMENT_TARGET);
	}

	protected void Update(){
		spriteRenderer.color = ReduceVisibility();

		RotateMovementTarget();
	}


	/// <summary>
	/// Make the enemy's visibility fade over time, based on a curve set in the Inspector.
	/// </summary>
	/// <returns>The enemy's color, with the alpha changed based on the curve.</returns>
	protected Color ReduceVisibility(){
		disappearTimer += Time.deltaTime;

		float tempAlpha = Mathf.Lerp(MAX_ALPHA, LOW_ALPHA, disappearCurve.Evaluate(disappearTimer/disappearTime));

		Color tempColor = spriteRenderer.color;

		tempColor.a = tempAlpha;

		return tempColor;
	}


	/// <summary>
	/// Make the enemy completely visible, by resetting back to the start of the curve. Note that this function
	/// will only work correctly if the curve is such that visibility is maximized at the start of the curve!
	/// </summary>
	public void ResetVisibility(){
		disappearTimer = 0.0f;
        Debug.Log("Reset vis");
	}


	/// <summary>
	/// Rotate the child gameobject the enemy is "swimming" toward around the enemy, so that the enemy
	/// will seem to swim toward a new point.
	/// </summary>
	protected void RotateMovementTarget(){
		turnTimer += Time.deltaTime;

		if (turnTimer >= timeToTurn){
			movementTarget.RotateAround(transform.position,
										Vector3.forward,
										Random.Range(-maxTargetShift, maxTargetShift));
			turnTimer = 0.0f;

			movementTarget.position = Maintain2DPosition(movementTarget.position);
		}
	}


	/// <summary>
	/// Put a position flat on a 2D plane. Used to keep objects from "drifting" in the third dimension.
	/// </summary>
	/// <returns>The Vector3 argument, with z = 0.0.</returns>
	/// <param name="position">The position of the object to be put on a flat 2D plane.</param>
	Vector3 Maintain2DPosition(Vector3 position){
		Vector3 temp = position;

		temp.z = 0.0f;

		return temp;
	}

	protected void FixedUpdate(){
		transform.rotation = Quaternion.RotateTowards(transform.rotation,
													  GetRotationToward(movementTarget),
													  turnRate * Time.deltaTime);

		rigidBody.AddForce(GoToward(movementTarget), ForceMode.Force);

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
	/// Get a point on a path that goes from this gameobject through a waypoint. The point can be
	/// before, at, or beyond the waypoint, depending upon the speed.
	/// </summary>
	/// <returns>A point along the path determined by the speed.</returns>
	/// <param name="waypoint">A transform to use as the waypoint.</param>
	protected Vector3 GoToward(Transform waypoint){
		Vector3 direction = (waypoint.position - transform.position).normalized;

		return direction * speed * Time.deltaTime;
	}


	/// <summary>
	/// What happens if this object gets hit?
	/// </summary>
	public void GetHit(){
		Destroy(gameObject);
	}


	/// <summary>
	/// What happens if this enemy contacts something?
	/// </summary>
	/// <param name="other">The collider of the thing the enemy hit.</param>
	protected void OnTriggerEnter(Collider other){
		if (other.name.Contains(PLAYER_NAME)){
			//do something
			Debug.Log("You got hit by an enemy!");
		} else if (other.name.Contains(SHIELD_NAME_FRAGMENT)){
			//do something
			Destroy(gameObject);
		}
	}
}
