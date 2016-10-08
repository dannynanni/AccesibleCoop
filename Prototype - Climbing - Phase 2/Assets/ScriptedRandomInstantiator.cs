using UnityEngine;
using System.Collections;

public class ScriptedRandomInstantiator : MonoBehaviour {

    public GameObject spawnedObject;

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float tMin;
    public float tMax;

    private float timer;
    private float tHold;

    void Start ()
    {
        tHold = Random.Range(tMin, tMax);
    }
    void Update () {
        timer += Time.deltaTime;
        if (timer >= tHold)
        {
            Instantiate(spawnedObject, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0), Quaternion.identity, transform);
            timer = 0;
        }
    }
}
