using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TurnOrderManager : MonoBehaviour {

	CameraManagerScript cameraManager;
	//assumes 4 players; change this if that changes
	private List<TurnTakers.ThingsThatTakeTurns> playerList = new List<TurnTakers.ThingsThatTakeTurns>();


	private void Start(){
		//prepare the list of everything that will have a turn, inlcuding both players and the environment
		GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < playerObjects.Length; i++){
			playerList.Add(playerObjects[i].GetComponent<TurnTakers.ThingsThatTakeTurns>());
		}
		playerList.Sort(CompareByNames);
		cameraManager = transform.root.Find("CameraManager").GetComponent<CameraManagerScript>();

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
		cameraManager.SetActivePlayer(playerList[newPlayerNum].gameObject);
	}


	/// <summary>
	/// Determine which of two objects comes first in name order.
	/// </summary>
	/// <returns>An int that the Sort function can use to order the objects.</returns>
	/// <param name="x">The first object to sort.</param>
	/// <param name="y">The second object to sort.</param>
	private int CompareByNames(TurnTakers.ThingsThatTakeTurns x, TurnTakers.ThingsThatTakeTurns y){
		return x.gameObject.name.CompareTo(y.gameObject.name);
	}
}
