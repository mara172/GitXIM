using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Diagnostics;




//using System.IO;

public class CollectData : MonoBehaviour {
	//Pupil ET
	/// <summary>
	/// My log.
	/// </summary>
	public LogBitPupil myLog;
	/// <summary>
	/// The manager.
	/// </summary>
	public ManagerBITalino manager;
	/// <summary>
	/// The bitreader.
	/// </summary>
	public BITalinoReader bitreader;
	/// <summary>
	/// The pupreader.
	/// </summary>
	public PupilReader pupreader;
	/// <summary>
	/// The serial.
	/// </summary>
	public BITalinoSerialPort serial;
	/// <summary>
	/// The state.
	/// </summary>
	public GUIText state;
	/// <summary>
	/// The data.
	/// </summary>
	public GUIText data;
	/// <summary>
	/// The eyetracker.
	/// </summary>
	public GUIText eyetracker;
	private StreamWriter sw;
	/// <summary>
	/// The useeyetracker.
	/// </summary>
	public bool useeyetracker = false;
	/// <summary>
	/// The data file.
	/// </summary>
	public bool dataFile = false;
	private Stopwatch stopWatch;
	/// <summary>
	/// The data path.
	/// </summary>
	public string dataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) ;
	// Use this for initialization
	instantiateAirplanesHighCondition instantiator;
	scoreScript scorer;

	void Start () {
		stopWatch = new Stopwatch();

//		myLog = new LogMsg("ATCLog_LC", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
	//	UnityEngine.Debug.Log (dataPath);
		myLog = new LogBitPupil(dataPath, "ATCLog_HC", "timestamp_write;timestamp_gotbitdata;CRC;SEQ;AnalogOutp1;AnalogOutp2;AnalogOutp3;AnalogOutp4;AnalogOutp5;AnalogOutp6;DigitalOutp1;DigitalOutp2;igitalOutp3;DigitalOutp4;" +
		                        "timestamp_gotpupil;eyetrackertime;norm_pos;diameter;confidence;nplanes;lastaction;lastactiondetails;positivescore;negativescore;targetpositions;collision_point");


		//myLog = new LogMsg("ATCLog_T", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
		// get input from bitalino copied from ConnectionState

		state.text = "";
		data.text = "";

		instantiator = GameObject.Find("Level").GetComponent<instantiateAirplanesHighCondition>();
		scorer = Camera.main.GetComponent<scoreScript> ();
		//UnityEngine.Debug.Log ("bin in start collect data");
	}

	// Update is called once per frame
	void FixedUpdate () {
		string bitalinodata = null;
		string pupildata = null;
		if (bitreader.asStart || (useeyetracker)) {
			if ( useeyetracker  && pupreader.on){
					pupildata = pupreader.getBuffer () [pupreader.pupBufferSize-1].Get ().ToString ();
				UnityEngine.Debug.Log (pupildata);
			}
			bitalinodata = bitreader.getBuffer () [bitreader.BufferSize - 1].ToString ();
			if (dataFile) {
				WriteinFile (bitalinodata, pupildata);
			}
			instantiator.lastAction=0;
			instantiator.lastActionDetails=0;
			instantiator.lastCrash.x = -2000;
			instantiator.lastCrash.y = -2000;
			instantiator.lastCrashAirplane.Clear();
			//UnityEngine.Debug.Log ("Collectdata from bitalino" + bitalinodata);
		}

		
		if (Input.GetKeyUp(KeyCode.Escape)){
			Application.Quit();
		}
	}

	private void WriteinFile(string bitdata,string pupildata)
	{
	//	UnityEngine.Debug.Log ("In WriteData");
		//pupildata = "getestet";

		string message2 = DateTime.Now.ToString("hh:mm:ss:fff")+";"+   bitdata + pupildata;
			
				int nPlanes = instantiator.nPlanes;
				int lastAction = instantiator.lastAction;
				int lastActionDetails = instantiator.lastActionDetails;
				int positiveScore = scorer.currentScorePositive;
				int negativeScore = (int) scorer.currentScoreNegative;
				string lastCrashAirplanes ="("+string.Join(",", instantiator.lastCrashAirplane.ToArray())+")";
				
				Vector2 lastCrash = instantiator.lastCrash;
				
		//		UnityEngine.Debug.Log ("Save line in collectdata");
				string completeline= message2 +  nPlanes.ToString() + ";" + lastAction.ToString() + ";" + lastActionDetails.ToString() + ";" + positiveScore.ToString() + ";" 
			+ negativeScore.ToString()+";"+ lastCrashAirplanes+";" + lastCrash.ToString()+";";
		//		UnityEngine.Debug.Log (completeline);
				myLog.LogMessage(completeline);
			//	UnityEngine.Debug.Log("Written");
	}

	/// <summary>
	/// Close this instance.
	/// </summary>
	public void close(){
		UnityEngine.Debug.Log ("Close Calibration");
	}

	void OnApplicationQuit(){
		close ();
		myLog.Close ();
	}	
	
}
