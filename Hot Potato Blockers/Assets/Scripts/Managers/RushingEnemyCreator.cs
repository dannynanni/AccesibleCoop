using UnityEngine;
using System.Collections;

public class RushingEnemyCreator : EnemyBaseScript {

	private GameObject rushingEnemy;
	private const string RUSHING_ENEMY_OBJ = "RushingEnemy";

	public float spawnDelay = 1.5f; //time between rushing enemies being spawned
	private float timer = 0.0f;
	private Transform ball;
	private const string BALL_OBJ = "Ball";

	public float ySpawnLoc = 10.0f; //the y-axis location where enemies will spawn

	private const string ENEMY_ORGANIZER = "Enemies";

	private void Start(){
		transform.parent = GameObject.Find(ENEMY_ORGANIZER).transform;
		rushingEnemy = Resources.Load(RUSHING_ENEMY_OBJ) as GameObject;
		ball = transform.root.Find(BALL_OBJ);
	}

	private void Update(){
		timer += Time.deltaTime;

		if (timer >= spawnDelay){
			MakeEnemy();
			timer = 0.0f;
		}
	}

	private void MakeEnemy(){
		Instantiate(rushingEnemy, new Vector3(ball.position.x, ySpawnLoc, 0.0f), Quaternion.identity);
	}

	public override void GetDestroyed(){
		Destroy(gameObject);
	}
}
