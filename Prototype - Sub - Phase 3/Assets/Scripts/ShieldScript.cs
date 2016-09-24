using UnityEngine;
using System.Collections;

public class ShieldScript : FightPower {

	public float sizeChangeRate = 0.1f;
	Vector3 deployed = new Vector3(1.7f, 1.7f, 1.4f);
	Vector3 retracted = new Vector3(0.0f, 0.0f, 0.0f);

	/// <summary>
	/// Make the shield bigger (to a maximum of "deployed") when the shield is active. Make it smaller
	/// (to a minimum of "retracted") when inactive.
	/// </summary>
	void Update(){
		if (Active){
			Vector3 newScale = Vector3.Lerp(transform.localScale, deployed, sizeChangeRate);
			transform.localScale = newScale;
		} else {
			Vector3 newScale = Vector3.Lerp(transform.localScale, retracted, sizeChangeRate);
			transform.localScale = newScale;
		}
	}
}
