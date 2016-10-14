using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public float chanceOfGoForBall = 0.8f; //between 1.0 and 0.0
	private Transform target;
	private Rigidbody2D rb2D;
	public float speed = 0.3f;

	private void Start(){
		target = ChooseTarget();
		rb2D = GetComponent<Rigidbody2D>();
	}

	private Transform ChooseTarget(){
		const string BALL_OBJ = "Ball";
		const string PLAYER_ORGANIZER = "Players";

		if (Random.Range(0.0f, 1.0f) <= chanceOfGoForBall){
			return transform.root.Find(BALL_OBJ);
		} else {
			Transform players = transform.root.Find(PLAYER_ORGANIZER);

			return (players.GetChild(Random.Range(0, players.childCount)));
		}

	}

	private void FixedUpdate(){
		Move();
	}

	private void Move(){
		Vector3 direction = (target.position - transform.position).normalized;

		rb2D.AddRelativeForce(direction * speed, ForceMode2D.Force);
	}

	private void OnTriggerEnter2D(Collider2D other){
		const string BALL_OBJ = "Ball";

		if (other.name.Contains(BALL_OBJ)){
			Transform ball = other.transform;
			ball.position = transform.position;
			ball.parent = transform;
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
				Debug.Log("You lose!");
			}
		}
	}
}
