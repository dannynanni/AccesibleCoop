namespace Player
{
	using UnityEngine;
	using System.Collections;

	public class PlayerMovement : MonoBehaviour {

		public float speed;

		public KeyCode up;
		public KeyCode down;
		public KeyCode left;
		public KeyCode right;

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
		protected int playerNum = 0;

		protected void Start(){
			ropeLinkingScript = transform.root.Find("RopeManager").GetComponent<RopeLinkingScript>();
			playerNum = int.Parse(gameObject.name[6].ToString()); //assumes players are named "Player#" with no space
		}


		protected void FixedUpdate(){
			Move();
			AlignToMountainside();
			ropeLinkingScript.PlayerFaceUpdate(MountainFace, playerNum);
		}


		protected void Move(){
			float timeAdjustedSpeed = speed * Time.deltaTime;

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
	}
}
