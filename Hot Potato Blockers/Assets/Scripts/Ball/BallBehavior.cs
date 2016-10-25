using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {

	public float flightTimePerUnitDistance = 0.1f;
	public AnimationCurve flightCurve;


	public IEnumerator PassBetweenPlayers(Transform start, Transform destination){
		Debug.Log("Passing between players");
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
}
