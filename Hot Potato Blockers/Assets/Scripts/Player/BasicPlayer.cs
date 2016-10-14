using UnityEngine;
using System.Collections;

public class BasicPlayer : MonoBehaviour {

	private bool ballCarrier = false;
	public bool BallCarrier{
		get { return ballCarrier; }
		set { ballCarrier = value; }
	}

	public int playerNum = 0;

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


	private void Start(){
		rb2D = GetComponent<Rigidbody2D>();
		otherPlayer = FindOtherPlayer();
	}

	private Transform FindOtherPlayer(){
		Transform players = transform.parent;
		Transform temp = transform;

		foreach (Transform player in players){
			if (!player.name.Contains(playerNum.ToString())){
				temp = player;
			}
		}

		if (temp == transform) { Debug.Log("Couldn't find other player for " + gameObject.name); }

		return temp;
	}

	private void Update(){
		float currentSpeed = baseSpeed;

		if (!BallCarrier){
			currentSpeed = AmIDashing();
		}
		rb2D.MovePosition(transform.position + Move(currentSpeed));

		if (BallCarrier){
			BallCarrier = TryToThrow();
		}
	}

	private float AmIDashing(){
		if (Input.GetButton(CIRCLE_BUTTON + playerNum.ToString())){
			return dashSpeed;
		} else {
			return baseSpeed;
		}
	}

	private Vector3 Move(float currentSpeed){
		Vector3 temp = new Vector3(0.0f, 0.0f, 0.0f);

		if (Input.GetKey(up) || Input.GetAxis(VERT_AXIS + playerNum.ToString()) > 0.3f){
			temp.y += currentSpeed;
		} else if (Input.GetKey(down) || Input.GetAxis(VERT_AXIS + playerNum.ToString()) < -0.3f){
			temp.y -= currentSpeed;
		}

		if (Input.GetKey(left) || Input.GetAxis(HORIZ_AXIS + playerNum.ToString()) < -0.3f){
			temp.x -= currentSpeed;
		} else if (Input.GetKey(right) || Input.GetAxis(HORIZ_AXIS + playerNum.ToString()) > 0.3f){
			temp.x += currentSpeed;
		}

		return temp;
	}

	private bool TryToThrow(){
		if (Input.GetButton(CIRCLE_BUTTON + playerNum.ToString())){
			if (transform.childCount > 0){
				transform.Find(BALL_OBJ).GetComponent<BallBehavior>().PassBetweenPlayers(transform, otherPlayer);
				return false;
			}
		}

		return true;
	}

}
