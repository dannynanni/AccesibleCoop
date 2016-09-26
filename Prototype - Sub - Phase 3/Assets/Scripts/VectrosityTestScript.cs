using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class VectrosityTestScript : MonoBehaviour {

    public int segmentNum;
    public float lineWidth;

    private VectorLine graphic;

    public Vector3[] values = new Vector3[10];

    public Vector3 pointA;
    public Vector3 pointB;
    public float dist;

	// Use this for initialization
	void Start () {

        pointA = new Vector3(-10, 0, 0);
        pointB = new Vector3(10, 0, 0);
        dist = Vector3.Distance(pointA, pointB);

        for (int i = 0; i < values.Length; i++)
        {
            float inc = ((i + 1) * dist / values.Length);
            Debug.Log(inc);
            values[i] = pointA + inc * Vector3.Normalize(pointB - pointA) + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        }

        graphic = new VectorLine("Squiggle", new List<Vector3>(segmentNum), null, lineWidth, LineType.Discrete, Joins.Weld);
        graphic.drawTransform = transform;

        

    }
	
	// Update is called once per frame
	void Update () {

        for (int i = 1; i < values.Length - 1; i++)
        {
            if (i == 0)
            {
                values[i] = pointA;
            }
            else if ( i == values.Length - 1)
            {
                values[i] = pointB;
            }
            else
            {
                float inc = ((i + 1) * dist / values.Length);
                Debug.Log(inc);
                values[i] = pointA + inc * Vector3.Normalize(pointB - pointA) + new Vector3(0, Random.Range(-1.1f, 1.1f), Random.Range(-1.1f, 1.1f));
            }
        }
        graphic.MakeSpline(values);
        graphic.Draw();

	}
}
