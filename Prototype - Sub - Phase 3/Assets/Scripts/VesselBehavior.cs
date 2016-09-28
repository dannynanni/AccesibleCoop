using UnityEngine;
using System.Collections;

public class VesselBehavior : MonoBehaviour {

	public float totalEffectDuration = 2.0f; //how long the submarine is frozen and blinking
	public float totalEffectTimer = 0.0f;
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
				mobilityState = false;
				visibilityState = false;
			} else {
				mobilityState = true;
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
		totalEffectTimer += Time.deltaTime;

		if (totalEffectTimer >= totalEffectDuration){
			GotHit = false;
			totalEffectTimer = 0.0f;
			individualBlinkTimer = 0.0f;
		} else {
			individualBlinkTimer += Time.deltaTime;
			if (individualBlinkTimer >= individualBlinkDuration){
				individualBlinkTimer = 0.0f;
			}
		}
	}
}
