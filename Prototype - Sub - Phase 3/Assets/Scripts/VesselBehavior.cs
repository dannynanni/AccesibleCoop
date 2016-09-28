using UnityEngine;
using System.Collections;

public class VesselBehavior : MonoBehaviour {

	public float totalBlinkDuration = 2.0f;
	public float totalBlinkTimer = 0.0f;
	public float individualBlinkDuration = 0.5f;
	private float individualBlinkTimer = 0.0f;

	private GameObject shipModel;
	private PlayerAbility.MovementScript movementScript;

	private bool mobilityState = true;
	private bool visibilityState = true;
	private bool gotHit = false;
	public bool GotHit{
		get { return gotHit; }
		set {
			gotHit = value;

			if (GotHit){
				visibilityState = false;
			} else {
				visibilityState = true;
			}
		}
	}

	public ControlIO[] controlIOs;

	private const string PLAYER_NAME = "Players";
	private const string SHIP_MODEL_NAME = "ShipModelStatic";

	private void Start(){
		shipModel = GameObject.Find(SHIP_MODEL_NAME);
		movementScript = GameObject.Find(PLAYER_NAME).GetComponent<PlayerAbility.MovementScript>();
	}

	private void Update(){
		if (GotHit){
			HitFeedback();
		}
			
		movementScript.Active = mobilityState;
		shipModel.SetActive(visibilityState);
	}

	private void HitFeedback(){
		mobilityState = MobilityDetermination();
		Blink();
	}

	private void Blink(){
		totalBlinkTimer += Time.deltaTime;

		if (totalBlinkTimer >= totalBlinkDuration){
			GotHit = false;
			mobilityState = true;
			visibilityState = true;
			totalBlinkTimer = 0.0f;
			individualBlinkTimer = 0.0f;
		} else {
			mobilityState = false;
			individualBlinkTimer += Time.deltaTime;
			if (individualBlinkTimer >= individualBlinkDuration){
				visibilityState = !visibilityState;
				individualBlinkTimer = 0.0f;
			}
		}
	}

	private bool MobilityDetermination(){
		if (totalBlinkTimer < totalBlinkDuration){
			return false;
		} else {
			return true;
		}
	}
}
