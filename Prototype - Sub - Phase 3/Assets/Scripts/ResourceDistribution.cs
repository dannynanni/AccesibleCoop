namespace PlayerAbility
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class ResourceDistribution : ResourceUse {

		public PlayerAbility.ResourceUse[] playerAbilities;
		private Image refillGauge;
		private RectTransform selectionMarker;
		public float selectDelay = 0.1f; //the selection marker has to wait this long to move
		private float selectTimer = 0.0f;
		private bool waitForNextSelect = false;

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
		public float toppedOff = Mathf.Epsilon; //the amount, out of 100, at which the refill stops


		private void Start(){
			refillGauge = GameObject.Find("Refill gauge").GetComponent<Image>();
			selectionMarker = GameObject.Find("Selection marker").GetComponent<RectTransform>();
		}

		private void Update(){
			CurrentResource += refillRate;
			refillGauge.fillAmount = CurrentResource/resourceMax;

			if (dispensing) { DistributeResource(); }

			//prevents the selection marker from moving every frame, so that it is controllable
			if (waitForNextSelect){
				waitForNextSelect = DelaySelectionInput();
			}
		}

		/// <summary>
		/// Move between the three ability resource gauges, to decide who to refill.
		/// </summary>
		/// <param name="leftRight">The horizontal axis value from the left thumbstick.</param>
		public void ChangeSelectedAbility(float leftRight){
			if (!waitForNextSelect){
				if (leftRight < 0.0f){
					SelectedAbility--;
				} else if (leftRight > 0.0f){
					SelectedAbility++;
				}

				selectionMarker.anchoredPosition = MoveSelectionMarker();
				waitForNextSelect = true;
			}
		}

		/// <summary>
		/// Provides the locations for the selection marker as it moves. This location
		/// is based on the anchored position and assumes the anchor is at the lower-left corner.
		/// </summary>
		/// <returns>The (x, y) coordinates the selection marker should go to.</returns>
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

			return temp; //this should never be (0.0f, 0.0f) at this point; if it is, there was a problem
		}

		/// <summary>
		/// Determines whether enough time has passed to allow the selection marker to move again.
		/// </summary>
		/// <returns><c>true</c>, if enough time has passed, <c>false</c> otherwise.</returns>
		private bool DelaySelectionInput(){
			selectTimer += Time.deltaTime;

			if (selectTimer >= selectDelay){
				selectTimer = 0.0f;
				return false;
			}

			return true;
		}

		public void Button(bool pressed){
			if (pressed){
				dispensing = true;
			} else {
				dispensing = false;
			}
		}
			
		/// <summary>
		/// Transfers resources from the captain's gauge to the chosen gauge.
		/// </summary>
		private void DistributeResource(){
			if (CurrentResource >= dispenseRate){ //make sure the captain has enough to distribute . . .
				//. . . and don't distribute to someone who's already at full
				if (100.0f - playerAbilities[SelectedAbility].CurrentResource > toppedOff){
					playerAbilities[SelectedAbility].CurrentResource += dispenseRate;
					CurrentResource -= dispenseRate;
				}
			}
		}
	}
}
