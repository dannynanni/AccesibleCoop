using UnityEngine;
using System.Collections;

public class ChasingEnemyCreator : EnemyBaseScript {

	private GameObject chasingEnemy;
	private const string CHASING_ENEMY = "ChasingEnemy";

	public float spawnDelay = 1.0f; //time between enemies appearing
	private float timer = 0.0f;

	/*
	 * 
	 * The next two variables are the location in world space where enemies spawn.
	 * Since the background scrolls behind the players, this is constant even as the players appear to move.
	 * 
	 */
	private float xSpawnLoc = 10.0f;
	private float ySpawnLoc = 10.0f;

	private const string ENEMY_ORGANIZER = "Enemies";

	private void Start(){
		transform.parent = GameObject.Find(ENEMY_ORGANIZER).transform;
		chasingEnemy = Resources.Load(CHASING_ENEMY) as GameObject;
	}

	private void Update(){
		timer += Time.deltaTime;

		if (timer >= spawnDelay){
			MakeEnemy();
			timer = 0.0f;
		}
	}

	private void MakeEnemy(){
		GameObject newEnemy = Instantiate(chasingEnemy,
										  new Vector3(xSpawnLoc, ySpawnLoc, 0.0f),
										  Quaternion.identity) as GameObject;
	}

	public override void GetDestroyed(){
		Destroy(gameObject);
	}
}
