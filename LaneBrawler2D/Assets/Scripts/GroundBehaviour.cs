using UnityEngine;
using System.Collections;

public class GroundBehaviour : MonoBehaviour {

    public bool isMoving;

    private bool notSame;
    private bool NOTSAME
    {
        get
        {
            return notSame;
        }
        set
        {
            if (value != notSame)
            {
                if (value)
                {
                    startTimer = true;
                    timer = 0;
                }
                else
                {
                    startTimer = false;
                }
                notSame = value;
            }
        }
    }
    private bool startTimer;
    private float timer;

    Vector3 originalPosition;
    public float moveDist;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (transform.position != originalPosition)
            NOTSAME = true;

        if (startTimer)
            timer += Time.deltaTime;

        if (Vector3.Distance(transform.position, originalPosition) > 3)
            transform.position = originalPosition + Vector3.up * 3;

        if (timer >= 3)
        {
            transform.position = transform.position + Vector3.Normalize(originalPosition - transform.position) * moveDist;
            if (Vector3.Distance(transform.position, originalPosition) < moveDist * 2)
            {
                transform.position = originalPosition;
                NOTSAME = false;
                isMoving = false;
            }
        }

    }
}
