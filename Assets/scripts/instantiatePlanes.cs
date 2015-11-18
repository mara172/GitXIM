using UnityEngine;
using System.Collections;

public class instantiatePlanes : MonoBehaviour {

	//public int[,] limits = new int[,] { {-25,0}, {0,10}, {25,0}, {0,-10} };

	// Change this for tutorial
	//public int[,] limits = new int[,] { {-25,0}, {25,0} };

	int[] myNumbers = new int[] {0,1,2,3,4,5,7,8,9};
	public int[,] limits = new int[,] { {-40,0}, {40,0} };

	public GameObject plane;

	//private float timeBetweenPlanes = 4f;  // 0.2 = 5 shots per second
	// times for difficult task
	private float timeBetweenPlanesMin = 2f;
	private float timeBetweenPlanesMax = 6f;
	// times for easy task
//	private float timeBetweenPlanesMin = 4f;  
//	private float timeBetweenPlanesMax = 8f;
	private int secondsBeforeFirstPlaneAppears = 0; // Wait X seconds before the first enemy appears after the game starts
	
	private float timeLastPlane;

	public float velocityModule = 1.2f;

	public int lastAction = 0;
	public int lastActionDetails = 0;

	public Vector2 lastCrash = new Vector2();
	public string lastCrashAirplane="";

	public int nPlanes = 0;

	// Use this for initialization
	void Start () {
	}

	void makePlane(int side) {
		string planeName;
		GameObject planeInstance;
		do {
			//planeName = Random.Range(1,1000).ToString();
			planeName = myNumbers[Random.Range(0,8)].ToString() + myNumbers[Random.Range(0,8)].ToString() ;
		} while (GameObject.Find(planeName));
		int layer, angle, velocityAngle;
		layer = Random.Range(9,11);
		angle = Random.Range(-20,20);
		//Change this for tutorial
		//angle = 0;
		planeInstance = GameObject.Instantiate(plane);
		planeInstance.transform.position = new Vector2(limits[side,0],limits[side,1]);
//		if (limits[side,1] == 0) {
			int posVariation = Random.Range(-limits[1,1]/3,limits[1,1]/3);
			posVariation += (int)planeInstance.transform.position.y;
			planeInstance.transform.position = new Vector2(planeInstance.transform.position.x,posVariation);
			int sign = limits[side,0]/Mathf.Abs(limits[side,0]);
			velocityAngle = 0;
			planeInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(-sign*velocityModule,0);
			if (sign > 0) {
				velocityAngle = 180;
				planeInstance.transform.Rotate(0,0,180);
			}
//		}
//		else {
//			int posVariation = Random.Range(-limits[2,0]/3,limits[2,0]/3);
//			posVariation += (int)planeInstance.transform.position.x;
//			planeInstance.transform.position = new Vector2(posVariation,planeInstance.transform.position.y);
//			int sign = limits[side,1]/Mathf.Abs(limits[side,1]);
//			velocityAngle = -sign*90;
//			planeInstance.transform.Rotate(0,0,-sign*90);
//		}
		planeInstance.transform.Rotate(0,0,angle);
		velocityAngle += angle;
		planeInstance.GetComponent<Rigidbody2D>().velocity = new Vector2 (velocityModule*Mathf.Cos (velocityAngle*Mathf.Deg2Rad),velocityModule*Mathf.Sin (velocityAngle*Mathf.Deg2Rad));
		planeInstance.layer = layer;
		planeInstance.GetComponent<SpriteRenderer>().color = new Color(0.8f,0.8f,0.0f);
		Debug.Log("new plane: " + planeName);
		planeInstance.name = planeName;
		//Assign label to each plane
		int layerHuman = ((layer == 9)? 2000: 3000);
		//planeInstance.GetComponentInChildren<TextMesh>().text = "P "+ planeName + " ("+layerHuman.ToString()+")";
	}

	// Update is called once per frame
	void Update () {
	
		if (Time.realtimeSinceStartup > secondsBeforeFirstPlaneAppears && Time.time >= timeLastPlane) {
			int side, nside;
			side = Random.Range(0,2);
			makePlane(side);
			do {
				nside = Random.Range(0,2);
			} while (nside == side);
			makePlane(nside);
			float timeBetweenPlanes = Random.Range(timeBetweenPlanesMin,timeBetweenPlanesMax);
			timeLastPlane = Time.time + timeBetweenPlanes;

		}

		nPlanes = GameObject.FindGameObjectsWithTag ("Plane").Length;


	}
}
