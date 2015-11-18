using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class instantiateAirplanesHighCondition : MonoBehaviour
{
	int[] myNumbers = new int[] {0,1,2,3,4,5,7,8,9};
	public int[,] limits = new int[,] { {-40,0}, {40,0} };
	
	public GameObject plane;
	
	//private float timeBetweenPlanes = 4f;  // 0.2 = 5 shots per second
	// times for difficult task
	private float timeBetweenPlanesMin = 2f;
	private float timeBetweenPlanesMax = 6f;
	private Vector2[] crashpoints = new Vector2[56];
	public Vector2 collisiongoal;
	float timetoreachgoal  = 10.0f;
	//rectangle where the airplanes could arrive (2 corners)
	public Vector2[] possiblePositions=new [] { new Vector2(-40,-25), new Vector2(40,25)};
	//freespace around possible positions
	private int freespace = 10;
	//public int[] directions = new int[] {45, 90};
	//public int[,] dependendAirplanPosition=new int[,] {{0,0}, {-40,0} };
	//public int[] dependenddirections = new int[] {180, 90};
	public Vector3 currentposition ;
	public Vector3  targetposition ;
	private string[] layernames = {"height1", "height2"};
	public float speed;

	public Vector2 lastCrash = new Vector2(-2000,-2000);
	public List<string> lastCrashAirplane=new List<string>();


	// times for easy task
	//	private float timeBetweenPlanesMin = 4f;  
	//	private float timeBetweenPlanesMax = 8f;
	private int secondsBeforeFirstPlaneAppears = 0; // Wait X seconds before the first enemy appears after the game starts
	
	private float timeLastPlane;
	
	public float velocityModule = 0.5f;
	
	public int lastAction = 0;
	public int lastActionDetails = 0;
	
	public int nPlanes = 0;

	// Use this for initialization
	void Start ()
	{
		float posx;
		float posy;
		int crashpointsindex =0;
		crashpoints.Initialize ();
		//Crashfield goes from -40 to +40 in x
		// and from 25 to -25 in y
		for (int i=0;i<9;i++){
			posx= (possiblePositions[0].x+freespace)+i*(Mathf.Abs(possiblePositions[0].x+freespace)+(Mathf.Abs(possiblePositions[1].x-freespace)))/9;
			for (int k = 0;k<6;k++){
				posy = (possiblePositions[0].y+freespace)+k*(Mathf.Abs(possiblePositions[0].y+freespace)+(Mathf.Abs(possiblePositions[1].y-freespace)))/6;
				Vector2 newvec = new Vector2(posx, posy);
				crashpoints[crashpointsindex]= newvec;
				crashpointsindex++;
					//.Add(new Vector2(posx,posy));
			}
		}
		nPlanes = 0;

	}

	void makePlane(int positionrangex, int positionrangey, float crashpositionx,float crashpositiony) {

		string planeName;
		GameObject planeInstance;
		do {
			planeName = myNumbers[Random.Range(0,8)].ToString() + myNumbers[Random.Range(0,8)].ToString() ;
		} while (GameObject.Find(planeName));
		int layer, angle;
		float velocityAngle;
		layer = Random.Range(0,2);
		//nPlanes += 1;
		planeInstance = GameObject.Instantiate(plane);
		//Debug.Log (nPlanes);
		Vector3 temp = planeInstance.transform.position; // copy to an auxiliary variable...
		velocityAngle = 0;
		currentposition = new Vector3 (positionrangex, positionrangey,planeInstance.transform.position.z);
		targetposition = new Vector3 (crashpositionx,crashpositiony ,planeInstance.transform.position.z);
		planeInstance.transform.position = currentposition;  
		//Debug.Log (targetposition);
		planeInstance.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetposition.y-currentposition.y, targetposition.x-currentposition.x) * Mathf.Rad2Deg );
		velocityAngle = Mathf.Atan2 (targetposition.y - currentposition.y, targetposition.x - currentposition.x);
		float lengthway = (currentposition - targetposition).magnitude;
		speed = lengthway/timetoreachgoal;
		planeInstance.GetComponent<Rigidbody2D>().velocity = new Vector2 (speed*Mathf.Cos (velocityAngle),speed*Mathf.Sin (velocityAngle));
		planeInstance.layer = LayerMask.NameToLayer (layernames [layer]);
		planeInstance.GetComponent<SpriteRenderer>().color = new Color(0.8f,0.8f,0.0f);
		Debug.Log("Instantited Airplanes"+targetposition.ToString());
		planeInstance.name = planeName;
		//Assign label to each plane
		int layerHuman = ((layer == 9)? 2000: 3000);
	}


	// Update is called once per frame
	void Update ()
	{

		if (Time.realtimeSinceStartup > secondsBeforeFirstPlaneAppears && Time.time >= timeLastPlane) {
			int side, nside;
			int Apositionrangex,Apositionrangey, sideA;
			int Bpositionrangex,Bpositionrangey, sideB;
			int Adirection,Bdirection;
			sideA = Random.Range(0,4);
			sideB = Random.Range(0,4);
			Bpositionrangex=0;
			Bpositionrangey=0;
			Apositionrangex=0;
			Apositionrangey=0;
			Adirection = 0;
			Bdirection = 0;
			//Airplane appears from down
			if (sideA ==0){
				Apositionrangex=Random.Range(-40,40);
				Apositionrangey=-40;
				Adirection = 90;

			}
			//Airplane appears from left
			else if (sideA ==1){
				Apositionrangex=-60;
				Apositionrangey=Random.Range(-25,25);
				Adirection = 0;
			}
			//Airplane appears from up
			else if (sideA ==2){
				Apositionrangex=Random.Range(-40,40);
				Apositionrangey=40;
				Adirection = 270;
			}
			//Airplane appears from right
			else if (sideA ==3){
				Apositionrangex=60;
				Apositionrangey=Random.Range(-25,25);
				Adirection = 180;
			}


			//Airplane appears from down
			if (sideB ==0){
				Bpositionrangex=Random.Range(-60,60);
				Bpositionrangey=-40;
				Bdirection = 90;
			}
			//Airplane appears from left
			else if (sideB ==1){
				Bpositionrangex=-60;
				Bpositionrangey=Random.Range(-25,25);
				Bdirection = 0;
			}
			//Airplane appears from up
			else if (sideB ==2){
				Bpositionrangex=Random.Range(-60,60);
				Bpositionrangey=40;
				Bdirection = 270;
			}
			//Airplane appears from right
			else if (sideB ==3){
				Bpositionrangex=60;
				Bpositionrangey=Random.Range(-25,25);
				Bdirection = 180;
			}
			int randomcrashpoint1,randomcrashpoint2;
			randomcrashpoint1 = Random.Range(0,crashpoints.Length-1);
			float crashpositionx = crashpoints[randomcrashpoint1][0];
			float crashpositiony = crashpoints[randomcrashpoint1][1];
			randomcrashpoint2 = Random.Range(0,crashpoints.Length-1);
			float crashposition2x = crashpoints[randomcrashpoint2][0];
			float crashposition2y = crashpoints[randomcrashpoint2][1];
			makePlane(Apositionrangex, Apositionrangey, crashpositionx, crashpositiony);
			makePlane(Bpositionrangex, Bpositionrangey, crashpositionx, crashpositiony);
			float timeBetweenPlanes = Random.Range(timeBetweenPlanesMin,timeBetweenPlanesMax);
			timeLastPlane = Time.time + timeBetweenPlanes;
			
		}
		
		nPlanes = GameObject.FindGameObjectsWithTag ("Plane").Length;

	}
}
