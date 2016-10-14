using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {

	public float spawnDist = 10.0f;
	public float spawnRate = 1.0f;
	private float timer = 0.0f;
	public GameObject enemy;
	private int numEnemies = 0;
	private Transform ball;

	private void Start(){
		const string BALL_OBJ = "Ball";

		ball = transform.root.Find(BALL_OBJ);
	}

	private void Update(){
		timer += Time.deltaTime;

		//Debug.Log(timer);
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

		newEnemy.GetComponent<EnemyBehavior>().Speed = numEnemies;

		return numEnemies;
	}

	private int PosOrNegOne(){
		return Random.Range(0,2) * 2 - 1;
	}

	public void ResetNumEnemies(){
		numEnemies = 0;
	}
}
