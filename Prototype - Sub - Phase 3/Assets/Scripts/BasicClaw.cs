/*
 * 
 * Use this script to launch (and automatically retract) the grabbing claw.
 * 
 * Call Launch() to send out the claw.
 * 
 */
namespace PlayerAbility
{
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;

	public class BasicClaw : ResourceUse {

		Transform clawTarget;

		public float range = 30.0f; //how far from the submarine the claw should go
		protected Vector3 startPoint = new Vector3(0.0f, 0.0f, 0.0f);
		protected Vector3 extendedPoint = new Vector3(0.0f, 0.0f, 0.0f); //position the claw is aiming toward
		protected Vector3 retractPoint = new Vector3(0.0f, 0.0f, 0.0f); //position claw actually reaches

		//off-screen location where the claws stay when they're not in use
		protected Vector3 openClawWaitPoint = new Vector3(-100.0f, -100.0f, -1000.0f);
		protected Vector3 closedClawWaitPoint = new Vector3(-100.0f, -100.0f, -1000.0f);

		public float deployTime = 1.0f; //how long it takes the claw to reach maximum extension
		protected float deployTimer = 0.0f;
		public AnimationCurve deployCurve; //animation curves allow for more realistic lerping; set in the inspector
		public float retractTime = 3.0f; //how long it takes the claw to retract
		protected float retractTimer = 0.0f;
		public AnimationCurve retractCurve;
		protected Collider[] collectibles;
		public float grabDist = 1.0f; //how close the claw has to get to a collectible to pick it up

        public float pauseTime = 0.5f; //how long the claw pauses after grabbing before being retracted

		protected bool deploying = false;
		protected bool retracting = false;

		protected const string COLLECTIBLE_TAG = "Collectible";
	    protected const string GRABBED_COLLECTIBLE_NAME = "Acquired";

	    public float powerUpRangeBonus = 20.0f;

	    private GameObject openClaw;
	    private GameObject closedClaw;
        private GameObject closedClawPart;

		private Image ammoGauge;

		private PlayerAbility.ResourceDistribution captainResourceDistro;



	    void Awake ()
	    {
			clawTarget = transform.parent.Find("Claw target");
	        openClaw = GameObject.Find("clawOpen");
	        closedClaw = GameObject.Find("clawClosed");

            closedClawPart = GameObject.Find("LineSnapParticles");

			openClaw.transform.position = openClawWaitPoint;
			closedClaw.transform.position = closedClawWaitPoint;
	        openClaw.SetActive(false);
	        closedClaw.SetActive(false);
			ammoGauge = GameObject.Find("Claw ammo gauge").GetComponent<Image>();
			captainResourceDistro = GameObject.Find("ResourceDistribution")
				.GetComponent<PlayerAbility.ResourceDistribution>();
	    }

		//put the claw target at the appropriate range
		protected void Start(){
			Vector3 temp = clawTarget.position;
			temp.x = range;
			clawTarget.position = temp;
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
            openClaw.GetComponent<WavyLineScript>().init();
			openClaw.transform.position = transform.position;
			startPoint = transform.position;
			extendedPoint = clawTarget.position;
            closedClaw.GetComponent<StraightLineScript>().shutDown();
	        closedClaw.SetActive(false);

			//expend ammo
			CurrentResource -= normalResourceUse;
			ammoGauge.fillAmount = CurrentResource/resourceMax;
		}

