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

		public float normalDamage = 1.0f;
		public float lowPowerDamage = 0.1f;
		private float damage = 0.0f;
		public float Damage{
			get { return Damage; }
			set{ Damage = value; }
		}
			
		private Image energyGauge;



		private void Start(){
			energyGauge = GameObject.Find("Energy gauge").GetComponent<Image>();
		}

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

