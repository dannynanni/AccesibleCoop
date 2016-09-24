namespace PlayerAbility
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class ResourceDistribution : ResourceUse {

		public PlayerAbility.ResourceUse[] playerAbilities;
		private Image refillGauge;
		private RectTransform selectionMarker;

		private int selectedAbility = 0;
		public int SelectedAbility{
			get { return selectedAbility; }
			set{
				if (value > playerAbilities.Length - 1){ //can't select an ability beyond those in the array
					selectedAbility = 0;
				} else if (value < 0){ //can't select a negative number in the array
					selectedAbility = playerAbilities.Length - 1;
				} else {
					selectedAbility = value;
				}
			}
		}

		public float refillRate = 0.01f; //how fast the submarine's power reserve naturally refills
		public float dispenseRate = 0.5f; //how fast power flows from the reserve to other player abilities
		private bool dispensing = false;

		private void Start(){
			refillGauge = GameObject.Find("Refill gauge").GetComponent<Image>();
			selectionMarker = GameObject.Find("Selection marker").GetComponent<RectTransform>();
		}

		private void Update(){
			CurrentResource += refillRate;
			refillGauge.fillAmount = CurrentResource/resourceMax;

			if (dispensing) { DistributeResource(); }
		}

		public void ChangeSelectedAbility(float leftRight){
			if (leftRight < 0.0f){
				SelectedAbility--;
			} else if (leftRight > 0.0f){
				SelectedAbility++;
			}

			selectionMarker.anchoredPosition = MoveSelectionMarker();
		}

		private Vector2 MoveSelectionMarker(){
			Vector2 leftPos = new Vector2(69.5f, 292.0f);
			Vector2 centerPos = new Vector2(138.0f, 292.0f);
			Vector2 rightPos = new Vector2(211.0f, 292.0f);
			Vector2 temp = new Vector2(0.0f, 0.0f);

			switch (SelectedAbility){
				case 0:
					temp = leftPos;
					break;
				case 1:
					temp = centerPos;
					break;
				case 2:
					temp = rightPos;
					break;
				default:
					Debug.Log("Illegal SelectedAbility: " + SelectedAbility);
					break;
			}

			return temp; //this should never be (0.0f, 0.0f) at this point; if it was, there was a problem
		}

		public void Button(bool pressed){
			if (pressed){
				dispensing = true;
			} else {
				dispensing = false;
			}
		}

		private void DistributeResource(){
			if (CurrentResource >= dispenseRate){ //make sure the captain has enough to distribute . . .
				//. . . and don't distribute to someone who's already at full
				if (100.0f - playerAbilities[SelectedAbility].CurrentResource > Mathf.Epsilon){
					playerAbilities[SelectedAbility].CurrentResource += dispenseRate;
					CurrentResource -= dispenseRate;
				}
			}
		}
	}
}
