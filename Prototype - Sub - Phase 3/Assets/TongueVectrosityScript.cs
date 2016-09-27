using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class TongueVectrosityScript : MonoBehaviour {
    public int segmentNum;
    public float lineWidth;

    private VectorLine graphic;

    public Vector3[] values = new Vector3[10];
    public Vector3[] drawValues = new Vector3[10];

    public Vector3 pointA;
    public Vector3 pointB;
    public float dist;

    public float squiggleMag;

    public Transform tongueTarget;

    public int frameCounter;

    public Texture tongueMat;

    // Use this for initialization
    void Start()
    {
        tongueTarget = transform.Find("TongueTarget");

        graphic = new VectorLine("Squiggle", new List<Vector3>(30*segmentNum), tongueMat, lineWidth, LineType.Discrete, Joins.Weld);
        graphic.SetColor(new Color(255, 154, 154, 255));

        //graphic.drawTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pointA = transform.position;
        pointB = tongueTarget.transform.position;

        dist = Vector3.Distance(pointA, pointB);

        frameCounter++;
        if (frameCounter >= 10)
        {
            frameCounter = 0;
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
        }        
    }

    void Update()
    {
        for (int i = 0; i < values.Length; i++)
        {
            drawValues[i] = Vector3.Lerp(drawValues[i], values[i], .3f);
            graphic.MakeSpline(drawValues);
            graphic.Draw();
        }
    }

    public void shutDown()
    {
        VectorLine.Destroy(ref graphic);
    }
}
