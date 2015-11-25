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

	public LogBitPupil myLog;
	public ManagerBITalino manager;
	public BITalinoReader bitreader;
	public PupilReader pupreader;

	public BITalinoSerialPort serial;

	//public GUIText eyetracker;
	private StreamWriter sw;

	public bool useeyetracker = false;
	public bool usebitalino = false;

	public bool dataFile = true;
	private Stopwatch stopWatch;
	public bool start = false;

	public string dataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) ;
	public string fileName ;
	public int experiment_time = 4*60;
	// Use this for initialization
	instantiateAirplanesHighCondition instantiator;
	scoreScript scorer;

	void Start () {
		stopWatch = new Stopwatch();
		stopWatch.Start ();
//		myLog = new LogMsg("ATCLog_LC", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
	//	UnityEngine.Debug.Log (dataPath);

		//myLog = new LogMsg("ATCLog_T", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
		// get input from bitalino copied from ConnectionState
		start = true;
		instantiator = GameObject.Find("Level").GetComponent<instantiateAirplanesHighCondition>();
		scorer = Camera.main.GetComponent<scoreScript> ();
		string header = "timestamp_write;";
		if (useeyetracker == true) {
			header = header + "timestamp_gotpupil;eyeTrackerTime;normPos;diameter;confidence;";
			//pupreader.clientPupil1.Client.Blocking=true;
		}
		if(usebitalino == true){
			header = header + "timestamp_gotbitdata;CRC;SEQ;AnalogOutp1;AnalogOutp2;AnalogOutp3;AnalogOutp4;AnalogOutp5;AnalogOutp6;DigitalOutp1;DigitalOutp2;DigitalOutp3;DigitalOutp4;";
		}
		myLog = new LogBitPupil(dataPath, fileName, header +
		                        "nplanes;listofAirplanesonScreen;lastAction;lastActionDetails;lastActionAirplaneNumber;calculatedScore;positiveScore;negativeScore;collidedAirplaneNumbers;targetPositions;collisionPoint");
	}

	// Update is called once per frame
	void FixedUpdate () {
		//UnityEngine.Debug.Log ("stop watch " + stopWatch.Elapsed.TotalSeconds + "experiment time"+ experiment_time);
		if (stopWatch.Elapsed.TotalSeconds <= experiment_time) {
			//UnityEngine.Debug.Log("in write " +stopWatch.Elapsed.TotalSeconds );
			string bitalinodata = null;
			string pupildata = null;
			if (usebitalino || (useeyetracker)) {
				if (useeyetracker && pupreader.on) {
					pupildata = pupreader.getBuffer () [pupreader.pupBufferSize - 1].Get ().ToString ();

				}
				if (usebitalino && bitreader.asStart) {
					bitalinodata = bitreader.getBuffer () [bitreader.BufferSize - 1].ToString ();

				}

			}
			if (dataFile) {
				WriteinFile (bitalinodata, pupildata);
			}
			instantiator.lastAction = 0;
			instantiator.lastActionDetails = 0;
			instantiator.lastActionAirPlaneNumber = "";
			instantiator.lastCrash.x = -2000;
			instantiator.lastCrash.y = -2000;
			instantiator.lastCrashAirplanes.Clear ();
			instantiator.lastCrashAirplanetargetposition.Clear ();
		}
		if(stopWatch.Elapsed.TotalSeconds > experiment_time){
			Application.Quit ();
		}
		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
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
		int nPlanes = 0;
		nPlanes = instantiator.nPlanes;
		int lastAction = 0;
		lastAction = instantiator.lastAction;

		int lastActionDetails = 0;
		lastActionDetails = instantiator.lastActionDetails;
		string lastActionAirPlaneNumber = "0";
		lastActionAirPlaneNumber = instantiator.lastActionAirPlaneNumber;
		if (lastActionAirPlaneNumber == "") {
			lastActionAirPlaneNumber = "0";
		}
		int calculatedScore = 0;
		calculatedScore=(int) scorer.currentScore;
		int positiveScore = 0;
		positiveScore=scorer.currentScorePositive;
		int negativeScore = 0;
		negativeScore= (int) scorer.currentScoreNegative;
		string crashedAir = "0";
		string onSreenAir = "0";
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
		i = 0;
		foreach(string airplane in instantiator.allAirplanesonScreen)
		{
			if (i != 0){
				onSreenAir = onSreenAir +"," + airplane;
			}else{
				onSreenAir = airplane;
			}
			i++;
		}

		string lastCrashAirplanestarget ="("+string.Join(",", instantiator.lastCrashAirplanetargetposition.ToArray())+")";
		Vector2 lastCrash =new Vector2 (-2000,-2000);
		lastCrash=instantiator.lastCrash;
		string completeline= message2 +  nPlanes.ToString() + ";" + onSreenAir + ";" + lastAction.ToString() + ";" + lastActionDetails.ToString()+ ";" + lastActionAirPlaneNumber + ";" + calculatedScore.ToString() + ";" + positiveScore.ToString() + ";" 
			+ negativeScore.ToString()+";"+ crashedAir +";"+ lastCrashAirplanestarget+";" + lastCrash.ToString()+";";

		//UnityEngine.Debug.Log (completeline);
		myLog.LogMessage(completeline);
		//UnityEngine.Debug.Log("Written");
	}
	internal string getTime()
	{
		return stopWatch.Elapsed.TotalSeconds.ToString();
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