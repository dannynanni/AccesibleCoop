﻿namespace PlayerAbility
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class BasicLightWeapon : ResourceUse {

		private bool active = false;
		public bool Active{
			get { return active; }
			set{
				active = value;
			}
		}

		public float normalDamage = 1.0f;	//the weapon's damage when it has power
		public float lowPowerDamage = 0.1f;	//the weapon's damage when it's out of power
		private float damage = 0.0f;		//the current damage inflicted; set based on circumstances
		public float Damage{
			get { return Damage; }
			set{ Damage = value; }
		}
			
		private Image energyGauge;



		private void Start(){
			energyGauge = GameObject.Find("Energy gauge").GetComponent<Image>();
		}

		/// <summary>
		/// When the button is pressed, use up energy and reflect that on the screen.
		/// This is not how damage is done! Enemies check whether the weapon is Active, and if so ask for its
		/// Damage. No damage is actually done by this script.
		/// </summary>
		private void Update(){
			if (Active){
				CurrentResource -= normalResourceUse;
				energyGauge.fillAmount = CurrentResource/resourceMax;
			}
		}

		public void Button (bool pressed)
		{
			//shoot if the button is pressed
			//if there's enough energy, shoot normally; otherwise, shoot at much-reduced power
			if (pressed){
				if (CurrentResource >= normalResourceUse){
					Damage = normalDamage;
				} else {
					Damage = lowPowerDamage;
				}
				Active = true;
			} else {
				Active = false;
			}
		}
	}
}