		/// <summary>
		/// Move the claw out toward its furthest extent
		/// </summary>
		protected Vector3 Deploy(){
			deployTimer += Time.deltaTime;

			Vector3 pos = Vector3.Lerp(startPoint,
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
	                if (Vector3.Distance(openClaw.transform.position, item.transform.position) <= grabDist)
	                {
	                    deploying = false;
	                    retracting = true;
	                    retractPoint = transform.position;
						    
	                    closedClaw.SetActive(true);
                        closedClaw.transform.position = openClaw.transform.position;
                        closedClawPart.transform.position = openClaw.transform.position;

                        closedClawPart.transform.position = Vector3.Lerp(clawTarget.position, transform.position, .5f);
                        closedClawPart.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(clawTarget.position.y - transform.position.y, clawTarget.position.x - transform.position.x));
                        float length = Vector3.Distance(clawTarget.position, transform.position);
                        GetComponent<ParticleSystem>().shape.box.Equals(new Vector3(length * 2, 0, 0));
                        closedClaw.GetComponent<StraightLineScript>().init();

                        item.transform.parent.parent = closedClaw.transform;
						item.transform.parent.name = GRABBED_COLLECTIBLE_NAME;
						openClaw.transform.position = openClawWaitPoint;
                        openClaw.GetComponent<WavyLineScript>().shutDown();
						openClaw.SetActive(false);
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

			Vector3 pos = Vector3.Lerp(closedClaw.transform.position,
									   transform.position,
									   retractCurve.Evaluate((retractTimer - pauseTime)/retractTime));

			return pos;
		}

	    protected void Update()
	    {
			//if the claw has been launched, send it further out, try to pick things up, and then
			//start retracting if it's reached its full range without grabbing anything
	        if (deploying)
	        {
	            openClaw.transform.position = Deploy();
	            TryToPickUp();
				if (Vector3.Distance(openClaw.transform.position, extendedPoint) <= Mathf.Epsilon)
	            {
	                deploying = false;
	                retracting = true;
	                retractPoint = transform.position;

	                closedClaw.SetActive(true);
                    closedClaw.transform.position = openClaw.transform.position;

                    closedClawPart.transform.position = Vector3.Lerp(clawTarget.position, transform.position, .5f);
                    closedClawPart.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(clawTarget.position.y - transform.position.y, clawTarget.position.x - transform.position.x));
                    float length = Vector3.Distance(clawTarget.position, transform.position);
                    GetComponent<ParticleSystem>().shape.box.Equals(new Vector3(length * 2, 0, 0));
                    closedClaw.GetComponent<StraightLineScript>().init();

                    openClaw.transform.position = openClawWaitPoint;
                    openClaw.GetComponent<WavyLineScript>().shutDown();
                    openClaw.SetActive(false);
	            }
	        }

			//if the claw is retracting, pull it back, and check to see if it's reached the sub
			//if so, get it off the screen and consume any collectible it grabbed
	        else if (retracting)
	        {
	            closedClaw.transform.position = Retract();
				if (Vector3.Distance(closedClaw.transform.position, transform.position) <= Mathf.Epsilon)
	            {
                    Debug.Log("Shutting down straight line, because close.");
                    closedClaw.GetComponent<StraightLineScript>().shutDown();
					closedClaw.transform.position = closedClawWaitPoint;
	                retracting = false;
	                if (closedClaw.transform.childCount > 2) //if >0, it has picked up a collectible
	                {
						GameObject collectible = GameObject.Find(GRABBED_COLLECTIBLE_NAME).gameObject;
						captainResourceDistro.CurrentResource +=
							collectible.GetComponent<ResourceCollectibleScript>().ResourceContained;
						Destroy(collectible);
	                }
	            }
	        }

			ammoGauge.fillAmount = CurrentResource/resourceMax; //keep the ammo gauge up-to-date
	    }

	    /// <summary>
	    /// What happens when the player presses their button?
	    /// </summary>
	    /// <param name="pressed">If set to <c>true</c>, the button is pressed; this is <c>false</c> otherwise.</param>
	    public void Button(bool pressed)
	    {
	        if (pressed)
	        {
	            //this if statement prevents the claw from launching before it has fully retracted, and if there's
				//insufficient ammo
				if (!deploying && !retracting && (CurrentResource >= normalResourceUse))
	            {
	                Launch();
	            }
	        }
	    }
	}
}
