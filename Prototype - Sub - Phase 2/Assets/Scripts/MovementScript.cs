﻿using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

    public delegate void movementMode();
    public movementMode runningMode;

    private bool FightMode = true;
    public bool FIGHTMODE
    {
        get
        {
            return FightMode;
        }
        set
        {
            if (value != FightMode)
            {
                if (value)
                {
                    FightMode = value;
                    runningMode = fightMove;
                    destination = transform.position;
                }
                else
                {
                    FightMode = value;
                    runningMode = exploreMove;
                }
            }
        }
    }

    public GameObject StaticModel;

    public Transform Pilot;

    public ParticleSystem TurnCav;

    private float thrust;
    public float thrustMin;
    public float thrustMax;
    private float thrustFactor;
    private float thrustLerp;
    private Vector3 thrustVector;

    public float buoyancyEffectMin;
    public float buoyancyEffectMax;
    private float buoyancyEffectLerp;
    private Vector3 buoyancyEffectVector;

    private float raw;
    private float corrected;
    private float ctrlSurfAng;
    public float ctrlSurfMax;
    private float ctrlSurfLerp;
    private Vector3 ctrlSurfVector;

    private Vector3 destination;

    public PropRotationScript PropScript;

    private int leftFlip;
    private bool left;
    public bool LEFT
    {
        get
        {
            return left;
        }
        set
        {
            if (value != left)
            {
                TurnCav.Emit(50);
                left = value;
                if (value)
                {
                    leftFlip = -1;
                    StaticModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    StaticModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    leftFlip = 1;
                }
            }
        }
    }

    void Awake()
    {
        FIGHTMODE = false;
    }
    
    void Start ()
    {
        Button(false);
        LEFT = false;
        leftFlip = 1;
    }

    // Update is called once per frame
    void FixedUpdate () {
        runningMode();       
    }

    // ------------------------------

    private void fightMove()
    {
        raw = Pilot.localRotation.eulerAngles.z;
        if (raw >= 180)
        {
            corrected = raw - 360;
        }
        else
        {
            corrected = raw;
        }

        GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, destination, .0425f));
        GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    private void exploreMove()
    {
        // thrustLerp is the smoothed factor applied to the thrustVector
        thrustLerp = Mathf.Lerp(thrustLerp, thrust, .05f);
        thrustVector = Vector3.right * thrustLerp * leftFlip;
        PropScript.rate = thrustLerp / 50;

        // buoyanceEffectLerp is the thrust dependent smoothed factor applied to the upward force applied from slowing down
        buoyancyEffectLerp = Mathf.Lerp(buoyancyEffectMin, buoyancyEffectMax, (1 - (thrustLerp * thrustFactor)));
        buoyancyEffectVector = Vector3.up * buoyancyEffectLerp;

        // ctrlSurfLerp is the velocity dependent smoothed factor applied to the upward/downward force applied by the planes
        raw = Pilot.localRotation.eulerAngles.z;
        if (raw >= 180)
        {
            corrected = raw - 360;
        }
        else
        {
            corrected = raw;
        }
        ctrlSurfAng = Mathf.Clamp(corrected, -90, 90) / 3;
        ctrlSurfLerp = Mathf.Lerp(ctrlSurfLerp, ctrlSurfMax * ctrlSurfAng, transform.root.GetComponent<Rigidbody>().velocity.magnitude);
        ctrlSurfVector = Vector3.up * ctrlSurfLerp;

        // all forces are summed and applied to rigidBody
        GetComponent<Rigidbody>().AddForce(thrustVector + buoyancyEffectVector + ctrlSurfVector, ForceMode.Force);

        // raw is used to flip the ship;
        if (raw >= 90 && raw <= 270)
        {
            LEFT = true;
        }
        else
        {
            LEFT = false;
        }
    }

    // ------------------------------

    public void Button (bool pressed)
    {
        if (FIGHTMODE)
        {
            if (pressed)
            {
                destination = transform.position + new Vector3(Mathf.Cos(corrected * Mathf.Deg2Rad), Mathf.Sin(corrected * Mathf.Deg2Rad), 0) * 20;
            }
        }
        else
        {
            thrustFactor = 1 / thrustMax;
            if (pressed)
            {
                thrust = thrustMax;
            }
            else
            {
                thrust = thrustMin;
            }
        }

    }
}
