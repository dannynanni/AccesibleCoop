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

	//public PlayerAbility.ResourceDistribution p0Ability;
	//public PlayerAbility.MovementScript p1Ability;
	//public PlayerAbility.BasicClaw p2Ability;
	//public PlayerAbility.BasicLightWeapon p3Ability;


    void Awake ()
    {

    }

    public void LS(float leftRight, float upDown)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg);

		//if (playerNum == 0){ //the captain should always be playerNum == 0!
		//	if (leftRight <= -0.5f || leftRight >= 0.5f || upDown >= 0.5f){
		//		p0Ability.ChangeSelectedAbility(leftRight, upDown);
		//	}
		//}
    }

    //send button presses to the correct script, depending on which mode the captain has set
    public void AButton(bool pressed)
    {
        //switch (playerNum){
        //	case 0:
        //		p0Ability.Button(pressed);
        //		break;
        //	case 1:
        //		p1Ability.Button(pressed);
        //		break;
        //	case 2:
        //		p2Ability.Button(pressed);
        //		break;
        //	case 3:
        //		p3Ability.Button(pressed);
        //		break;
        //	default:
        //		Debug.Log("Illegal playerNum: " + playerNum);
        //		break;
        //}
    }
}
