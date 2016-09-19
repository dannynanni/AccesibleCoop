/*
 * 
 * Use this script to launch (and automatically retract) the grabbing claw.
 * 
 * Call Launch() to send out the claw.
 * 
 */

using UnityEngine;
using System.Collections;

public class BasicClaw : MonoBehaviour {

	protected Transform parent;
	protected Rigidbody rb;

	public Vector3 extendedPoint = new Vector3(0.0f, 0.0f, 0.0f); //how far from the submarine the claw should go
	public float deployTime = 1.0f; //how long it takes the claw to reach maximum extension
	protected float deployTimer = 0.0f;
	public AnimationCurve deployCurve; //animation curves allow for more realistic lerping; must set in the inspector
	public float retractTime = 3.0f; //how long it takes the claw to retract
	protected float retractTimer = 0.0f;
	public AnimationCurve retractCurve;

	protected bool deploying = false;
	protected bool retracting = false;

	protected void Start(){
		parent = transform.parent;
		rb = GetComponent<Rigidbody>();
	}

	//call this to begin
	public void Launch(){
		//this if statement prevents the claw from launching before it has fully retracted
		if (Vector3.Distance(transform.localPosition, parent.localPosition) <= Mathf.Epsilon)
		{
			deploying = true;
			retracting = false;

			//reset timers for proper lerping
			deployTimer = 0.0f;
			retractTimer = 0.0f;
		}
	}
		
	protected Vector3 Deploy(){
		deployTimer += Time.deltaTime;

		Vector3 pos = Vector3.Lerp(parent.localPosition,
								   extendedPoint,
								   deployCurve.Evaluate(deployTimer/deployTime));

		return pos;
	}

	protected Vector3 Retract(){
		retractTimer += Time.deltaTime;

		Vector3 pos = Vector3.Lerp(extendedPoint,
								   parent.localPosition,
								   retractCurve.Evaluate(retractTimer/retractTime));

		return pos;
	}

	void Update(){
		if (deploying){
			transform.localPosition = Deploy();
			if (Vector3.Distance(transform.localPosition, extendedPoint) <= Mathf.Epsilon){
				deploying = false;
				retracting = true;
			}
		} else if (retracting){
			transform.localPosition = Retract();
			if (Vector3.Distance(transform.localPosition, parent.localPosition) <= Mathf.Epsilon){
				retracting = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space)){
			Launch();
		}
	}
}
