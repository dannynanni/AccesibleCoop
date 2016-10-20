using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    private SpecialAbilityInterface mySpecials;

    public Rigidbody2D myRB;

    public AnimationControlScript myACS;

    public float speed;

    public AnimationCurve jumpForceMod;
    public float jumpScalar;
    private float JUMPSCALARMODIFIED;

    private delegate void Executor();
    private Executor executor;

    public bool onGround;

    private bool left;
    private bool LEFT
    {
        get
        {
            return left;
        }
        set
        {
            if (left != value)
            {
                 if (value)
                {
                    myACS.ISLEFT = true;
                }
                else
                {
                    myACS.ISLEFT = false;
                }
                left = value;
            }
        }
    }

    public float jumpInc;
    private bool isJumping;
    private bool ISJUMPING
    {
        get
        {
            return isJumping;
        }
        set
        {
            if (value != isJumping)
            {
                //Debug.Log("Setting ISJUMPING to " + value);
                if (value)
                {
                    jumpInc = 0;
                }
                isJumping = value;
            }
        }
    }
    public float jumpScaleRate;

    private int movementLeftRight;
    private int abilityLeftRight = 1;

    public bool actionJump;
    public bool actionS1;
    public bool actionS2;

    void Awake ()
    {
        mySpecials = GetComponent<SpecialAbilityInterface>();
    }

    void FixedUpdate()
    {
        jumpInc += jumpScaleRate;
        if (jumpInc > 1 || !ISJUMPING)
        {
            jumpInc = 1;
            JUMPSCALARMODIFIED = 0;
            ISJUMPING = false;
        }
        else if (jumpInc <= 1 && ISJUMPING)
        {
            JUMPSCALARMODIFIED = jumpScalar;
        }

        myRB.AddForce(/* JumpForce */Vector3.up * jumpScalar * jumpForceMod.Evaluate(jumpInc) + 
            /* MoveForce */ transform.right * movementLeftRight * speed, ForceMode2D.Force);
        //myRB.velocity = (Vector2.ClampMagnitude(myRB.velocity, 10));
    }

    void Update () {

        //if (Input.GetKey(KeyCode.E))
        //{
        //    myACS.setAnimState(animStates.aCast);
        //    mySpecials.SpecialAbility1(abilityLeftRight);
        //}
        //else if (Input.GetKey(KeyCode.Q))
        //{
        //    myACS.setAnimState(animStates.aCast);
        //    mySpecials.SpecialAbility2(abilityLeftRight);
        //}
        //else
        //{
        //    myACS.setAnimState(animStates.aIdle);
        //    mySpecials.Reset2();
        //}

        if (actionS1)
        {
            myACS.setAnimState(animStates.aCast);
            mySpecials.SpecialAbility1(abilityLeftRight);
        }
        else if (actionS2)
        {
            myACS.setAnimState(animStates.aCast);
            mySpecials.SpecialAbility2(abilityLeftRight);
        }
        else
        {
            myACS.setAnimState(animStates.aIdle);
            mySpecials.Reset2();
        }


        if (actionJump && onGround)
        {
            myACS.setAnimState(animStates.aCast);
            ISJUMPING = true;
        }
        else if (!actionJump)
        {
            ISJUMPING = false;
        }

        //if (Input.GetKeyDown(KeyCode.Space) && onGround)
        //{
        //    myACS.setAnimState(animStates.aCast);
        //    ISJUMPING = true;
        //}
        //else if (Input.GetKey(KeyCode.Space))
        //{
        //    myACS.setAnimState(animStates.aCast);
        //}
        //else if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    ISJUMPING = false;
        //}

        //else if (Input.GetKey(KeyCode.D))
        //{
        //    LEFT = false;
        //    movementLeftRight = 1;
        //    abilityLeftRight = 1;
        //    myACS.setAnimState(animStates.aWalk);
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    LEFT = true;
        //    movementLeftRight = -1;
        //    abilityLeftRight = -1;
        //    myACS.setAnimState(animStates.aWalk);
        //}
        //else
        //{
        //    movementLeftRight = 0;
        //    myACS.setAnimState(animStates.aIdle);
        //}
    }

    public void JoystickLeft (float leftRight)
    {
        if (leftRight < -.25f)
        {
            LEFT = true;
            movementLeftRight = -1;
            abilityLeftRight = -1;
            myACS.setAnimState(animStates.aWalk);
        }
        else if (leftRight > .25f)
        {
            LEFT = false;
            movementLeftRight = 1;
            abilityLeftRight = 1;
            myACS.setAnimState(animStates.aWalk);
        }
        else
        {
            movementLeftRight = 0;
            myACS.setAnimState(animStates.aIdle);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        onGround = true;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        onGround = true;
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        onGround = false;
    }

    public void ButtonInput (int button, bool pressed)
    {
        if (button == 0)
        {
            actionJump = pressed;
        }
        else if (button == 1)
        {
            actionS1 = pressed;
        }
        else if (button == 2)
        {
            actionS2 = pressed;
        }

    }
}
