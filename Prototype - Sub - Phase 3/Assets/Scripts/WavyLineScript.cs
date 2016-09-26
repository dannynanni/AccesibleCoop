using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class WavyLineScript : MonoBehaviour {

    public int segmentNum;
    public float lineWidth;

    private VectorLine graphic;

    public Vector3[] values = new Vector3[10];

    public Transform ship;

    public Vector3 pointA;
    public Vector3 pointB;
    public float dist;

    public float squiggleMag;

    public delegate void drawSquig();
    public drawSquig drawSquigDel;

    // Use this for initialization
    void Start()
    {
        //graphic = new VectorLine("Squiggle", new List<Vector3>(segmentNum), null, lineWidth, LineType.Discrete, Joins.Weld);
        //graphic.drawTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawSquigDel != null)
        {
            drawSquigDel();
        }
    }

    public void init()
    {
        graphic = new VectorLine("Squiggle", new List<Vector3>(segmentNum), null, lineWidth, LineType.Discrete, Joins.Weld);
        drawSquigDel = drawingFunc;
        //Debug.Log("Init");
    }

    public void shutDown()
    {
        VectorLine.Destroy(ref graphic);
        drawSquigDel = null;
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

        dist = Vector3.Distance(pointA, pointB);

        for (int i = 0; i < values.Length; i++)
        {
            if (i == 0)
            {
                values[i] = pointA;
            }
            else if (i == values.Length - 1)
            {
                values[i] = pointB;
            }
            else
            {
                float inc = ((i) * dist / (values.Length));
                //Debug.Log(inc);
                values[i] = pointA + inc * Vector3.Normalize(pointB - pointA) + new Vector3(Random.Range(-.5f, .5f), Random.Range(-squiggleMag, squiggleMag), Random.Range(-squiggleMag, squiggleMag));
            }
        }
        graphic.MakeSpline(values);
        graphic.Draw();
    }
}
