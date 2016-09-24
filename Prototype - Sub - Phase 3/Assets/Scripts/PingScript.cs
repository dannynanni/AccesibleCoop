using UnityEngine;
using System.Collections;

public class PingScript : MonoBehaviour {

    private float maxSize;
    private float MAXSIZE
    {
        get
        {
            return maxSize;
        }
        set
        {
            if (value != maxSize)
            {
                maxSize = value;
                myPing = Grow;
            }
        }
    }
    private float growTime;
    private float shrinkTime;
    private float timer;

    private delegate void pingBehavior();
    private pingBehavior myPing;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (myPing != null)
        {
            myPing();
        }
    }

    private void Grow()
    {
        timer += Time.deltaTime;
        transform.localScale = Vector3.one * Mathf.Lerp(5, MAXSIZE, timer / growTime);
        if (timer/growTime >= 1)
        {
            myPing = Shrink;
        }
    }
    private void Shrink()
    {
        timer -= Time.deltaTime;
        transform.localScale = Vector3.one * Mathf.Lerp(0, MAXSIZE, timer / growTime);
        if (timer / growTime <= .01)
        {
            Destroy(this.gameObject);
        }
    }

    public void PingInit (float max, float gTime, float sTime)
    {
        growTime = gTime;
        shrinkTime = sTime;
        MAXSIZE = max;
    }
}
