using UnityEngine;
using System.Collections;

public class SeaMineScript : MonoBehaviour {

    private ParticleSystem explosion;
    private GameObject mine;
    private GameObject chain;

    public bool destroy;
    public float timer;

    void Update()
    {
        if (destroy)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 10)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Players")
        {
            Debug.Log("Explode");
            explosion = transform.Find("UnderwaterExplosion").GetComponent<ParticleSystem>();
            explosion.Emit(100);

            mine = transform.Find("Explosive").gameObject;
            mine.SetActive(false);

            chain = transform.Find("Chain").gameObject;
            chain.GetComponent<Rigidbody>().useGravity = true;

            destroy = true;
        }
    }
}
