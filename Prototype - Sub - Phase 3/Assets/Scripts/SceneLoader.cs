using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	void Awake(){
		SceneManager.LoadScene("Test Level 2", LoadSceneMode.Additive);
        SceneManager.LoadScene("Kraken", LoadSceneMode.Additive);

    }
}
