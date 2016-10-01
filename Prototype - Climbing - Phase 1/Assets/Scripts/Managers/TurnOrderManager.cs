using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TurnOrderManager : MonoBehaviour {

	//assumes 4 players; change this if that changes
	private List<Player.PlayerMovement> playerList = new List<Player.PlayerMovement>();


	private void Start(){
		GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < playerObjects.Length; i++){
			playerList.Add(playerObjects[i].GetComponent<Player.PlayerMovement>());
		}
		playerList.Sort(CompareByNames);

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

		if (newPlayerNum > playerList.Count - 1){
			newPlayerNum = 0;
		}

		playerList[newPlayerNum].Reset();
		Debug.Log("new active player: " + newPlayerNum);
	}


	/// <summary>
	/// Determine which of two objects comes first in name order.
	/// </summary>
	/// <returns>An int that the Sort function can use to order the objects.</returns>
	/// <param name="x">The first object to sort.</param>
	/// <param name="y">The second object to sort.</param>
	private int CompareByNames(Player.PlayerMovement x, Player.PlayerMovement y){
		return x.gameObject.name.CompareTo(y.gameObject.name);
	}
}
