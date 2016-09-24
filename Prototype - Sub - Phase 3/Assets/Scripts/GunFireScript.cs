using UnityEngine;
using System.Collections;
public class GunFireScript : FightPower
{
    public GameObject bullet;
    private bool triggerPulled;
    public float reloadTime;
    public float bulletForce;
    private float timer;
    public float powerUpGrowth = 2.0f;
    protected float currentScale = 1.0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (Active)
        {
            if (timer >= reloadTime && triggerPulled)
            {
                Fire();
                timer = 0;
            }
        }
    }
    void Fire()
    {
        GameObject myBullet = (GameObject)Instantiate(bullet, transform.position + transform.right * 3, transform.rotation, GameObject.Find("Container").transform);
        myBullet.transform.localScale *= currentScale;
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
    protected override bool PowerUpCheck(bool potentialState)
    {
        if (base.poweredUp != potentialState)
        {
            if (potentialState)
            {
                currentScale *= powerUpGrowth;
                reloadTime /= powerUpGrowth;
            }
            else
            {
                currentScale /= powerUpGrowth;
                reloadTime *= powerUpGrowth;
            }
            return potentialState;
        }
        else
        {
            return base.poweredUp;
        }
    }
}
