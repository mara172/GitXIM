using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;

public class SelectorBehaviourScript : MonoBehaviour {
	
	bool selectPlaneMode = false;
	string selectedPlaneNameBuffer = "";
	public string selectedPlaneName = "";
	
	public GameObject pMode;
	
	// Use this for initialization
	void Start () {
		//GameObject.Find ("p_mode").transform.position = new Vector2 (-Screen.width / 2 + Screen.width / 10, -Screen.height / 2 + Screen.height / 10);
		//Debug.Log (Camera.main.ScreenToWorldPoint (new Vector2 (-Screen.width / 2 + Screen.width / 10, -Screen.height / 2 + Screen.height / 10)));
		Vector2 pPosition = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
		pMode = GameObject.Find ("p_mode");
		pMode.transform.position = new Vector2 (- pPosition.x * 9 / 10, -pPosition.y * 4 / 5); 
		pMode.SetActive (false);
		//Debug.Log (Screen.width.ToString() + Screen.height.ToString() );
		Debug.Log ("joyful airplane container");
	}
	
	// Update is called once per frame
	void Update () {
		if (selectPlaneMode) {
			foreach (char c in Input.inputString) {
				Debug.Log(c);
				//if (c == "\n"[0] || c == "\r"[0]) {
				if (selectedPlaneNameBuffer.Length == 1) {
					selectedPlaneNameBuffer += c;
					GameObject selectedPlane;
					selectedPlane = GameObject.Find (selectedPlaneNameBuffer);
					pMode.SetActive (false);
					if (selectedPlane) {
						selectedPlaneName = selectedPlaneNameBuffer;
						Debug.Log("selected: " + selectedPlaneName);
						selectedPlane.GetComponent<SpriteRenderer>().color = Color.red;
					}
					selectedPlaneNameBuffer = "";
					selectPlaneMode = false;
				}
				else {
					selectedPlaneNameBuffer += c;
				}
			}
		}
		if (selectedPlaneName == "" && Input.GetKeyDown("p")) {
			pMode.SetActive (true);
			Debug.Log ("entering select plane mode");
			selectPlaneMode = true;
		}
	}
}
/*

public class SelectorBehaviourScript : MonoBehaviour {

	bool selectPlaneMode = false;
	string selectedPlaneNameBuffer = "";
	public string selectedPlaneName = "";

	public GameObject pMode;

	// Use this for initialization
	void Start () {
		//GameObject.Find ("p_mode").transform.position = new Vector2 (-Screen.width / 2 + Screen.width / 10, -Screen.height / 2 + Screen.height / 10);
		//Debug.Log (Camera.main.ScreenToWorldPoint (new Vector2 (-Screen.width / 2 + Screen.width / 10, -Screen.height / 2 + Screen.height / 10)));
		Vector2 pPosition = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
		pMode = GameObject.Find ("p_mode");
		pMode.transform.position = new Vector2 (- pPosition.x * 9 / 10, -pPosition.y * 4 / 5); 
		pMode.SetActive (false);
		//Debug.Log (Screen.width.ToString() + Screen.height.ToString() );
		Debug.Log ("joyful airplane container");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (selectPlaneMode) {
			foreach (char c in Input.inputString) {
				Debug.Log(c);
				//if (c == "\n"[0] || c == "\r"[0]) {

				if (selectedPlaneNameBuffer.Length == 1) {
					selectedPlaneNameBuffer += c;
					GameObject selectedPlane;
					selectedPlane = GameObject.Find (selectedPlaneNameBuffer);
					pMode.SetActive (false);
					if (selectedPlane) {
						selectedPlaneName = selectedPlaneNameBuffer;
						Debug.Log("selected: " + selectedPlaneName);
						selectedPlane.GetComponent<SpriteRenderer>().color = Color.red;
					}
					selectedPlaneNameBuffer = "";
					selectPlaneMode = false;
				}
				else if (c == 1||c == 2||c == 3||c == 4||c == 5||c == 6||c == 7||c == 8||c == 9){
					selectedPlaneNameBuffer += c;
				}
				else{
					pMode.SetActive (false);
					selectPlaneMode = false;
				}
			}
		}
		if (selectedPlaneName == "" && Input.GetKeyDown ("p")) {
			pMode.SetActive (true);
			Debug.Log ("entering select plane mode");
			selectPlaneMode = true;
		}
		if (Input.GetKeyDown ("q")) {
			pMode.SetActive (false);
			selectPlaneMode = false;
		}
	}
}*/
