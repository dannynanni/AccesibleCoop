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
	private bool firstPass = true; //nothing will spawn until the players have passed the ball once
	public bool FirstPass{
		get { return firstPass; }
		set {
			if (firstPass != value){
				firstPass = value;

				if (firstPass == true){
					MakeEnemy(currentEnemy); //make an enemy right away on the first pass, to provide clearer feedback
				}
			}
		}
	}

	private GameObject currentEnemy;

	private const string BALL_OBJ = "Ball";
	private const string HOMING_ENEMY_OBJ = "HomingEnemy";
	private const string RUSHING_ENEMY_OBJ = "RushingEnemy";

	private void Start(){
		

		SpawnRate = startSpawnRate;
		ball = transform.root.Find(BALL_OBJ);
		currentEnemy = Resources.Load(HOMING_ENEMY_OBJ) as GameObject;
	}

	private void Update(){
		if (FirstPass){
			timer += Time.deltaTime;
		}

		if (timer >= spawnRate){
			timer = 0.0f;
			numEnemies = MakeEnemy(currentEnemy);
		}
	}

	private int MakeEnemy(GameObject enemyType){
		GameObject newEnemy = Instantiate(enemyType,
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

	public void NewEnemyPhase(string phase){
		if (phase.Contains(RUSHING_ENEMY_OBJ)){
			currentEnemy = Resources.Load(RUSHING_ENEMY_OBJ) as GameObject;
		}
	}
}
