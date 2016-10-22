using UnityEngine;
using System.Collections;

public class BasicPlayer : MonoBehaviour {

	private bool ballCarrier = false;
	public bool BallCarrier{
		get { return ballCarrier; }
		set { ballCarrier = value; }
	}

	public char playerNum = '0';

	private const string SQUARE_BUTTON = "PS4_Square_";
	private const string TRIANGLE_BUTTON = "PS4_Triangle_";
	private const string CIRCLE_BUTTON = "PS4_O_";
	private const string X_BUTTON = "PS4_X_";
	private const string VERT_AXIS = "PS4_DPad_Vert_";
	private const string HORIZ_AXIS = "PS4_DPad_Horiz_";

	public float baseSpeed = 1.0f;
	public float dashSpeed = 2.0f;
	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public KeyCode toss;
	public KeyCode dash;

	private Rigidbody2D rb2D;

	private Transform otherPlayer;
	private const string BALL_OBJ = "Ball";

	private bool tackled = false;
	public bool Tackled{
		get { return tackled; }
		set { tackled = value; }
	}
	public float tackleKnockback = 5.0f;
	public float tackleDuration = 2.0f;
	private float tackleTimer = 0.0f;
	private EnemyCreator enemyCreator;
	private FieldBehavior fieldBehavior;
	private const string FIELD_OBJ = "Field";

	private const string PHASE_CHANGE_OBJ = "Change";


	private void Start(){
		const string ENEMYCREATOR_OBJ = "Enemy creator";
		playerNum = gameObject.name[7]; //assumes players are named using the convention "Player #"
		rb2D = GetComponent<Rigidbody2D>();
		otherPlayer = FindOtherPlayer();
		enemyCreator = GameObject.Find(ENEMYCREATOR_OBJ).GetComponent<EnemyCreator>();
		fieldBehavior = GameObject.Find(FIELD_OBJ).GetComponent<FieldBehavior>();
	}

	private Transform FindOtherPlayer(){
		Transform players = transform.parent;
		Transform temp = transform;

		foreach (Transform player in players){
			if (!player.name.Contains(playerNum.ToString())){
				temp = player;
			}
		}

		Debug.Log("otherPlayer for " + gameObject.name + " set to " + temp);
		if (temp == transform) { Debug.Log("Couldn't find other player for " + gameObject.name); }

		return temp;
	}

	private void Update(){
		if (BallCarrier){
			BallCarrier = TryToThrow();
			//TryToMoveField();
		}
	}

	private void FixedUpdate(){
		float currentSpeed = baseSpeed;

		if (!BallCarrier){
			currentSpeed = AmIDashing();
		}

		if (!Tackled){
			rb2D.MovePosition(transform.position + Move(currentSpeed));
		} else if (Tackled){
			Tackled = RunTackleTimer();
		}

		//transform.position = KeepPlayerOnScreen();
	}

	private float AmIDashing(){
		if (Input.GetButton(CIRCLE_BUTTON + playerNum) || Input.GetKey(dash)){
			return dashSpeed;
		} else {
			return baseSpeed;
		}
	}

	private Vector3 Move(float currentSpeed){
		Vector3 temp = new Vector3(0.0f, 0.0f, 0.0f);

		if (Input.GetKey(up) || Input.GetAxis(VERT_AXIS + playerNum) > 0.3f){
			temp.y += currentSpeed;
		} else if (Input.GetKey(down) || Input.GetAxis(VERT_AXIS + playerNum) < -0.3f){
			temp.y -= currentSpeed;
		}

		if (Input.GetKey(left) || Input.GetAxis(HORIZ_AXIS + playerNum) < -0.3f){
			temp.x -= currentSpeed;
		} else if (Input.GetKey(right) || Input.GetAxis(HORIZ_AXIS + playerNum) > 0.3f){
			temp.x += currentSpeed;
		}

		return temp;
	}

	private bool TryToThrow(){
		if (Input.GetButton(CIRCLE_BUTTON + playerNum) || Input.GetKey(toss)){
			Debug.Log("Input read");
			if (transform.childCount > 0){
				Debug.Log("Trying to throw");
				StartCoroutine(transform.Find(BALL_OBJ).GetComponent<BallBehavior>()
					.PassBetweenPlayers(transform, otherPlayer));
				enemyCreator.FirstPass = true; //we've passed at least once, OK to start spawning enemies
				enemyCreator.ResetNumEnemies();
				return false;
			}
		}

		return true;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.name.Contains(BALL_OBJ)){
			other.transform.position = transform.position;
			other.transform.parent = transform;
			BallCarrier = true;
		} else if (other.name.Contains(PHASE_CHANGE_OBJ)){
			enemyCreator.NewEnemyPhase(other.name);
		}
	}

	public void GetTackled(Transform opposingPlayer){
		Tackled = true;
		Vector3 tackleDirection = (opposingPlayer.position - transform.position).normalized;

		rb2D.AddRelativeForce(tackleDirection * tackleKnockback, ForceMode2D.Impulse);
	}

	private bool RunTackleTimer(){
		tackleTimer += Time.deltaTime;

		if (tackleTimer >= tackleDuration){
			tackleTimer = 0.0f;
			rb2D.velocity = Vector3.zero;
			return false;
		} else {
			return true;
		}
	}

	private void TryToMoveField(){
		if (Input.GetKey(up) || Input.GetAxis(VERT_AXIS + playerNum) > 0.3f){
			fieldBehavior.ScrollField();
		}
	}
}
