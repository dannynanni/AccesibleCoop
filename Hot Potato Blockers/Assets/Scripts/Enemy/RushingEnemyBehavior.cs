using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RushingEnemyBehavior : MonoBehaviour {

	public Vector3 target;
	private Rigidbody2D rb2D;
	public float speed = 0.3f;
	public float Speed{
		get { return speed; }
		set { speed = value; }
	}
	public float resetDelay = 1.0f;
	private bool rushing = true;

	private void Start(){
		transform.parent = GameObject.Find("Scene").transform;
		Debug.Log("Start called");
		target = ChooseTarget();
		rb2D = GetComponent<Rigidbody2D>();
	}

	private Vector3 ChooseTarget(){
		//Debug.Log("ChooseTarget called");
		const string BALL_OBJ = "Ball";

		return GameObject.Find(BALL_OBJ).transform.position;
	}

	private void FixedUpdate(){
		//Debug.Log("FixedUpdate called");
		if (rushing){
			Move();
		}

		if (Vector3.Distance(transform.position, target) <= Mathf.Epsilon){
			rushing = false; //stop adding force once you pass through the target position
		}
	}

	private void Move(){
		Vector3 direction = (target - transform.position).normalized;

		//Debug.Log(direction);
		rb2D.AddRelativeForce(direction * speed, ForceMode2D.Force);
	}

	private void OnTriggerEnter2D(Collider2D other){
		const string BALL_OBJ = "Ball";

		if (other.name.Contains(BALL_OBJ)){
			Transform ball = other.transform;
			ball.position = transform.position;
			ball.parent = transform;
			StartCoroutine(Reset());
			Debug.Log("You lose!");
		}
	}

	private void OnCollisionEnter2D(Collision2D collision){
		const string PLAYER_OBJ = "Player";
		const string BALL_OBJ = "Ball";

		if (collision.gameObject.name.Contains(PLAYER_OBJ)){

			collision.gameObject.GetComponent<BasicPlayer>().GetTackled(transform);
			if (collision.gameObject.GetComponent<BasicPlayer>().BallCarrier){
				Transform ball = GameObject.Find(BALL_OBJ).transform;
				ball.position = transform.position;
				ball.parent = transform;
				collision.gameObject.GetComponent<BasicPlayer>().BallCarrier = false;
				Time.timeScale = 0.0f;
				StartCoroutine(Reset());
				Debug.Log("You lose!");
			}
		}
	}

	private IEnumerator Reset(){
		float timer = 0.0f;
		float start = Time.realtimeSinceStartup;

		while (timer < resetDelay){
			timer = Time.realtimeSinceStartup - start;

			yield return null;
		}

		Time.timeScale = 1.0f; //put the timescale back to normal, or else the game will be stopped on reload

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		yield break;
	}
}
