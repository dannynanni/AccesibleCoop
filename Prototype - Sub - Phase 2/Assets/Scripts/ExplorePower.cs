/*
 * 
 * This class allows other classes to inherit from it so that the captain's power up can find all of them.
 * 
 * It also provides some functionality shared by all abilities: the possibility of being powered up,
 * and being turned on or off based on the submarine's mode.
 * 
 */

using UnityEngine;
using System.Collections;

public class ExplorePower : MonoBehaviour {

	protected bool active = true;
	public bool Active{
		get { return active; }
		set { active = value; }
	}

	protected bool poweredUp = false;
	public bool PoweredUp{
		get { return poweredUp; }
		set{
			poweredUp = PowerUpCheck(value);
		}
	}

	//override this to enable powerups; see BasicClaw for an example
	protected virtual bool PowerUpCheck(bool potentialState){
		if (poweredUp != potentialState){
			return potentialState;
		} else {
			return poweredUp;
		}
	}
}
