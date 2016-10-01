namespace TurnTakers
{
	using UnityEngine;
	using System.Collections;

	public class ThingsThatTakeTurns : MonoBehaviour {

		protected TimeKeeper timeKeeper;
		protected TurnOrderManager turnOrderManager;
		protected bool active = false;
		protected int playerNum = 0;

		protected virtual void Awake(){
			timeKeeper = GetComponent<TimeKeeper>();
			turnOrderManager = transform.root.Find("TurnOrderManager").GetComponent<TurnOrderManager>();
			playerNum = int.Parse(gameObject.name[6].ToString()); //name must start with "Player#" with no space
		}

		public virtual void Reset(){
			/*
			 * 
			 * Each type of thing that takes a turn has its own reset function; this is just here to make it
			 * easy for the TurnOrderManager to call them.
			 * 
			 */
		}
	}
}
