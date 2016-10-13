using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour {

    public Text tgt;

    public float timerInSeconds;

    private float timer;

    public int minutes;
    public int seconds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        timer = timerInSeconds - Time.realtimeSinceStartup;
        minutes = (int)timer / 60;
        seconds = (int)timer % 60;

        if (seconds >= 10)
        {
            tgt.text = minutes + ":" + seconds;
        }
        else
        {
            tgt.text = minutes + ":0" + seconds;
        }

        if (timer <= 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
