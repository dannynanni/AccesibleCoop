using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class VectrosityTestScript2 : MonoBehaviour
{

    public float lineWidth;

    private VectorLine graphic;

    public Vector3 pointA;
    public Vector3 pointB;

    public List<Vector3> startEnd;

    public delegate void drawStraight();
    public drawStraight drawStraightDel;

    // Use this for initialization
    void Start()
    {
        //graphic = new VectorLine("Squiggle", new List<Vector3>(segmentNum), null, lineWidth, LineType.Discrete, Joins.Weld);
        //graphic.drawTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        pointA = new Vector3(-10, 0, 0);
        pointB = new Vector3(10, 0, 0);

        startEnd.Clear();
        startEnd.Add(pointA);
        startEnd.Add(pointB);

        graphic = new VectorLine("Line", startEnd, lineWidth, LineType.Continuous, Joins.Weld);
        graphic.Draw();
    }
}
