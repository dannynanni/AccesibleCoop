using UnityEngine;
using System.Collections;

public class FieldBehavior : MonoBehaviour {

	public float speed = 0.1f;

	protected void Update(){
		ScrollField();
	}

	public void ScrollField(){
		Vector3 temp = new Vector3(speed, 0.0f, 0.0f);

		transform.position -= temp;
	}
}
