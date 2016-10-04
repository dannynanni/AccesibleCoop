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

    private float cameraAng;
    private float targetAng;
    private float distToPlayer;
    private float camDist;

	// Use this for initialization
	void Start () {
        // Add points from bottom to top
        spinePoints.Add(Vector3.zero);
        spinePoints.Add(new Vector3(0, 17, 0));

	}
	
	// Update is called once per frame
	void Update () {

        indexNum = findFirstPointAbove();
        Debug.Log(indexNum);
        lerpFloat = interpolationFloat(indexNum);
        Vector3 evenPos = Vector3.Lerp(transform.position, Vector3.Lerp(spinePoints[indexNum], spinePoints[indexNum + 1], lerpFloat), .05f);
        transform.position = evenPos - Vector3.up * .15f;

        targetAng = calcTargetAng();
        cameraAng = Mathf.Lerp(cameraAng, targetAng, .05f);
        transform.rotation = Quaternion.Euler(0, cameraAng, 0);

        distToPlayer = distToPlayerCalc(transform.position, targetPlayer.transform.position);
        camDist = distToPlayer + 10;
        Camera.localPosition = new Vector3 (0, 0, camDist);

    }

    private int findFirstPointAbove ()
    {
        for (int i = spinePoints.Count - 1; i >= 0; i--)
        {
            Debug.Log(i);
            if (targetPlayer.transform.position.y >= spinePoints[i].y)
            {
                return i;
            }
        }
        return 0;
    }

    private float interpolationFloat (int index)
    {
       return ((targetPlayer.transform.position.y - spinePoints[index].y) / (spinePoints[index + 1].y - spinePoints[index].y));
    }

    private float calcTargetAng ()
    {
        return (Mathf.Rad2Deg * Mathf.Atan2(targetPlayer.transform.position.x - transform.position.x, targetPlayer.transform.position.z - transform.position.z));
    }

    private float distToPlayerCalc(Vector3 centerPos, Vector3 playerPos)
    {
        return Vector3.Distance(centerPos, playerPos); 
    }

    public void SetActivePlayer (GameObject activePlayer)
    {
        targetPlayer = activePlayer;
    }
}
