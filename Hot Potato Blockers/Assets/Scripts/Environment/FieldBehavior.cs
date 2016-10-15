using UnityEngine;
using System.Collections;

public class FieldBehavior : MonoBehaviour {

	public float speed = 0.1f;

	public void ScrollField(){
		Vector3 temp = new Vector3(0.0f, speed, 0.0f);

		transform.position -= temp;
	}
}
