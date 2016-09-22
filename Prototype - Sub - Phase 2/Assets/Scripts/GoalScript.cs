using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

	Transform playerVehicle;
	const string PLAYER_VEHICLE = "Players";
	public float winDistance = 2.0f;

	void Start(){
		playerVehicle = GameObject.Find(PLAYER_VEHICLE).transform;
	}

	/// <summary>
	/// Determine whether the players have won. They win if they get close enough to this gameobject.
	/// </summary>
	void Update(){
		if (Vector3.Distance(playerVehicle.position, transform.position) <= winDistance){
			//Do something
			Debug.Log("You win!");
		}
	}
}
