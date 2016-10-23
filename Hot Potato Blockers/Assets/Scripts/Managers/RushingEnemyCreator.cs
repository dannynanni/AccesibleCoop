using UnityEngine;
using System.Collections;

public class RushingEnemyCreator : MonoBehaviour {

	private GameObject rushingEnemy;
	private const string RUSHING_ENEMY_OBJ = "RushingEnemy";

	public float spawnDelay = 1.5f; //time between rushing enemies being spawned
	private float timer = 0.0f;
	private Transform ball;
	private const string BALL_OBJ = "Ball";

	public float spawnDist = 10.0f; //the y-axis location where enemies will spawn

	private void Start(){
		rushingEnemy = Resources.Load(RUSHING_ENEMY_OBJ) as GameObject;
		Debug.Log(rushingEnemy);
		ball = transform.root.Find(BALL_OBJ);
		Debug.Log(ball);
		gameObject.SetActive(false); //turn this enemy creator off until the player reaches a certain distance
	}

	private void Update(){
		timer += Time.deltaTime;

		if (timer >= spawnDelay){
			MakeEnemy();
			timer = 0.0f;
		}
	}

	private void MakeEnemy(){
		Instantiate(rushingEnemy, new Vector3(ball.position.x, spawnDist, 0.0f), Quaternion.identity);
	}
}
