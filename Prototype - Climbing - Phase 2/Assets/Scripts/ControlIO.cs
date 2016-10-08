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
    public SimplePlatformController myControl;

    void Awake ()
    {
        myControl = GetComponent<SimplePlatformController>();
    }

    public void LS(float leftRight, float upDown)
    {
            myControl.ControllerLeftRight(leftRight);
    }

    //send button presses to the correct script, depending on which mode the captain has set
    public void AButton (bool pressed)
    {
        myControl.ControllerJumpInput(pressed);
    }
}
