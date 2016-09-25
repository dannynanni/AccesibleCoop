namespace PlayerAbility
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
			get { return damage; }
			set{ damage = value; }
		}
		private float debugDamage = 1000000.0f; //a rediculous amount; if this ever happens, something is wrong!
			
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
				Damage = SetDamage();
				CurrentResource -= normalResourceUse;
				energyGauge.fillAmount = CurrentResource/resourceMax;
			}
		}

		private float SetDamage(){
			if (CurrentResource >= normalResourceUse){
				return normalDamage;
			} else {
				return lowPowerDamage;
			}

			return debugDamage; //if the weapon is insta-destroying everything, something is wrong 
		}

		public void Button (bool pressed)
		{
			//shoot if the button is pressed
			//if there's enough energy, shoot normally; otherwise, shoot at much-reduced power
			if (pressed){
				Active = true;
			} else {
				Active = false;
			}
		}
	}
}

