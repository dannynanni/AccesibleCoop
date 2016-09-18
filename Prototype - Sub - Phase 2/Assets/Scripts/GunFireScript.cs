using UnityEngine;
using System.Collections;

public class GunFireScript : MonoBehaviour {

    public GameObject bullet;
    private bool triggerPulled;

    public float reloadTime;
    public float bulletForce;

    private float timer;


    void Update ()
    {
        timer += Time.deltaTime;

        if (timer >= reloadTime && triggerPulled)
        {
            Fire();
            timer = 0;
        }
    }

    void Fire ()
    {
        GameObject myBullet = (GameObject) Instantiate(bullet, transform.position + transform.right * 3, transform.rotation, GameObject.Find("Container").transform);
        Vector3 boreLineForce = transform.right * bulletForce;
        myBullet.GetComponent<Rigidbody>().AddForce(boreLineForce, ForceMode.Impulse);
    }

    public void PullTrigger(bool pressed)
    {
        if (pressed)
        {
            triggerPulled = true;
        }
        else
        {
            triggerPulled = false;
        }
    }
}
