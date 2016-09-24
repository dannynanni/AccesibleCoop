/*
 * 
 * Use this script to launch (and automatically retract) the grabbing claw.
 * 
 * Call Launch() to send out the claw.
 * 
 */

using UnityEngine;
using System.Collections;

public class BasicClaw : ExplorePower {

	protected Transform parent;

	public float range = 30.0f; //how far from the submarine the claw should go
	protected Vector3 extendedPoint = new Vector3(0.0f, 0.0f, 0.0f); //localPosition the claw is aiming toward
	protected Vector3 retractPoint = new Vector3(0.0f, 0.0f, 0.0f); //localPosition claw actually reaches
	public float deployTime = 1.0f; //how long it takes the claw to reach maximum extension
	protected float deployTimer = 0.0f;
	public AnimationCurve deployCurve; //animation curves allow for more realistic lerping; must set in the inspector
	public float retractTime = 3.0f; //how long it takes the claw to retract
	protected float retractTimer = 0.0f;
	public AnimationCurve retractCurve;
	protected Collider[] collectibles;
	public float grabDist = 1.0f; //how close the claw has to get to a collectible to pick it up

	protected bool deploying = false;
	protected bool retracting = false;

	protected const string COLLECTIBLE_TAG = "Collectible";
    protected const string GRABBED_COLLECTIBLE_NAME = "Acquired";

    public float powerUpRangeBonus = 20.0f;

    private GameObject openClaw;
    private GameObject closedClaw;

    void Awake ()
    {
        openClaw = GameObject.Find("clawOpen");
        closedClaw = GameObject.Find("clawClosed");
        openClaw.SetActive(false);
        closedClaw.SetActive(false);
    }

	protected void Start(){
		parent = transform.parent;
		extendedPoint.x = range;
	}

	//call this to begin
	public void Launch(){
			deploying = true;
			retracting = false;

			//reset timers for proper lerping
			deployTimer = 0.0f;
			retractTimer = 0.0f;

			//find items potentially close enough to retrieve
			collectibles = Physics.OverlapSphere(transform.position, range);

        openClaw.SetActive(true);
        closedClaw.SetActive(false);
	}

	/// <summary>
	/// Move the claw out toward its furthest extent
	/// </summary>
	protected Vector3 Deploy(){
		deployTimer += Time.deltaTime;

		Vector3 pos = Vector3.Lerp(parent.localPosition,
								   extendedPoint,
								   deployCurve.Evaluate(deployTimer/deployTime));

		return pos;
	}

    /// <summary>
    /// See if the claw can grab something. If close enough to the collectible, it grabs the collectible,
    /// starts retracting back toward the submarine, and the score increases by one.
    /// </summary>
    protected void TryToPickUp()
    {
        foreach (Collider item in collectibles)
        {
            if (item.tag.Contains(COLLECTIBLE_TAG))
            {
                if (Vector3.Distance(transform.position, item.transform.position) <= grabDist)
                {
                    item.transform.parent.parent = transform;
                    item.transform.parent.name = GRABBED_COLLECTIBLE_NAME;
                    deploying = false;
                    retracting = true;
                    retractPoint = transform.localPosition;

                    openClaw.SetActive(false);
                    closedClaw.SetActive(true);

                    break;
                }
            }
        }
    }


    /// <summary>
    /// Move the claw back toward the submarine.
    /// </summary>
    protected Vector3 Retract(){
		retractTimer += Time.deltaTime;

		Vector3 pos = Vector3.Lerp(retractPoint,
								   parent.localPosition,
								   retractCurve.Evaluate(retractTimer/retractTime));

		return pos;
	}

    protected void Update()
    {
        if (deploying)
        {
            transform.localPosition = Deploy();
            TryToPickUp();
            if (Vector3.Distance(transform.localPosition, extendedPoint) <= Mathf.Epsilon)
            {
                deploying = false;
                retracting = true;
                retractPoint = transform.localPosition;

                openClaw.SetActive(false);
                closedClaw.SetActive(true);
            }
        }
        else if (retracting)
        {
            transform.localPosition = Retract();
            if (Vector3.Distance(transform.localPosition, parent.localPosition) <= Mathf.Epsilon)
            {
                retracting = false;
                if (transform.childCount > 4)
                {
                    Destroy(transform.Find(GRABBED_COLLECTIBLE_NAME).gameObject);
                }
            }
        }
    }

    //provides the powerup unique to the claw
    protected override bool PowerUpCheck (bool potentialState){
		if (base.poweredUp != potentialState){
			if (potentialState){
				range += powerUpRangeBonus;
				extendedPoint.x = range;
			} else {
				range -= powerUpRangeBonus;
				extendedPoint.x = range;
			}
			return potentialState;
		} else {
			return base.poweredUp;
		}
	}

    /// <summary>
    /// What happens when the weaponeer presses their button?
    /// </summary>
    /// <param name="pressed">If set to <c>true</c>, the button is pressed; this is <c>false</c> otherwise.</param>

    public void Button(bool pressed)
    {
        if (pressed)
        {
            //this if statement prevents the claw from launching before it has fully retracted
            if (!deploying && !retracting)
            {
                Launch();
            }
        }
    }
}
