using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOrderManager : MonoBehaviour {

	//assumes 4 players; change this if that changes
	private Player.PlayerMovement[] players = new Player.PlayerMovement[4];

	private void Start(){
		GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

		if (playerObjects.Length != players.Length) { Debug.Log("Length mismatch!"); }

		for (int i = 0; i < playerObjects.Length; i++){
			players[i] = playerObjects[i].GetComponent<Player.PlayerMovement>();
		}

		NewActivePlayer(-1); //start with player 0.
	}


	/// <summary>
	/// Switch on a new active player. This does not switch the previous player off; players are responsible
	/// for doing that themselves.
	/// </summary>
	/// <param name="previousPlayerNum">The player number of the previous player.</param>
	public void NewActivePlayer(int previousPlayerNum){
		previousPlayerNum++;
		int newPlayerNum = previousPlayerNum;

		if (newPlayerNum > players.Length - 1){
			newPlayerNum = 0;
		}

		players[newPlayerNum].Reset();
	}
}
