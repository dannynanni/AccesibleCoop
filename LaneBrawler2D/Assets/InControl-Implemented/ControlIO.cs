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

    public PlayerMovementController myPMC;

    void Start ()
    {
        myPMC = GetComponent<PlayerMovementController>();
    }

    public void LS(float leftRight, float upDown)
    {
        //transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg);
        myPMC.JoystickLeft(leftRight);

    }

    //send button presses to the correct script, depending on which mode the captain has set
    public void AButton (bool pressed)
    {
        myPMC.ButtonInput(0, pressed);
    }
    public void BButton(bool pressed)
    {
        myPMC.ButtonInput(1, pressed);
    }
    public void XButton(bool pressed)
    {
        myPMC.ButtonInput(2, pressed);
    }
}
