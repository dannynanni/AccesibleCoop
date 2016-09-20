﻿/*
 * This class' only purpose is to allow other classes to inherit from it so that the captain's power up can find
 * all of them.
 * 
 * This class doesn't do anything on its own! It's only a reference.
 * 
 */

using UnityEngine;
using System.Collections;

public class FightPower : MonoBehaviour {

	protected bool active = false;
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
