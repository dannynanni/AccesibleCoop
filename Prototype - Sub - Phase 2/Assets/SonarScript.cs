using UnityEngine;
using System.Collections;

public class SonarScript : MonoBehaviour {

    public GameObject pingPrefab;

    public void MakeAPing(bool onDown)
    {
        if (onDown)
        {
            GameObject myPing = (GameObject)Instantiate(pingPrefab, transform.position, Quaternion.identity);
            myPing.GetComponent<PingScript>().PingInit(500, 3, 2);
        }
    }

}
