using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KrakenMovementParameters : MonoBehaviour {

    public float moveSpeedInitial;

    public float time1;
    public float moveSpeed1;

    public float time2;
    public float moveSpeed2;

    public float time3;
    public float moveSpeed3;

    public float time4;
    public float moveSpeed4;

    public float time5;
    public float moveSpeed5;

    private float timer;
    private bool[] tripped = new bool[5];

    void Start ()
    {
        SetNewSpeed(moveSpeedInitial);
    }

    void Update () {

        timer += Time.deltaTime;

        if (timer > time1 && !tripped[0])
        {
            tripped[0] = true;
            SetNewSpeed(moveSpeed1);
        }

        if (timer > time2 && !tripped[1])
        {
            tripped[1] = true;
            SetNewSpeed(moveSpeed2);
        }

        if (timer > time3 && !tripped[2])
        {
            tripped[2] = true;
            SetNewSpeed(moveSpeed3);
        }

        if (timer > time4 && !tripped[3])
        {
            tripped[3] = true;
            SetNewSpeed(moveSpeed4);
        }

        if (timer > time5 && !tripped[4])
        {
            tripped[4] = true;
            SetNewSpeed(moveSpeed5);
        }
    }

    void SetNewSpeed(float newSpeed)
    {
        GetComponent<KrakenHeadScript>().newSpeed(newSpeed);
    }
}
