using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	Text scoreText;
	const string LABEL = "Treasures: ";

	/// <summary>
	/// The score. Whenever this changes, the label on the screen changes as well.
	/// </summary>
	int score = 0;
	public int Score{
		get { return score; }
		set{
			score = value;
			scoreText.text = LABEL + score.ToString();
		}
	}

	void Start(){
		scoreText = GetComponent<Text>();
	}
}
