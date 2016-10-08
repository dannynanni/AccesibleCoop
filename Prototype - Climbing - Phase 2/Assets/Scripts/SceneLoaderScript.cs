using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour {

    public string myScene1;
    public string myScene2;
 
    private void Awake ()
    {
        if (myScene1.Substring(0).Length > 0)
            SceneManager.LoadScene(myScene1, LoadSceneMode.Additive);

//        if (myScene2.Substring(0).Length > 0)
//            SceneManager.LoadScene(myScene2, LoadSceneMode.Additive);
    }
}
