using UnityEngine;
using System.Collections;

public class ControlIO : MonoBehaviour {

    public void LS (float leftRight, float upDown)
    {
        Debug.Log(gameObject.name + " is pressing the stick to " + Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg + " degrees");
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(upDown, leftRight) * Mathf.Rad2Deg);
    }

    public void AButton (bool pressed )
    {
        if (pressed)
        {
            Debug.Log(gameObject.name + " pressed A");
        }
        else if (!pressed)
        {
            Debug.Log(gameObject.name + " released A");
        }

    }
}
