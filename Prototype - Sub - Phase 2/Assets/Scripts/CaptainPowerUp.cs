using UnityEngine;
using System.Collections;

public class CaptainPowerUp : MonoBehaviour {

	Transform captain;
	protected GameObject[] explorePowers;
	protected GameObject[] fightPowers;
	protected const string EXPLORE_TAG = "ExploreMode";
	protected const string FIGHT_TAG = "FightMode";
	public float powerUpAngle = 10.0f; //how close the captain has to match someone else's direction to power them up

	protected void Start(){
		captain = transform.parent;
		explorePowers = GameObject.FindGameObjectsWithTag(EXPLORE_TAG);
		fightPowers = GameObject.FindGameObjectsWithTag(FIGHT_TAG);
	}

	protected void Update(){
		foreach (GameObject crewmember in explorePowers){
			if (Mathf.Abs(captain.eulerAngles.z - crewmember.transform.eulerAngles.z) <= powerUpAngle){
				crewmember.GetComponent<ExplorePower>().PoweredUp = true;
			} else {
				crewmember.GetComponent<ExplorePower>().PoweredUp = false;
			}
		}

		foreach (GameObject crewmember in fightPowers){
			if (Mathf.Abs(captain.eulerAngles.z - crewmember.transform.eulerAngles.z) <= powerUpAngle){
				crewmember.GetComponent<FightPower>().PoweredUp = true;
			} else {
				crewmember.GetComponent<FightPower>().PoweredUp = false;
			}
		}
	}
}
