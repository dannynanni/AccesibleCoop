using UnityEngine;
using System.Collections;

/// <summary>
/// This class keeps track of time. It can be put on multiple objects so that they can each be paused separately.
/// Use an object's TimeKeeper.deltaTime just like you would use Time.deltaTime. set its TimeKeeper.TimeScale = 0
/// to pause it, or = 1 to unpause it.
/// </summary>
public class TimeKeeper : MonoBehaviour {

	private float time = 0.0f;
	private float deltaTime = 0.0f;
	public float DeltaTime{
		get { return deltaTime; }
		set { deltaTime = value; }
	}
	private float timescale = 1.0f;
	public float Timescale{
		get { return timescale; }
		set{
			if (value < 0.0f){
				timescale = 0.0f;
			} else if (value > 1.0f){
				time = 1.0f;
			} else {
				timescale = value;
			}
		}
	}
	private float lastTimestamp = 0.0f;



	private void Start(){
		lastTimestamp = Time.realtimeSinceStartup;
	}

	private void Update (){
		float realDelta = Time.realtimeSinceStartup - lastTimestamp;
		lastTimestamp = Time.realtimeSinceStartup;
		time += realDelta * Timescale;
		DeltaTime = realDelta * Timescale;
	}
}