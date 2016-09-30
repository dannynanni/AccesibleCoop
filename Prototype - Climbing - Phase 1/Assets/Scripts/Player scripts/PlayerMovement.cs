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

		protected void FixedUpdate(){
			Move();
			AlignToMountainside();
		}

		protected void Move(){
			Vector3 temp = transform.position;

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
				//do nothing; the player is properly aligned	
			} else {
				Debug.Log("missed!");
			}
		}

		/// <summary>
		/// Send a ray along the transform's forward axis. If it hits the mountain, snap this object's rotation to
		/// flat against the mountainside.
		/// 
		/// IMPORTANT: This assumes that the mountain extends upward along the y-axis from (0, 0, 0).
		/// </summary>
		/// <returns><c>true</c> if the ray hits the mountain, <c>false</c> otherwise.</returns>
		protected bool AlignWithCurrentSide(){
			Vector3 mountainNormal = new Vector3(0.0f, 0.0f, 0.0f);
			Vector3 dirToMountainAxis = new Vector3(0.0f, transform.position.y, 0.0f) - transform.position;
			RaycastHit hitInfo;

			Debug.DrawRay(transform.position, transform.forward, Color.red, 2.0f);
			Debug.DrawRay(transform.position, transform.right, Color.blue, 2.0f);
			Debug.DrawRay(transform.position, transform.up, Color.green, 2.0f);
			Debug.DrawRay(transform.position,
						  new Vector3(0.0f, transform.position.y, 0.0f) - transform.position,
						  Color.yellow,
						  2.0f);

			if (Physics.Raycast(transform.position, dirToMountainAxis, out hitInfo)){
				if (hitInfo.collider.gameObject.name.Contains("Pyramid")){
					Debug.Log("hit");
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

		protected void SwitchSides(){

		}
	}
}
