using UnityEngine;
using System.Collections;

public class VesselBehavior : MonoBehaviour {

	public float totalBlinkDuration = 2.0f;
	private float totalBlinkTimer = 0.0f;
	public float individualBlinkDuration = 0.5f;
	private float individualBlinkTimer = 0.0f;

	private bool visible = true;
	private bool gotHit = false;
	public bool GotHit{
		get { return gotHit; }
		set {
			gotHit = value;

			if (GotHit){
				visible = false;
				foreach (ControlIO controlScript in controlIOs){
					controlScript.TakingInput = false;
				}
			} else {
				visible = true;
				foreach (ControlIO controlScript in controlIOs){
					controlScript.TakingInput = true;
				}
			}
		}
	}

	public ControlIO[] controlIOs;

	private GameObject shipModel;
	private const string SHIP_MODEL_NAME = "ShipModelStatic";

	private void Start(){
		shipModel = GameObject.Find(SHIP_MODEL_NAME);
	}

	private void Update(){
		if (GotHit){
			HitFeedback();
		}

		shipModel.SetActive(visible);
	}

	private void HitFeedback(){
		Blink();
	}

	private void Blink(){
		totalBlinkTimer += Time.deltaTime;

		if (totalBlinkTimer >= totalBlinkDuration){
			GotHit = false;
			visible = true;
			totalBlinkTimer = 0.0f;
			individualBlinkTimer = 0.0f;
		} else {
			individualBlinkTimer += Time.deltaTime;
			if (individualBlinkTimer >= individualBlinkDuration){
				visible = !visible;
				individualBlinkTimer = 0.0f;
			}
		}
	}
}
