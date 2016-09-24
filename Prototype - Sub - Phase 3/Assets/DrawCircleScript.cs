using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class DrawCircleScript : MonoBehaviour {

    public int segmentNumber;

    public float lineWidth;
    public float radius;

    private VectorLine[] graphic = new VectorLine[3] ;
    private GameObject gORef;

    private bool shrink;

    public float maxRadius = 500;

	// Use this for initialization
	void Start () {
        for (int i = 0; i <=2; i++)
        {
            graphic[i] = new VectorLine("Bob", new List<Vector3>(segmentNumber), lineWidth);
            graphic[i].drawTransform = transform;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        graphic[0].MakeCircle(Vector3.zero, radius + 10);
        graphic[1].MakeCircle(Vector3.zero, radius + 5);
        graphic[2].MakeCircle(Vector3.zero, radius);
        for (int i = 0; i <= 2; i++)
        {
            graphic[i].Draw();
        }


        if (radius >= maxRadius)
        {
            shrink = true;
        }

        if (!shrink)
        {
            radius *= 1.05f;
        }
        else
        {
            radius -= 2;
        }

        if (radius <= 0)
        {
            NegatePing();
        }

    }

    public void NegatePing()
    {
        Debug.Log("Time to die");
        for (int i = 0; i <= 2; i++)
        {
            VectorLine.Destroy(ref graphic[i]);
        }
        Destroy(this.gameObject);
    }
}
