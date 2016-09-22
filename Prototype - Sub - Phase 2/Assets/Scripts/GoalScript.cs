using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

	Transform playerVehicle;
	const string PLAYER_VEHICLE = "Players";
	public float winDistance = 2.0f;
    ScoreScript scoreScript;

	void Start(){
		playerVehicle = GameObject.Find(PLAYER_VEHICLE).transform;
        scoreScript = GameObject.Find("Score").GetComponent<ScoreScript>();
	}

	/// <summary>
	/// Determine whether the players have won. They win if they get close enough to this gameobject.
	/// </summary>
	void Update(){
		if (scoreScript.Score == 7){
			//Do something
			Debug.Log("You win!");
		}
	}
}
