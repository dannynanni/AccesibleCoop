/*
 * 
 * This script switches between enemy types. It will destroy all the previously-existing enemies, and then
 * instantiate the enemy creator(s) for the enemies that should start appearing.
 * 
 * When you want to change to a new type of enemy, drag the "phase change" prefab to the part of the level
 * where you want the new enemies to start appearing, and add the new enemy creator prefabs to the string array
 * in the inspector.
 * 
 */

using UnityEngine;
using System.Collections;

public class ChangeEnemyType : MonoBehaviour {

	[Header("Add enemy creator prefabs here")]
	public GameObject[] newEnemyTypes; //This MUST be the enemy creator prefab(s) you want to switch to

	private Transform enemyOrganizer; //the hierarchy item that enemies are grouped under
	private const string ENEMY_ORGANIZER = "Enemies";

	private void Start(){
		enemyOrganizer = transform.root.Find(ENEMY_ORGANIZER);
	}

	//when the player passes a checkpoint, destroy all existing enemies and set up the creator for the new ones
	public void NewEnemyPhase(){
		DestroyOldEnemies();
		MakeNewEnemies();
	}

	//go through all the existing enemies and get rid of them
	private void DestroyOldEnemies(){
		foreach (Transform enemy in enemyOrganizer){
			enemy.GetComponent<EnemyBaseScript>().GetDestroyed();
		}
	}

	//instantiate all the enemy creators in the public string array
	private void MakeNewEnemies(){
		foreach (GameObject enemyType in newEnemyTypes){
			GameObject newEnemyCreator = Instantiate(enemyType, Vector3.zero, Quaternion.identity) as GameObject;
		}
	}
}
