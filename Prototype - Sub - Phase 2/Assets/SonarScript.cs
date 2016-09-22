using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SonarScript : MonoBehaviour {

    public GameObject pingPrefab;
    private GameObject activePing;

    private List<Collider> pingHit = new List<Collider>();

    public void MakeAPing(bool onDown)
    {
        if (onDown)
        {
            GameObject myPing = (GameObject)Instantiate(pingPrefab, transform.position, Quaternion.identity);
            //myPing.GetComponent<DrawCircleScript>().PingInit(500, 3, 5);
            activePing = myPing;
        }
        else
        {
            Debug.Log("Let it go");
            if (Vector3.Distance(activePing.transform.position, transform.position) < activePing.GetComponent<DrawCircleScript>().radius + 10f
                && Vector3.Distance(activePing.transform.position, transform.position) > activePing.GetComponent<DrawCircleScript>().radius - 2f)
            {
                Debug.Log("Let go on time");
                foreach (Collider myCol in Physics.OverlapSphere(activePing.transform.position, activePing.GetComponent<DrawCircleScript>().maxRadius))
                {
                    pingHit.Add(myCol);
                    if (myCol.gameObject.name == "Basic enemy(Clone)")
                    {
                        myCol.GetComponent<BasicEnemyBehavior>().ResetVisibility();
                    }
                }
            }
            activePing.GetComponent<DrawCircleScript>().NegatePing();
            Debug.Log("Destroy ping");
        }
    }
}
