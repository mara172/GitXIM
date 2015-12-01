using UnityEngine;
using System.Collections;

public class SelectorBehaviourScriptTraining : MonoBehaviour {
	
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
		if(selectedPlaneNameBuffer.Length == 2){
			Debug.Log("Got Airplane");
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
	}
	
	void OnGUI(){
		
		if (Event.current.type == EventType.KeyDown) {
			KeyPressedEventHandler ();
		}
		
	}
	private void KeyPressedEventHandler() {
		Debug.Log ("I am here");
		// do whatever you want to do when a key was pressed ;-)
		Debug.Log ("I am in selected Airplane");
		if(Input.GetKeyDown("q")){
			selectedPlaneNameBuffer = "";
			selectPlaneMode = false;
			pMode.SetActive (false);
		}
		if (Input.GetKeyDown("p")) {
			selectedPlaneNameBuffer = "";
			pMode.SetActive (true);
			Debug.Log ("entering select plane mode");
			selectPlaneMode = true;
		}
		if (selectPlaneMode) {
			if ((Input.GetKeyDown ("1") || Input.GetKeyDown ("2") || Input.GetKeyDown ("3") || Input.GetKeyDown ("4") || Input.GetKeyDown ("5") || Input.GetKeyDown ("6") || Input.GetKeyDown ("7") || Input.GetKeyDown ("8") || Input.GetKeyDown ("9") || Input.GetKeyDown ("0"))){
				selectedPlaneNameBuffer += Input.inputString[0];
				Input.ResetInputAxes();
				Debug.Log (selectedPlaneNameBuffer);
				Debug.Log (selectedPlaneNameBuffer.Length);
			}
			
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
