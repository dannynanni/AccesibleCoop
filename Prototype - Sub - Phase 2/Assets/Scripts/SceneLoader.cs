using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	void Awake(){
		SceneManager.LoadScene("Test Level 1", LoadSceneMode.Additive);
	}
}
