using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {

	public float flightTimePerUnitDistance = 0.1f;
	public AnimationCurve flightCurve;
	private const string END_ZONE_OBJ = "End zone";

	public IEnumerator PassBetweenPlayers(Transform start, Transform destination){
		float totalFlightTime = Vector3.Distance(start.position, destination.position) * flightTimePerUnitDistance;
		float timer = 0.0f;

		while (timer <= totalFlightTime){
			timer += Time.deltaTime;

			transform.position = Vector3.Lerp(start.position,
											  destination.position,
											  flightCurve.Evaluate(timer/totalFlightTime));

			yield return null;
		}

		yield break;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.transform.parent.name.Contains(END_ZONE_OBJ)){
			Debug.Log("You win!");
		}
	}
}
