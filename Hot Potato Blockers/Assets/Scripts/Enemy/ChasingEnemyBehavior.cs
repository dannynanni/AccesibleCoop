using UnityEngine;
using System.Collections;

public class ChasingEnemyBehavior : EnemyBaseScript {

	public Transform target;
	private Rigidbody2D rb2D;
	public float speed = 0.3f;
	public float Speed{
		get { return speed; }
		set { speed = value; }
	}
	private GameObject destroyParticle;

	private const string BALL = "Ball";

	private void Start(){
		target = transform.root.Find(BALL);
		rb2D = GetComponent<Rigidbody2D>();
		destroyParticle = Resources.Load("DestroyParticle") as GameObject;
	}

	private void FixedUpdate(){
		rb2D.MovePosition(Move());
	}

	private Vector3 Move(){
		Vector3 direction = (target.position - transform.position).normalized;

		//Debug.Log(direction);
		return direction * Speed * Time.deltaTime;
	}

	public override void GetDestroyed(){
		Instantiate(destroyParticle, transform.position, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)));
		Destroy(gameObject);
	}
}
