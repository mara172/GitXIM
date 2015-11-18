using UnityEngine;
using System.Collections;

public class AirplaneBehaviourScript : MonoBehaviour {
	
	Rigidbody2D rbPlane;
	SelectorBehaviourScript selector;
	instantiateAirplanesHighCondition instantiator;
	LineRenderer lineRenderer;
	public AudioClip audioRoger;
	public AudioClip audioCollision;
	private AudioSource audioSource;
	private GUIStyle guiStyle = new GUIStyle(); //create a new variable
	GameObject scorekeeper; 
	int test ;


	//private int currentScore;
	
	void Start () {
		rbPlane = GetComponent<Rigidbody2D> ();
		selector = GameObject.Find("airplane_container").GetComponent<SelectorBehaviourScript>();
		Debug.Log (selector);
		instantiator = GameObject.Find("Level").GetComponent<instantiateAirplanesHighCondition>();
		lineRenderer = GetComponent<LineRenderer>();
		//lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
		lineRenderer.SetColors (Color.black, new Color(0.1f,0.2f,0.1f));
		lineRenderer.SetWidth(0.1f,0.1f);
		scorekeeper = GameObject.Find("Main Camera");

		//Audio
		audioSource = gameObject.AddComponent<AudioSource>();

	
	}
	
	
	void RotateAndChangeVelocity(int angle) {
		float currentAngle = transform.rotation.eulerAngles.z + angle;
		transform.rotation=Quaternion.Euler(0, 0,  currentAngle );
		rbPlane.velocity = new Vector2 (instantiator.speed*Mathf.Cos (currentAngle*Mathf.Deg2Rad),instantiator.speed*Mathf.Sin (currentAngle*Mathf.Deg2Rad));
		selector.selectedPlaneName = "";
		//change color
		GetComponent<SpriteRenderer>().color = new Color(0.8f,0.8f,0.0f);
		//GetComponent<SpriteRenderer>().color = ((this.gameObject.layer == 9) ? Color.blue: Color.black);
		//play audio Roger
		audioSource.clip = audioRoger;
		audioSource.Play();
	}
	
