using UnityEngine;
using System.Collections;

public class MatchingIdols : BasicIdol {

	protected MatchingIdols otherIdol;
	protected GameObject closePlayer;
	protected bool destroyable = false;
	public bool Destroyable{
		get { return destroyable; }
		set { destroyable = value; }
	}
	protected Collider2D myCollider;

	protected void Start(){
		otherIdol = FindOtherIdol();
		myCollider = GetComponent<Collider2D>();
	}

	protected MatchingIdols FindOtherIdol(){
		GameObject[] idols = GameObject.FindGameObjectsWithTag(gameObject.tag);
		MatchingIdols matchingIdol = gameObject.GetComponent<MatchingIdols>(); //default initialization for error-checking

		foreach (GameObject idol in idols){
			if (idol != gameObject){
				matchingIdol = idol.GetComponent<MatchingIdols>();
			}
		}

		if (matchingIdol.gameObject == gameObject){
			Debug.Log("Couldn't find matching idol for " + gameObject.name);
		}

		return matchingIdol;
	}

	public override void CheckForDestruction(GameObject player){
		closePlayer = player;
		Destroyable = true;
	}

	protected void Update(){
		if (closePlayer != null){
			if (!closePlayer.GetComponent<Collider2D>().bounds.Intersects(myCollider.bounds)){
				Destroyable = false;
			}
		}

		if (Destroyable && otherIdol.Destroyable){
			Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f)));
			Destroy(gameObject);
		}
	}
}
