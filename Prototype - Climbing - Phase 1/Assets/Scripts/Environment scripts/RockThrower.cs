namespace TurnTakers
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class RockThrower : ThingsThatTakeTurns {

		public float turnDuration = 5.0f; //how long the environment's turn lasts
		public float rocksPerTurn = 2; //# of rocks to throw each turn
		private float turnTimer = 0.0f;
		private float throwTimer = 0.0f;
		private GameObject rock;
		public float maxForce = 1.0f; //how hard rocks are thrown in the x and z directions
		private List<Rigidbody> players = new List<Rigidbody>();
		private float ropeDistance = 0.0f;


		protected override void Awake(){
			timeKeeper = GetComponent<TimeKeeper>();
			turnOrderManager = transform.root.Find("TurnOrderManager").GetComponent<TurnOrderManager>();
			playerNum = int.Parse(gameObject.name[6].ToString()); //name must start with "Player#" with no space
			rock = Resources.Load("Rock") as GameObject;
			players = FindPlayers();
			ropeDistance = transform.root.Find("RopeManager").GetComponent<RopeLinkingScript>().linkDistMax;
		}

		private List<Rigidbody> FindPlayers(){
			List<Rigidbody> temp = new List<Rigidbody>();
			GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");

			foreach (GameObject obj in playerObjs){
				if (!obj.name.Contains("-")){
					temp.Add(obj.GetComponent<Rigidbody>());
				}
			}

			return temp;
		}

		private void Update(){
			if (active){
				PrepareToThrowRock();
				MeasureTurnDuration();
			}
		}

		private void MeasureTurnDuration(){
			turnTimer += timeKeeper.DeltaTime;

			if (turnTimer >= turnDuration){
				turnOrderManager.NewActivePlayer(playerNum);
				active = false;
				timeKeeper.Timescale = 0.0f;
				StopHazardsInPlace();
				UnlinkPlayers();
			}
		}

		private void PrepareToThrowRock(){
			throwTimer += Time.deltaTime;
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
			active = true;
			turnTimer = 0.0f;
			throwTimer = 0.0f;
			timeKeeper.Timescale = 1.0f;
			StartHazardsMoving();
			LinkPlayers();
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


		/// <summary>
		/// Which players are linked together? Freeze their positions so that they're immune to the rocks
		/// </summary>
		private void LinkPlayers(){
			for (int i = 0; i < players.Count; i++){
				for (int j = 0; j < players.Count; j++){
					//in order to be linked, players must (1) be different, (2) be close, and (3) be on the same
					//mountain face
					if (players[i].gameObject.name != players[j].gameObject.name && 
						Vector3.Distance(players[i].transform.position, players[j].transform.position) <= ropeDistance &&
						players[i].transform.eulerAngles.y == players[j].transform.eulerAngles.y){
						players[i].constraints = RigidbodyConstraints.FreezeAll;
						players[j].constraints = RigidbodyConstraints.FreezeAll;
					}
				}
			}
		}

		private void UnlinkPlayers(){
			for (int i = 0; i < players.Count; i++){
				players[i].constraints = RigidbodyConstraints.None;
			}
		}
	}
}
