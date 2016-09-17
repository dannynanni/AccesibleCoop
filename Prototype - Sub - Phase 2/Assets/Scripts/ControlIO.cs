using UnityEngine;
using System.Collections;

public class ControlIO : MonoBehaviour {

    public int playerNum;

    public MovementScript p1;

    void Awake ()
    {

    }

    public void LS (float leftRight, float upDown)
    {
        //Debug.Log(gameObject.name + " is pressing the stick to " + Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg + " degrees");
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg);
    }

    public void AButton (bool pressed )
    {
        if (pressed)
        {
            //Debug.Log(gameObject.name + " pressed A");
            if (playerNum == 1)
            {
                p1.Button(true);
            }
        }
        else if (!pressed)
        {
            //Debug.Log(gameObject.name + " released A");
            if (playerNum == 1)
            {
                p1.Button(false);
            }
        }


    }
}
