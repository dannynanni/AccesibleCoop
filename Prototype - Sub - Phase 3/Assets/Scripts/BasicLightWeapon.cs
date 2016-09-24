using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicLightWeapon : MonoBehaviour {

	private bool active = false;
	public bool Active{
		get { return active; }
		set{
			active = value;
		}
	}

	public float damage = 1.0f;
	public float Damage{
		get { return Damage; }
		set{ Damage = value; }
	}

	//variables relating to the submarine's use of fuel to move
	public float energyStart = 100.0f;
	public float attackEnergyUse = 2.0f;
	private float currentEnergy = 100.0f;
	public float CurrentEnergy{
		get { return currentEnergy; }
		set{
			currentEnergy = value;
			if (currentEnergy > energyStart){ //the light can never have more energy than it started with
				currentEnergy = energyStart;
			} else if (currentEnergy < 0.0f){ //energy can never be a negative number
				currentEnergy = 0.0f;
			}
		}
	}
	Image energyGauge;

	void Start(){
		energyGauge = GameObject.Find("Energy gauge").GetComponent<Image>();
	}

	public void Button (bool pressed)
	{
		if (pressed){
			Active = true;;
		} else {
			Active = false;
		}
	}
}
