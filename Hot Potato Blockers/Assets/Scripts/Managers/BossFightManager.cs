using UnityEngine;
using System.Collections;

public class BossFightManager : MonoBehaviour {

	private const string ENEMY_ORGANIZER = "Enemies";
	public Vector3 startLoc = new Vector3(13.0f, 0.0f, 0.0f); //start just off screen
	private float speed = 0.03f;
	private const string FIELD_OBJ = "Field";

	private void Start(){
		transform.parent = GameObject.Find(ENEMY_ORGANIZER).transform;
		transform.position = startLoc;
		speed = GameObject.Find(FIELD_OBJ).GetComponent<FieldBehavior>().speed; //match this speed to the background's, so it flows in naturally
	}

	//make the boss fight scroll onto the screen, as though the players are approaching it
	private void Update(){
		Vector3 temp = new Vector3(speed, 0.0f, 0.0f);

		if (transform.position.x > 0.0f){
			transform.position -= temp;
		}
	}
}
