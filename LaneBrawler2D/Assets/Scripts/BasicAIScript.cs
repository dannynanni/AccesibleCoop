using UnityEngine;
using System.Collections;

public class BasicAIScript : MonoBehaviour {

    public Rigidbody2D myRB;
    public float speed;

    private int movementLeftRight = 1;
    private float timer;
    public float switchDirTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        timer += Time.deltaTime;
        if (timer >= switchDirTime)
        {
            timer -= switchDirTime;
            movementLeftRight *= -1;
        }

        myRB.AddForce(transform.right * movementLeftRight * speed, ForceMode2D.Force);
        transform.localScale = new Vector3(-movementLeftRight, 1, 1);

    }
}
