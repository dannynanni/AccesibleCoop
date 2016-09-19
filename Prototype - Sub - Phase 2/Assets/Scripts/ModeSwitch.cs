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

	void Start(){
		fightFeatures = GameObject.FindGameObjectsWithTag("FightMode");
		exploreFeatures = GameObject.FindGameObjectsWithTag("ExploreMode");
	}

	//Call this script to change the submarine between modes
	public void SwitchMode(){
		ReverseState(fightFeatures);
		ReverseState(exploreFeatures);
	}

	void ReverseState(GameObject[] featuresToChange){
		foreach (GameObject feature in featuresToChange){
			feature.SetActive(!feature.activeInHierarchy);
		}
	}
}
