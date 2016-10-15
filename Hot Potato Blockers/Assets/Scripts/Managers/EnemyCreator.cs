using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {

	public float spawnDist = 10.0f;
	public float minSpawnRate = 1.0f;
	public float startSpawnRate = 5.0f;
	private float spawnRate = 1.0f;
	public float SpawnRate{
		get { return spawnRate; }
		set { 
			if (value >= minSpawnRate){
				spawnRate = value;
			} else {
				spawnRate = minSpawnRate;
			}
		}
	}
	private float timer = 0.0f;
	public GameObject enemy;
	private int numEnemies = 0;
	private Transform ball;
	private bool firstPass = false; //nothing will spawn until the players have passed the ball once
	public bool FirstPass{
		get { return firstPass; }
		set { firstPass = true; }
	}

	private void Start(){
		const string BALL_OBJ = "Ball";

		SpawnRate = startSpawnRate;
		ball = transform.root.Find(BALL_OBJ);
	}

	private void Update(){
		if (FirstPass){
			timer += Time.deltaTime;
		}

		if (timer >= spawnRate){
			timer = 0.0f;
			numEnemies = MakeEnemy();
		}
	}

	private int MakeEnemy(){
		GameObject newEnemy = Instantiate(enemy,
										  new Vector3(ball.position.x + spawnDist * PosOrNegOne(),
													  ball.position.y + spawnDist * PosOrNegOne(),
													  0.0f),
										  Quaternion.identity) as GameObject;
		numEnemies += 1;
		SpawnRate--;

		newEnemy.GetComponent<EnemyBehavior>().Speed = numEnemies;

		return numEnemies;
	}

	private int PosOrNegOne(){
		return Random.Range(0,2) * 2 - 1;
	}

	public void ResetNumEnemies(){
		numEnemies = 0;
		SpawnRate = startSpawnRate;
	}
}
