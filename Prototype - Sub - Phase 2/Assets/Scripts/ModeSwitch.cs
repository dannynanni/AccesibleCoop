/*
 * 
 * Use this script to change the submarine's mode between "fight" and "explore."
 * For this script to work, the abilities for each mode must be on separate gameobjects.
 * E.g., the gun, the sonar, even the different ways of moving must be on separate objects.
 * 
 * Call the SwitchMode() script to change states.
 * 
 */

using UnityEngine;
using System.Collections;

public class ModeSwitch : MonoBehaviour {

	GameObject[] fightFeatures;
	GameObject[] exploreFeatures;

	public ControlIO p0;
	public ControlIO p1;
	public ControlIO p2;
	public ControlIO p3;
	ControlIO[] controlIOs;

	bool newInstruction = false;

	void Start(){
		fightFeatures = GameObject.FindGameObjectsWithTag("FightMode");
		exploreFeatures = GameObject.FindGameObjectsWithTag("ExploreMode");
		controlIOs = new ControlIO[] { p0, p1, p2, p3 };
	}

	//Call this script to change the submarine between modes
	void SwitchMode(){
		foreach (GameObject fightPower in fightFeatures){
			fightPower.GetComponent<FightPower>().Active = !fightPower.GetComponent<ExplorePower>().Active;
		}

		foreach (GameObject explorePower in exploreFeatures){
			explorePower.GetComponent<ExplorePower>().Active = !explorePower.GetComponent<ExplorePower>().Active;
		}

		foreach (ControlIO controlScript in controlIOs){
			controlScript.Mode1 = !controlScript.Mode1;
		}
	}

	//switch modes when the button is pressed, one time per press.
	public void Button(bool pressed){
		if (pressed != newInstruction){ //make sure the button isn't being held down
			newInstruction = pressed;

			if (newInstruction){ //this is a new change in the button state; if the button is being pressed, switch modes
				SwitchMode();
			}
		}
	}
}
