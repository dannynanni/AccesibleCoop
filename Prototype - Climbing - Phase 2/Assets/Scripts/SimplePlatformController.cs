using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	private Vector2 startPos;


	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

	public GameObject explosion; //the particle that warns players that they stepped on the wrong color


	// Use this for initialization
	void Awake () 
	{
		//anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		grounded = CheckGround();
			

		if (Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
		}
	}

	/// <summary>
	/// Determines whether the player is on the ground. If so, and it's the wrong color, resets the player's position.
	/// </summary>
	/// <returns><c>true</c>, if player is on the ground, <c>false</c> otherwise.</returns>
	private bool CheckGround(){
		RaycastHit2D hit = Physics2D.Linecast(transform.position,
											  groundCheck.position,
											  1 << LayerMask.NameToLayer("Ground"));
		if (!hit){
			return false; //didn't find any ground; player is in the air
		} else {
			if (hit.collider.tag == gameObject.tag || hit.collider.tag == "Ground"){
				return true; //found ground of the appropriate color, or a neutral color
			} else {
				ResetPosition();
				return true;
			}
		}
	}

	private void ResetPosition(){

	}

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");

		//anim.SetFloat("Speed", Mathf.Abs(h));

		if (h * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * h * moveForce);

		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();

		if (jump)
		{
			//anim.SetTrigger("Jump");
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}


	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}