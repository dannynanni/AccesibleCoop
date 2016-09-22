using UnityEngine;
using System.Collections;

public class BasicBulletBehaviorScript : MonoBehaviour {

	/// <summary>
	/// What happens if this bullet hits something?
	/// </summary>
	/// <param name="other">The collider of the thing the bullet hit.</param>
	void OnTriggerEnter(Collider other){
		if (other.tag.Contains("enemy")){
			other.gameObject.GetComponent<BasicEnemyBehavior>().GetHit();
		}
	}
}
