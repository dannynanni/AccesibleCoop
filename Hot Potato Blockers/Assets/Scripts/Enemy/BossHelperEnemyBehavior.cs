using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossHelperEnemyBehavior : EnemyBaseScript {

	private Transform target;
	public Transform Target{
		get { return target; }
		set { target = value; }
	}
	private Rigidbody2D rb2D;
	public float speed = 0.3f;
	public float Speed{
		get { return speed; }
		set { speed = value; }
	}
	public float resetDelay = 1.0f;
	private GameObject destroyParticle;
	private const string PLAYER_ORGANIZER = "Players";

	private void Start(){
		transform.parent = GameObject.Find("Scene").transform.Find("Enemies");
		rb2D = GetComponent<Rigidbody2D>();
		destroyParticle = Resources.Load("DestroyParticle") as GameObject;
	}

	private void FixedUpdate(){
		Move();
	}

	private void Move(){
		Vector3 direction = (target.position - transform.position).normalized;

		rb2D.AddRelativeForce(direction * speed, ForceMode2D.Force);
	}

	public override void GetDestroyed(){
		Instantiate(destroyParticle, transform.position, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)));
		Destroy(gameObject);
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
				Time.timeScale = 0.0f;
				StartCoroutine(Reset());
				Debug.Log("You lose!");
			} else {
				GetDestroyed(); //enemies are destroyed if body-blocked
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
