using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class StraightLineScript : MonoBehaviour {

    public float lineWidth;

    private VectorLine graphic;

    public Transform ship;

    public Vector3 pointA;
    public Vector3 pointB;

    public List<Vector3> startEnd;

    public delegate void drawStraight();
    public drawStraight drawStraightDel;

    public ParticleSystem LineSnap;

    // Use this for initialization
    void Start()
    {
        //graphic = new VectorLine("Squiggle", new List<Vector3>(segmentNum), null, lineWidth, LineType.Discrete, Joins.Weld);
        //graphic.drawTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawStraightDel != null)
        {
            drawStraightDel();
        }
    }

    public void init()
    {
        graphic = new VectorLine("Line", startEnd, lineWidth, LineType.Continuous, Joins.Weld);
        drawStraightDel = drawingFunc;

        LineSnap.Emit(200);
        Debug.Log("Emit 200");
    }

    public void shutDown()
    {
        VectorLine.Destroy(ref graphic);
        drawStraightDel = null;
        //Debug.Log("Destroy graphic");
    }

    public void drawingFunc()
    {
        if (ship == null)
        {
            ship = GameObject.Find("Players").transform;
        }


        pointA = ship.position;
        pointB = transform.position;

        startEnd.Clear();
        startEnd.Add(pointA);
        startEnd.Add(pointB);

        VectorLine.Destroy(ref graphic);
        graphic = new VectorLine ("Line", startEnd, lineWidth, LineType.Continuous, Joins.Weld);
        graphic.Draw();
    }
}