	void SafeDestroy(int type) {

		if (selector.selectedPlaneName == this.name) {
			selector.selectedPlaneName = "";
		}
		//instantiator.nPlanes -= 1;
		Debug.Log (instantiator.nPlanes);
		this.gameObject.GetComponent<Renderer>().enabled = false;
		if (type == 1) {
			Destroy (this.gameObject, audioCollision.length); //waits till audio is finished playing before destroying.
		}
		else {
			Destroy(this.gameObject);
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {

		audioSource.clip = audioCollision;
		audioSource.Play();

		if((instantiator.possiblePositions[0].x < this.gameObject.transform.position.x)&&(this.gameObject.transform.position.x < instantiator.possiblePositions[1].x)
		   &&(instantiator.possiblePositions[0].y <this.gameObject.transform.position.y ) &&(this.gameObject.transform.position.y < instantiator.possiblePositions[1].y)){
			scorekeeper.GetComponent<scoreScript>().currentScore -= 1.0f;
			scorekeeper.GetComponent<scoreScript> ().currentScoreNegative += 1.0f;
 			instantiator.lastCrashAirplane.Add(instantiator.targetposition.ToString());
			Debug.Log("Crashed Airplanes "+instantiator.targetposition.ToString());
			instantiator.lastCrash.x = this.gameObject.transform.position.x;
			instantiator.lastCrash.y = this.gameObject.transform.position.y;
			//Debug.Log(scorekeeper.GetComponent<scoreScript> ().currentScoreNegative);
		}
		else{
			scorekeeper.GetComponent<scoreScript>().currentScore += 1.0f;
			scorekeeper.GetComponent<scoreScript> ().currentScorePositive += 1;
			//Debug.Log("Not counting crash");
			//Debug.Log(this.gameObject.transform.position);
		}
		SafeDestroy (1);
	}
	
	void FixedUpdate () {
		Vector2 screenPos = Camera.main.WorldToScreenPoint (transform.position);
		if (screenPos.x < 0 || screenPos.x > Camera.main.pixelWidth || screenPos.y < 0 || screenPos.y > Camera.main.pixelHeight) {
			//Debug.Log("I am safe");
			//Debug.Log("score now: " + scorekeeper.GetComponent<scoreScript>().currentScore.ToString());
			SafeDestroy(0);
			scorekeeper.GetComponent<scoreScript>().currentScore++;
			scorekeeper.GetComponent<scoreScript>().currentScorePositive++;
			//Debug.Log("new score: " + scorekeeper.GetComponent<scoreScript>().currentScore.ToString());
		} 
		else {
			lineRenderer.SetPosition(0,transform.position);
			int currentAngle = (int) transform.rotation.eulerAngles.z;
			float addx = 20*Mathf.Cos(currentAngle*Mathf.Deg2Rad);
			float addy = 20*Mathf.Sin(currentAngle*Mathf.Deg2Rad);
			lineRenderer.SetPosition(1,new Vector2(transform.position.x+addx,transform.position.y+addy));
			if (selector.selectedPlaneName == this.name) {
				//				if (Input.GetKeyDown ("a")) {
				//					Debug.Log ("Rotate 90!");
				//					RotateAndChangeVelocity (-90);
				//					instantiator.lastAction = 1;
				//					instantiator.lastActionDetails = 1;
				//				}
				if (Input.GetKeyDown ("down")) {
					Debug.Log ("Rotate 45!");
					instantiator.lastAction = 1;
					instantiator.lastActionDetails = 2;
					RotateAndChangeVelocity (-45);

				}
				if (Input.GetKeyDown ("up")) {
					Debug.Log ("Rotate -45!");
					instantiator.lastAction = 1;
					instantiator.lastActionDetails = 3;
					RotateAndChangeVelocity (45);

				}
				//				if (Input.GetKeyDown ("f")) {
				//					Debug.Log ("Rotate -90!");
				//					RotateAndChangeVelocity (90);
				//					instantiator.lastAction = 1;
				//					instantiator.lastActionDetails = 4;
				//				}
				if (Input.GetKeyDown("l")) {
					this.gameObject.layer = ((this.gameObject.layer == 9) ? 10: 9);
					instantiator.lastAction = 2;
					instantiator.lastActionDetails = 1;
					//GetComponent<SpriteRenderer>().color = ((this.gameObject.layer == 9) ? Color.blue: Color.black);
					GetComponent<SpriteRenderer>().color = new Color(0.8f,0.8f,0.0f);
					//update plane height (layer) on text label
					int layerHuman = ((this.gameObject.layer == 9)? 2000: 3000);
					//GetComponentInChildren<TextMesh>().text = "P "+ this.name + " ("+layerHuman.ToString()+")";
					selector.selectedPlaneName = "";
					audioSource.clip = audioRoger;
					audioSource.Play();

				}
				if (Input.GetKeyDown("m")) {
					//GetComponent<SpriteRenderer>().color = ((this.gameObject.layer == 9) ? Color.blue: Color.black);
					GetComponent<SpriteRenderer>().color = new Color(0.8f,0.8f,0.0f);
					selector.selectedPlaneName = "";
				}
				if (Input.GetKeyDown("o")) {
					rbPlane.velocity = new Vector2 (rbPlane.velocity.x*9,rbPlane.velocity.y*9);
					selector.selectedPlaneName = "";
				}
			}
		}

	}
	void OnGUI(){
		
		Vector3 getPixelPos = Camera.main.WorldToScreenPoint( rbPlane.position);
		getPixelPos.y = Screen.height - getPixelPos.y;
		int layerHuman = ((this.gameObject.layer == 9)? 2000: 3000);
		string nm = "P " + this.name + " (" + layerHuman.ToString () + ")";
		// Fontsize computer
		//guiStyle.fontSize = 12; //change the font size
		// Fontsize XIM
		guiStyle.fontSize = 46; //change the font size
		guiStyle.normal.textColor = Color.white;

		if((transform.rotation.eulerAngles.z < 90) || (270 < transform.rotation.eulerAngles.z)&& ( transform.rotation.eulerAngles.z < 360)){
			//Namespace XIM
			GUI.Label (new Rect (getPixelPos.x-280, getPixelPos.y, 200f, 100f), nm, guiStyle);
			//Namespace Computer
			//GUI.Label (new Rect (getPixelPos.x-80, getPixelPos.y, 200f, 100f), nm, guiStyle);
		}
		else{
			//Namespace XIM
			GUI.Label (new Rect (getPixelPos.x+40, getPixelPos.y, 200f, 100f), nm, guiStyle);
			//Namespace Computer
			//GUI.Label (new Rect (getPixelPos.x+10, getPixelPos.y, 200f, 100f), nm, guiStyle);
			
		}
	}
}
