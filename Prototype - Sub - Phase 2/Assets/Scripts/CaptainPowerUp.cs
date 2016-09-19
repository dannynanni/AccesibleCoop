using UnityEngine;
using System.Collections;

public class CaptainPowerUp : MonoBehaviour {

	protected GameObject[] explorePowers;
	protected const string EXPLORE_TAG = "ExploreMode";
	public float powerUpAngle = 10.0f; //how close the captain has to match someone else's direction to power them up

	protected void Start(){
		explorePowers = GameObject.FindGameObjectsWithTag(EXPLORE_TAG);
	}

	protected void Update(){
		foreach (GameObject crewmember in explorePowers){
			if (Mathf.Abs(transform.eulerAngles.z - crewmember.transform.eulerAngles.z) <= powerUpAngle){
				crewmember.GetComponent<ExplorePower>().PoweredUp = true;
			} else {
				crewmember.GetComponent<ExplorePower>().PoweredUp = false;
			}
		}
	}
}
