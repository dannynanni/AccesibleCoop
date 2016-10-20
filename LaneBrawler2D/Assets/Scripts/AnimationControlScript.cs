using UnityEngine;
using System.Collections;

public class AnimationControlScript : MonoBehaviour {

    public Animator myAnimator;

    private bool isLeft;
    public bool ISLEFT {
        get {
            return isLeft;
        }
        set {
            if (value != isLeft) {
                float leftIsPlus;
                if (value) {
                    leftIsPlus = 1;
                }
                else {
                    leftIsPlus = -1;
                }
                transform.localScale = new Vector3(leftIsPlus, 1, 1);
                isLeft = value;
            }
        }
    }

    public void setAnimState (animStates animState)
    {
        switch (animState)
        {
            case (animStates.aIdle):
                myAnimator.SetBool("idle", true);
                myAnimator.SetBool("walk", false);
                myAnimator.SetBool("cast", false);
                //Debug.Log("only idle true");
                break;
            case (animStates.aWalk):
                myAnimator.SetBool("idle", false);
                myAnimator.SetBool("walk", true);
                myAnimator.SetBool("cast", false);
                //Debug.Log("only walk true");
                break;
            case (animStates.aCast):
                myAnimator.SetBool("idle", false);
                myAnimator.SetBool("walk", false);
                myAnimator.SetBool("cast", true);
                //Debug.Log("only cast true");
                break;
        }
    }


	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    setAnimState(animStates.aCast);
        //} 
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    setAnimState(animStates.aWalk);
        //    ISLEFT = false;
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    setAnimState(animStates.aWalk);
        //    ISLEFT = true;
        //}
        //else
        //{
        //    setAnimState(animStates.aIdle);
        //}

    }
}

public enum animStates { aIdle, aWalk, aCast }


