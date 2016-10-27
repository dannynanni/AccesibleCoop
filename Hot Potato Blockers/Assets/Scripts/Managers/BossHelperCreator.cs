using UnityEngine;
using System.Collections;

public class BossHelperCreator : MonoBehaviour {

	private GameObject helpingEnemy;
	private const string HELPING_ENEMY = "BossHelperEnemy";

	public float timeBetweenEnemies = 0.5f; //# of seconds between creating an enemy
	private float enemyTimer = 0.0f;
	public float spawnOffset = 0.0f; //can be used to create a slight delay before this spawner kicks off
	private float offsetTimer = 0.0f;
	private bool waitingForOffset = true;

	private const string PLAYER_OBJ = "Player ";
	private Transform myTargetPlayer;

	private const float SCREEN_MIDPOINT = 4.0f;

	private void Start(){
		helpingEnemy = Resources.Load(HELPING_ENEMY) as GameObject;
		myTargetPlayer = GameObject.Find(PLAYER_OBJ + FindTargetPlayer()).transform;
	}

	private void Update(){
		if (waitingForOffset){
			waitingForOffset = WaitForOffset();
		} else {
			enemyTimer += Time.deltaTime;

			if (enemyTimer >= timeBetweenEnemies){
				MakeEnemy();
				enemyTimer = 0.0f;
			}
		}
	}

	private bool WaitForOffset(){
		offsetTimer += Time.deltaTime;

		if (offsetTimer >= spawnOffset){
			return false;
		} else {
			return true;
		}
	}

	private char FindTargetPlayer(){
		if (transform.position.y >= SCREEN_MIDPOINT) { return '1'; }
		else { return '2'; }
	}

	private void MakeEnemy(){
			GameObject newEnemy = Instantiate(helpingEnemy, transform.position, Quaternion.identity) as GameObject;
			newEnemy.GetComponent<BossHelperEnemyBehavior>().Target = myTargetPlayer;
	}
}
