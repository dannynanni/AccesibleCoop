/*
 * 
 * This is the top-level class for all player abilities.
 * In the new prototype, all player abilities involve a resource. This script provides a generic resource that
 * each ability can use as fuel, ammo, or whatever is thematically appropriate.
 * 
 * All player abilities inherit from this class, so that they have the variables for a resource.
 * Some player abilities add extra variables for things like using more of the resource in certain cases;
 * check the individual scripts for that.
 * 
 */

namespace PlayerAbility //this organizes player abilities under the Scripts button in the Unity Inspector
{
	using UnityEngine;
	using System.Collections;

	public class ResourceUse : MonoBehaviour {

		public float resourceMax = 100.0f; //the maximum amount of the resource you can have
		public float normalResourceUse = 50.0f; //how much of the resource is used to shoot, move, etc.
		private float currentResource = 100.0f; //how much of the resource you have right now
		public float CurrentResource{
			get { return currentResource; }
			set{
				currentResource = value;
				if (currentResource > resourceMax){ //you can never have more of the resource than the maximum
					currentResource = resourceMax;
				} else if (currentResource < 0.0f){ //the resource can never be a negative number
					currentResource = 0.0f;
				}
			}
		}
	}
}
