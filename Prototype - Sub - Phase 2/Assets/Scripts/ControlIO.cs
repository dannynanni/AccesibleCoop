using UnityEngine;
using System.Collections;

public class ControlIO : MonoBehaviour {

    public int playerNum;

    public Component p0;
    public MovementScript p1;
    public GunFireScript p2;
    public Component p4;

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
            if (playerNum == 2)
            {
                p2.PullTrigger(true);
            }
        }
        else if (!pressed)
        {
            //Debug.Log(gameObject.name + " released A");
            if (playerNum == 1)
            {
                p1.Button(false);
            }
            if (playerNum == 2)
            {
                p2.PullTrigger(false);
            }
        }


    }
}
