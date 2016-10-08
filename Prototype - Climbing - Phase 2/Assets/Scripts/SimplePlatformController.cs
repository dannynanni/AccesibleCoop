using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {


	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	private bool resetting = false;


	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

    private bool controllerJump;
    private bool controllerLeft;
    private bool controllerRight;

	private Vector2 startPos; //where the player started the level
	private Vector2 explodePos; //the location where a player lands on an incorrectly-colored platform
	public float returnDelay = 2.0f; //how long it takes to reset after landing on a wrong-colored platform
	public AnimationCurve returnCurve;
	private float returnTimer = 0.0f;
	public GameObject explosion; //particle effect for when player lands on wrong-colored platform
	private bool stunned = false; //whether the player has been hit by a boulder
	public bool Stunned{
		get { return stunned; }
		set { stunned = value; }
	}
	public float stunDuration = 1.0f;
	private float stunTimer = 0.0f;


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
		//under normal circumstances, check to see if the player is on the wrong color ground,
		//and allow the player to jump
		if (!resetting)
		{
			grounded = CheckGround();
				

			if (Input.GetButtonDown("Jump") && grounded)
			{
				jump = true;
			}

	        else if (controllerJump && grounded)
	        {
	            jump = true;
	        }
		//if the player landed on the wrong color, take them back to start
		} else {
			transform.position = ResetPosition();
			if (Vector2.Distance(rb2d.position, startPos) <= Mathf.Epsilon){
				returnTimer = 0.0f;
				resetting = false;
			}
		}

		//if the player is stunned, run a timer until the time to be stunned runs out
		if (stunned){
			stunTimer += Time.deltaTime;
			if (stunTimer >= stunDuration){
				stunned = false;
				stunTimer = 0.0f;
			}
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
            //prevents player from re-inputting a jump command while mid-air so as to prevent inadvertant bounce.
            controllerJump = false;
			return false; //didn't find any ground; player is in the air
		} else {
			if (hit.collider.tag == gameObject.tag || hit.collider.tag == "Ground"){
				return true; //found ground of the appropriate color, or a neutral color
			} else {
				CrashLand(); //oops! landed on the wrong color
				return true;
			}
		}
	}

	private void CrashLand(){
		resetting = true;
		explodePos = rb2d.position;
		Instantiate(explosion, rb2d.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)));
	}

	private Vector2 ResetPosition(){
		returnTimer += Time.deltaTime;

		Vector2 temp = Vector2.Lerp(explodePos, startPos, returnCurve.Evaluate(returnTimer/returnDelay));

		return temp;
	}

	void FixedUpdate()
	{
		if (!resetting)
		{
            float h;
	        if (controllerRight)
	        {
	            h = 1;
	        }
	        else if (controllerLeft)
	        {
	            h = -1;
	        }
	        else
	        {
	            h = 0;
	        }


			//anim.SetFloat("Speed", Mathf.Abs(h));

			if (h * rb2d.velocity.x < maxSpeed && !stunned)
				rb2d.AddForce(Vector2.right * h * moveForce);

			if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
				rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

			if (h > 0 && !facingRight)
				Flip ();
			else if (h < 0 && facingRight)
				Flip ();

			if (jump && !stunned)
			{
				//anim.SetTrigger("Jump");
				rb2d.AddForce(new Vector2(0f, jumpForce));
				jump = false;
                controllerJump = false;
			}
		}
	}


	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public void ControllerJumpInput (bool buttonDown)
    {
        if (buttonDown)
            controllerJump = true;
        else if (!buttonDown)
            jump = false;
    }

    public void ControllerLeftRight (float leftRight)
    {
        //Debug.Log("controller left stick input");
        if (leftRight > 0.25)
        {
            controllerRight = true;
            controllerLeft = false;
        }
        else if (leftRight < -.25f)
        {
            controllerRight = false;
            controllerLeft = true;
        }
        else
        {
            controllerRight = false;
            controllerLeft = false;
        }
    }

	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.name.Contains("idol")){
			other.GetComponent<BasicIdol>().CheckForDestruction(gameObject);
		}
	}
}