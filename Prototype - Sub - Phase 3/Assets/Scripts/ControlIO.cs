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

    void Awake ()
    {

    }

    public void LS(float leftRight, float upDown)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg);
    }

    //send button presses to the correct script, depending on which mode the captain has set
    public void AButton (bool pressed )
    {
		//send out to relevant scripts here
    }
}
