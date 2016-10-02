using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManagerScript : MonoBehaviour {

    // Allows for a spine to be inserted mathematically that represents the "center" of the mountain
    // Automatically sets the CameraManager to a position along the spine at the height of the targetPlayer 

    public List<Vector3> spinePoints = new List<Vector3>();
    public Transform Camera;

    public GameObject targetPlayer;

    private int indexNum;
    private float lerpFloat;

	// Use this for initialization
	void Start () {
        // Add points from bottom to top
        spinePoints.Add(Vector3.zero);
        spinePoints.Add(new Vector3(0, 15, 0));

	}
	
	// Update is called once per frame
	void Update () {

        indexNum = findFirstPointAbove();
        lerpFloat = interpolationFloat(indexNum);
        transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(spinePoints[indexNum], spinePoints[indexNum + 1], lerpFloat), .05f);

    }

    private int findFirstPointAbove ()
    {
        for (int i = spinePoints.Count - 1; i >= 0; i--)
        {
            if (targetPlayer.transform.position.y >= spinePoints[i].y)
            {
                return indexNum;
            }
            indexNum = i;
        }
        return 0;
    }

    private float interpolationFloat (int index)
    {
       return ((targetPlayer.transform.position.y - spinePoints[index].y) / (spinePoints[index + 1].y - spinePoints[index].y));
    }

    public void SetActivePlayer (GameObject activePlayer)
    {
        targetPlayer = activePlayer;
    }
}
