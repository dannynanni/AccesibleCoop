using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TomSceneSetup : MonoBehaviour {

	public float jumpForce = 1000.0f;
	public float platformScale = 0.25f;
	public float playerScale = 0.5f;
	public float playerSpeed = 3.0f;
	public float newTimeScale = 0.3f;
	protected const float USE_PREFAB_VALUE = 10000000.0f;

	protected void Start(){
		
		List<GameObject> players = FindObjects("player");
		Shrink(players, new Vector3(playerScale, playerScale, 1.0f));
		Slow(players, playerSpeed);
		ChangeJump(players);

		List<GameObject> blocks = FindObjects("block");
		Shrink(blocks, new Vector3(USE_PREFAB_VALUE, platformScale, 1.0f));
		Time.timeScale = newTimeScale;
	}

	protected List<GameObject> FindObjects(string target){
		List<GameObject> temp = new List<GameObject>();

		GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();

		foreach (GameObject obj in objs){
			if (obj.name.Contains(target)){
				temp.Add(obj);
			}
		}

		return temp;
	}

	protected void Shrink(List<GameObject> objs, Vector3 scale){
		foreach (GameObject obj in objs){
			Vector3 newScale = scale;
			if (newScale.x == USE_PREFAB_VALUE) { newScale.x = obj.transform.localScale.x; }

			obj.transform.localScale = newScale;
		}
	}

	protected void Slow(List<GameObject> objs, float speed){
		foreach (GameObject obj in objs){
			obj.GetComponent<SimplePlatformController>().maxSpeed = playerSpeed;
		}
	}

	protected void ChangeJump(List<GameObject> objs){
		foreach (GameObject obj in objs){
			obj.GetComponent<SimplePlatformController>().jumpForce = jumpForce;
		}
	}
}
