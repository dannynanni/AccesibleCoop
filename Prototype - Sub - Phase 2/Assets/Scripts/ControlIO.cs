/*
 * 
 * This script takes button presses from InControl and sends them out to the scripts that players use to do things.
 * 
 * To enable new verbs, write a new ~Abilities function, and call it from the AButton function (replacing whichever mode you're getting rid of).
 * Follow the pattern for ExploreAbilities and FightAbilities (or something else that's more elegant, but the pattern will work ;-) ).
 * 
 */

using UnityEngine;
using System.Collections;

public class ControlIO : MonoBehaviour {

    public int playerNum;

	public ModeSwitch p0Explore;
	public MovementScript p1Explore;
	public BasicClaw p2Explore;
    public SonarScript p3Explore;

	public ModeSwitch p0Fight;
	public MovementScript p1Fight;
	public GunFireScript p2Fight;
	public ShieldScript p3Fight;

	bool mode1 = false; //currently 1 == fight, 2 == explore
	public bool Mode1{
		get { return mode1; }
		set { mode1 = value; }
	}

	bool takingInput = true;
	public bool TakingInput{
		get { return takingInput; }
		set{
			takingInput = value;
		}
	}

    void Awake ()
    {

    }

    public void LS(float leftRight, float upDown)
    {
		if (TakingInput){
        	transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg);
		}
    }

    //send button presses to the correct script, depending on which mode the captain has set
    public void AButton (bool pressed )
    {
		if (TakingInput){
			switch(Mode1){
				case true:
					FightAbilities(pressed);
					break;
				case false:
					ExploreAbilities(pressed);
					break;
			}
		}
    }

	/*
	 * Distribute the button state (pressed or not pressed) to the appropriate scripts.
	 * 
	 * This script does NOT determine what the button state means--that is left to the different scripts.
	 * 
	 */
	void FightAbilities(bool pressed){
		switch(playerNum){
			case 0:
				p0Fight.Button(pressed);
				break;
			case 1:
				p1Fight.Button(pressed);
				break;
			case 2:
				p2Fight.PullTrigger(pressed);
				break;
			default:
				Debug.Log("Illegal playerNum: " + playerNum);
				break;
		}
	}

	/*
	 * Distribute the button state (pressed or not pressed) to the appropriate scripts.
	 * 
	 * This script does NOT determine what the button state means--that is left to the different scripts.
	 * 
	 */
	void ExploreAbilities(bool pressed){
		switch(playerNum){
			case 0:
				p0Explore.Button(pressed);
				break;
			case 1:
				p1Explore.Button(pressed);
				break;
			case 2:
				p2Explore.Button(pressed);
				break;
			default:
                p3Explore.MakeAPing(pressed);
				break;
		}
	}
}
