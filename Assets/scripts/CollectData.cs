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
	public bool usebitalino = false;
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

		//myLog = new LogMsg("ATCLog_T", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
		// get input from bitalino copied from ConnectionState

		state.text = "";
		data.text = "";

		instantiator = GameObject.Find("Level").GetComponent<instantiateAirplanesHighCondition>();
		scorer = Camera.main.GetComponent<scoreScript> ();
		string header = "timestamp_write;";


		if (useeyetracker == true) {
			header = header + "timestamp_gotpupil;eyetrackertime;norm_pos;diameter;confidence;";
			pupreader.clientPupil1.Client.Blocking=true;
		}
		if(usebitalino == true){
			header = header + "timestamp_gotbitdata;CRC;SEQ;AnalogOutp1;AnalogOutp2;AnalogOutp3;AnalogOutp4;AnalogOutp5;AnalogOutp6;DigitalOutp1;DigitalOutp2;igitalOutp3;DigitalOutp4;";
		}
		myLog = new LogBitPupil(dataPath, "ATCLog_HC", header +
		                        "nplanes;lastaction;lastactiondetails;lastactionairplanenumber;positivescore;negativescore;collidedairplanenumbers;targetpositions;collision_point");
		


		//UnityEngine.Debug.Log ("bin in start collect data");
	}

	// Update is called once per frame
	void FixedUpdate () {
		string bitalinodata = null;
		string pupildata = null;
		if (usebitalino || (useeyetracker)) {
			if (useeyetracker  && pupreader.on){
				pupildata = pupreader.getBuffer () [pupreader.pupBufferSize-1].Get ().ToString ();
			}
			if(usebitalino && bitreader.asStart){
				bitalinodata = bitreader.getBuffer () [bitreader.BufferSize - 1].ToString ();
			}

		
			//UnityEngine.Debug.Log ("Collectdata from bitalino" + bitalinodata);
		}
		if (dataFile) {
			WriteinFile (bitalinodata, pupildata);
		}
		instantiator.lastAction=0;
		instantiator.lastActionDetails=0;
		instantiator.lastActionAirPlaneNumber = "";
		instantiator.lastCrash.x = -2000;
		instantiator.lastCrash.y = -2000;
		instantiator.lastCrashAirplanes.Clear ();
		instantiator.lastCrashAirplanetargetposition.Clear();
		if (Input.GetKeyUp(KeyCode.Escape)){
			Application.Quit();
		}
	}

	private void WriteinFile(string bitalinodata,string pupildata)
	{
		if ((usebitalino == true) && (bitalinodata == null)) {
			bitalinodata = ";;;;;;;;;;;;;";
		}
		if ((useeyetracker == true) && (pupildata == null)) {
			pupildata = ";;;;;";
		}
		string message2 = DateTime.Now.ToString("hh:mm:ss:fff") + ";" +    pupildata +bitalinodata;
		int nPlanes = instantiator.nPlanes;
				int lastAction = instantiator.lastAction;
				int lastActionDetails = instantiator.lastActionDetails;
				string lastActionAirPlaneNumber = instantiator.lastActionAirPlaneNumber;
				int positiveScore = scorer.currentScorePositive;
				int negativeScore = (int) scorer.currentScoreNegative;
				string crashedAir = "";
		int i = 0;
				foreach(string airplane in instantiator.lastCrashAirplanes)
				{
			if (i != 0){
				crashedAir = crashedAir +"," + airplane;
			}else{
				crashedAir = airplane;
			}
			i++;
				}

				string lastCrashAirplanestarget ="("+string.Join(",", instantiator.lastCrashAirplanetargetposition.ToArray())+")";
				string lastCrashAirplanes ="("+crashedAir+")";
				Vector2 lastCrash = instantiator.lastCrash;
		//		UnityEngine.Debug.Log ("Save line in collectdata");
				string completeline= message2 +  nPlanes.ToString() + ";" + lastAction.ToString() + ";" + lastActionDetails.ToString()+ ";" + lastActionAirPlaneNumber + ";" + positiveScore.ToString() + ";" 
			+ negativeScore.ToString()+";"+ lastCrashAirplanes +";"+ lastCrashAirplanestarget+";" + lastCrash.ToString()+";";
				//UnityEngine.Debug.Log (completeline);
				myLog.LogMessage(completeline);
			//	UnityEngine.Debug.Log("Written");
	}

	/// <summary>
	/// Close this instance.
	/// </summary>
	public void close(){
		bitreader.close ();
		pupreader.close ();
		UnityEngine.Debug.Log ("Close Calibration");
	}

	void OnApplicationQuit(){
		close ();
		myLog.Close ();
	}	
	
}
