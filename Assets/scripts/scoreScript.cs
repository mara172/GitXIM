using UnityEngine;
using System.Collections;
using UnityEngine.UI; // needed for UI score

public class scoreScript : MonoBehaviour {

	public Text playerScore;
	public float currentScore = 0;

	public int currentScorePositive = 0;
	public float currentScoreNegative = 0;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		playerScore.text = "Score: "+currentScore+"\nScore positive: "+currentScorePositive+"\nScore negative: "+currentScoreNegative+"\n";
	//	TextMesh textObject = GameObject.Find("ScoreNum").GetComponent<TextMesh>();
		//playerScore = playerScore + 1;


	

	}
}
