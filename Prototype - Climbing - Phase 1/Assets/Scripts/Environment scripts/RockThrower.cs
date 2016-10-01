namespace TurnTakers
{
	using UnityEngine;
	using System.Collections;

	public class RockThrower : ThingsThatTakeTurns {

		public float turnDuration = 5.0f; //how long the environment's turn lasts
		public float rocksPerTurn = 2; //# of rocks to throw each turn
		private float turnTimer = 0.0f;
		private float throwTimer = 0.0f;
		private GameObject rock;
		public float maxForce = 1.0f; //how hard rocks are thrown in the x and z directions



		protected override void Awake(){
			timeKeeper = GetComponent<TimeKeeper>();
			turnOrderManager = transform.root.Find("TurnOrderManager").GetComponent<TurnOrderManager>();
			playerNum = int.Parse(gameObject.name[6].ToString()); //name must start with "Player#" with no space
			rock = Resources.Load("Rock") as GameObject;
		}

		private void Update(){
			if (active){
				PrepareToThrowRock();
				MeasureTurnDuration();
			}
		}

		private void MeasureTurnDuration(){
			turnTimer += timeKeeper.DeltaTime;

			//Debug.Log(turnTimer);
			if (turnTimer >= turnDuration){
				turnOrderManager.NewActivePlayer(playerNum);
				active = false;
				timeKeeper.Timescale = 0.0f;
				StopHazardsInPlace();
			}
		}

		private void PrepareToThrowRock(){
			throwTimer += Time.deltaTime;
			Debug.Log(throwTimer);
			if (throwTimer >= turnDuration/rocksPerTurn){
				Debug.Log("throwing");
				throwTimer = 0.0f;
				GameObject newRock = Instantiate(rock, transform.position, Quaternion.identity) as GameObject;
				newRock.transform.parent = transform;
				newRock.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-maxForce, maxForce),
														   0.0f,
														   Random.Range(-maxForce, maxForce)),
														   ForceMode.Impulse);
			}
		}

		public override void Reset(){
			Debug.Log("starting turn");
			active = true;
			turnTimer = 0.0f;
			throwTimer = 0.0f;
			timeKeeper.Timescale = 1.0f;
			StartHazardsMoving();
		}

		private void StopHazardsInPlace(){
			GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");

			foreach (GameObject hazard in hazards){
				hazard.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			}
		}

		private void StartHazardsMoving(){
			GameObject[] hazards = GameObject.FindGameObjectsWithTag("Hazard");

			foreach (GameObject hazard in hazards){
				hazard.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			}
		}
	}
}
