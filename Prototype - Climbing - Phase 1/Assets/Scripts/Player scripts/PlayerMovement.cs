namespace TurnTakers
{
	using UnityEngine;
	using System.Collections;

	public class PlayerMovement : ThingsThatTakeTurns {

		public float inputWait = 1.0f; //delay before a new player can enter inputs
		private float inputTimer = 0.0f;
		public float speed;
		public float moveDistance = 10.0f;
		protected float usedDistance = 0.0f;
		protected Vector3 start = new Vector3(0.0f, 0.0f, 0.0f);
		protected Vector3 currentLocation = new Vector3(0.0f, 0.0f, 0.0f);
		public Color activeColor;
		protected Renderer render;

		public KeyCode up;
		public KeyCode down;
		public KeyCode left;
		public KeyCode right;

		//float values from InControl, so that players can use the D-Pad/thumbstick
		protected float leftRight = 0.0f;
		public float LeftRight{
			get { return leftRight; }
			set{ leftRight = value; }
		}
		protected float upDown = 0.0f;
		public float UpDown{
			get { return upDown; }
			set{ upDown = value; }
		}
		public float inputDeadZone = 0.5f; //how far does the thumbstick have to move to be registered? <= 1.0f

		protected int mountainFace = 0; //mountain has four faces, numbered 0-3.
		protected int MountainFace{
			get { return mountainFace; }
			set{
				if (value >= 0 && value < 4){
					mountainFace = value;
				} else if (value >= 4){ //prevent mountainFace from going above 3 by wrapping around
					mountainFace = 0;
				} else if (value < 0){ //prevent mountainFace from going below 0 by wrapping around
					mountainFace = 3;
				}
			}
		}
		//how far the climber can turn on the y-axis while staying on a given facing
		//NEVER more than 45.0f
		//see GetCurrentFace() for more details
		public float mountainFaceAngleTolerance = 45;

		protected RopeLinkingScript ropeLinkingScript;


		protected override void Awake(){
			render = GetComponent<Renderer>();
			start = transform.position;
			turnOrderManager = transform.root.Find("TurnOrderManager").GetComponent<TurnOrderManager>();
			timeKeeper = GetComponent<TimeKeeper>();
			ropeLinkingScript = transform.root.Find("RopeManager").GetComponent<RopeLinkingScript>();
			playerNum = int.Parse(gameObject.name[6].ToString()); //assumes players are named "Player#" with no space
		}


		protected void FixedUpdate(){
			inputTimer += Time.deltaTime;
			if (active && inputTimer >= inputWait){
				KeyboardMove();
				ControllerMove();
				usedDistance = GetDistanceTraveled();
				if (usedDistance >= moveDistance){
					turnOrderManager.NewActivePlayer(playerNum);
					active = false;
					timeKeeper.Timescale = 0.0f;
					render.material.color = Color.white;
				}
				start = transform.position; //set the start position for the next frame

				AlignToMountainside();
			}

			ropeLinkingScript.PlayerFaceUpdate(MountainFace, playerNum);
		}


		protected void KeyboardMove(){
			float timeAdjustedSpeed = speed * timeKeeper.DeltaTime;

			MoveByKey(up, transform.up * timeAdjustedSpeed);
			MoveByKey(left, -transform.right * timeAdjustedSpeed);
			MoveByKey(down, -transform.up * timeAdjustedSpeed);
			MoveByKey(right, transform.right * timeAdjustedSpeed);
		}

		protected void MoveByKey(KeyCode key, Vector3 movement){
			if(Input.GetKey(key)){
				transform.position += movement;
			}
		}

		protected void ControllerMove(){
			float timeAdjustedSpeed = speed * timeKeeper.DeltaTime;

			if (UpDown > inputDeadZone){
				MoveByController(transform.up * timeAdjustedSpeed);
			} else if (UpDown < -inputDeadZone){
				MoveByController(-transform.up * timeAdjustedSpeed);
			}

			if (LeftRight < -inputDeadZone){
				MoveByController(-transform.right * timeAdjustedSpeed);
			} else if (LeftRight > inputDeadZone){
				MoveByController(transform.right * timeAdjustedSpeed);
			}
		}

		protected void MoveByController(Vector3 movement){
			transform.position += movement;
		}

		protected float GetDistanceTraveled(){
			currentLocation = transform.position;
			return usedDistance += Vector3.Distance(start, currentLocation);
		}

		/// <summary>
		/// Start by trying to stay aligned with the side you're climbing up.
		/// If you can't (because you've moved past your current side), flip to another side.
		/// </summary>
		protected void AlignToMountainside(){
			if (AlignWithCurrentSide()){
				MountainFace = GetCurrentFace();
				//transform.position = StayCloseToMountain();
			} else {
				Debug.Log("missed!");
			}
		}


		/// <summary>
		/// Send a ray toward the center of the mountain, parallel with the ground.
		/// If it hits the mountain, snap this object's rotation to flat against the mountainside.
		/// 
		/// IMPORTANT: This assumes that the mountain extends upward along the y-axis from (0, 0, 0).
		/// </summary>
		/// <returns><c>true</c> if the ray hits the mountain, <c>false</c> otherwise.</returns>
		protected bool AlignWithCurrentSide(){
			Vector3 mountainNormal = new Vector3(0.0f, 0.0f, 0.0f);
			Vector3 dirToMountainAxis = new Vector3(0.0f, transform.position.y, 0.0f) - transform.position;
			RaycastHit hitInfo;

//			Debug.DrawRay(transform.position, transform.forward, Color.red, 2.0f);
//			Debug.DrawRay(transform.position, transform.right, Color.blue, 2.0f);
//			Debug.DrawRay(transform.position, transform.up, Color.green, 2.0f);
//			Debug.DrawRay(transform.position,
//						  new Vector3(0.0f, transform.position.y, 0.0f) - transform.position,
//						  Color.yellow,
//						  2.0f);

			if (Physics.Raycast(transform.position, dirToMountainAxis, out hitInfo)){
				if (hitInfo.collider.gameObject.name.Contains("Pyramid")){
					mountainNormal = hitInfo.normal;
					Debug.DrawRay(new Vector3(0.0f, transform.position.y, 0.0f), mountainNormal, Color.magenta, 2.0f);

					transform.rotation = Quaternion.LookRotation(-1 * mountainNormal);

					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}


		/// <summary>
		/// Determine which face of the mountain the climber is on, so that the rope can be drawn correctly.
		/// 
		/// This uses the climber's y-axis rotation in degrees. As a result, if the climber's y-axis rotation
		/// changes too far, it will cause the climber to be thought to be on the wrong face.
		/// 
		/// mountainFaceAngleTolerance determines how far the climber can turn away from the face of the mountain.
		/// mountainFaceAngleTolerance can never be more than 45 degrees, or else the angle ranges for different faces
		/// will blend into each other.
		/// </summary>
		/// <returns>The current face.</returns>
		protected int GetCurrentFace(){
			float angle = transform.eulerAngles.y;

			if (angle > 270 - mountainFaceAngleTolerance && angle < 270 + mountainFaceAngleTolerance){
				return 0;
			} else if (angle > 360 - mountainFaceAngleTolerance || angle < 0 + mountainFaceAngleTolerance) {
				return 1;
			} else if (angle > 90 - mountainFaceAngleTolerance && angle < 90 + mountainFaceAngleTolerance) {
				return 2;
			} else {
				return 3;
			}
		}

//		protected Vector3 StayCloseToMountain(){
//			RaycastHit hitInfo;
//			Vector3 dirToMountainAxis = new Vector3(0.0f, transform.position.y, 0.0f) - transform.position;
//
//
//			if (Physics.Raycast(transform.position, dirToMountainAxis, out hitInfo)){
//				//do something
//			}
//		}

		public override void Reset(){
			usedDistance = 0.0f;
			active = true;
			timeKeeper.Timescale = 1.0f;
			render.material.color = activeColor;
			inputTimer = 0.0f;
		}
	}
}
