using UnityEngine;
using System.Collections;

public class AirPlaneMovingBehaviour : MonoBehaviour
{
	instantiateAirplanesHighCondition instantiator;
	float timetoreachgoal;
	Rigidbody2D rbPlane;
	// Use this for initialization
	void Start ()
	{
		rbPlane = GetComponent<Rigidbody2D> ();
		instantiator = GameObject.Find("Level").GetComponent<instantiateAirplanesHighCondition>();
		SelectorBehaviourScript selector;
		selector = GameObject.Find("airplane_container").GetComponent<SelectorBehaviourScript>();

	}
	
	// Update is called once per frame
	void Update ()
	{

		/*	timetoreachgoal  = 500.0f;
		    float lengthway = (instantiator.currentposition - instantiator.targetposition).magnitude;
			float speed = lengthway/timetoreachgoal;
			Debug.Log(speed);
			
			Vector3 moving = (instantiator.currentposition - instantiator.targetposition)* Time.deltaTime *speed;
			Debug.Log(moving);
			moving.z = 0;
			transform.position += moving ;*/

	}
}

