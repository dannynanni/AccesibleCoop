using UnityEngine;
using System.Collections;

public class BasicLightFadeInScript : MonoBehaviour {

    private SpriteRenderer SR;
    private float SRAlphaBase;

    private float timer;

	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
        SRAlphaBase = SR.color.a;
        SR.color = new Color (255, 255, 255, 0);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        timer += Time.deltaTime;
        if (timer > 2)
        {
            SR.color = (Color32.Lerp(SR.color, new Color(255, 255, 255, SRAlphaBase), .05f));
        }

    }
}
