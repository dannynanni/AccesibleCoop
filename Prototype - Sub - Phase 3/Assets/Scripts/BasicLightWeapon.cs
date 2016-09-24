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

		public float damage = 1.0f;
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
			//only shoot if the button is pressed and there's enough energy
			if (pressed && CurrentResource >= normalResourceUse){
				Active = true;;
			} else {
				Active = false;
			}
		}
	}
}

