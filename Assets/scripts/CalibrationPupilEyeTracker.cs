using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;


//using System.IO;

public class CalibrationPupilEyeTracker : MonoBehaviour {
	//Pupil ET
	public int recievePortUDPPupil = 5000; //4353;//12987; 
	private UdpClient clientPupil;
	private IPEndPoint remoteIpEndPointPupil;
	private System.Threading.Thread lThread;
	bool on = true;
	List<Vector2> marker_positions = new List<Vector2>(); 
	public Texture2D marker;
	float marker_size;
	int curr_marker = 0;
	int transparency = 0;
	public LogMsg myLog;

	public ManagerBITalino manager;
	public BITalinoReader bitreader;
	public PupilReader pupreader;
	public BITalinoSerialPort serial;
	public GUIText state;
	public GUIText data;
	public GUIText eyetracker;
	// Use this for initialization
	instantiatePlanes instantiator;
	scoreScript scorer;
	string  message2 = null;

	void Start () {
//		myLog = new LogMsg("ATCLog_LC", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
		myLog = new LogMsg("ATCLog_HC", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
		//myLog = new LogMsg("ATCLog_T", "pupilsize,timestamp,nplanes,lastaction,lastactiondetails,positivescore,negativescore");
		clientPupil = new UdpClient( recievePortUDPPupil );
		remoteIpEndPointPupil = new IPEndPoint( IPAddress.Any, recievePortUDPPupil );
		lThread = new System.Threading.Thread( new System.Threading.ThreadStart(listenUDPThreadPupil) );
		lThread.Name = "UDP listen thread1";
		lThread.Start();

		float w = Screen.width;
		float h = Screen.height;
		float w_offset = 600.0f;
		float h_offset = 100.0F;
		marker_size = 100.0f;
		float middle_offset = 200.0f;
		// get input from bitalino copied from ConnectionState
		state.text = "";
		data.text = "";
		StartCoroutine(startBit());

		/// <summary>
		/// Initialise the connection
		/// </summary>

		
		//		marker_positions.Add(new Vector2(w *(3.0f/8.0f) + middle_offset, h/2));
//		marker_positions.Add(new Vector2(w/4.0F + w_offset, h/2));
//		marker_positions.Add(new Vector2(w/4.0F + w_offset, 0.0F + h_offset));
//		marker_positions.Add(new Vector2(w/2.0F, 0.0F + h_offset));
//		marker_positions.Add(new Vector2(w - w/4.0F - w_offset, 0.0F + h_offset));
//		marker_positions.Add(new Vector2(w - w/4.0F - w_offset, h/2));
//		marker_positions.Add(new Vector2(w - w/4.0F - w_offset, h - h_offset ));
//		marker_positions.Add(new Vector2(w/2.0F, h - h_offset));
//		marker_positions.Add(new Vector2(w/4.0F + w_offset, h - h_offset));
//		marker_positions.Add(new Vector2(w - w *(3.0f/8.0f) -middle_offset, h/2));

		///////
		instantiator = GameObject.Find("Level").GetComponent<instantiatePlanes>();
		scorer = Camera.main.GetComponent<scoreScript> ();
	}

	private IEnumerator startBit()
	{
		state.text = "Connecting port " + serial.portName;
		while (!manager.IsReady)
			yield return new WaitForSeconds(0.5f);
		state.text = "Connected";
		while ((int)manager.Acquisition_State != 0)
			yield return new WaitForSeconds(0.5f);
		state.text = "Acquisition start";
		
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		string bitalinodata;
		if (bitreader.asStart)
		{
			bitalinodata = bitreader.getBuffer()[bitreader.BufferSize - 1].ToString();
			Debug.Log(bitalinodata);
		}
		string pupildata;
		/*if (pupreader.lThread.IsAlive)
		{
			pupildata = pupreader.getBuffer()[pupreader.PupBufferSize - 1].ToString();
			Debug.Log(pupildata);
		}*/
		
		if (Input.GetKeyUp(KeyCode.Escape)){
			Application.Quit();
		}
	
	}

//	void OnGUI(){
//
////		foreach (Vector2 pos in marker_positions){
//			GUI.DrawTexture(new Rect(marker_positions[curr_marker].x - marker_size/2.0f, marker_positions[curr_marker].y - marker_size/2.0f, marker_size, marker_size), marker, ScaleMode.ScaleToFit);
////			GUI.DrawTexture(new Rect(pos.x - marker_size/2.0f, pos.y - marker_size/2.0f, marker_size, marker_size), marker, ScaleMode.ScaleToFit);
////		}
//	}

	private void listenUDPThreadPupil()
	{
		Debug.Log ("Thread Pupil software");
		string bitalinodata;
		while( on )
		{  
			try
			{

				//if (bitreader.asStart)
				//{
				//	bitalinodata = reader.getBuffer()[reader.BufferSize - 1].ToString();
				//	Debug.Log(bitalinodata);
				//}

				//IMU Data
				byte[] packet2 = clientPupil.Receive( ref remoteIpEndPointPupil );
				
				if( packet2 != null && packet2.Length > 0 ){
					Debug.Log("Receiving packet");
					//string glove = System.Text.Encoding.Unicode.GetByteCount(packet2);
				
					//String glove = System.Convert.ToString(packet2);
				//	string glove = System.Text.Encoding.Default.GetString(packet2);	
						//packet2.ToString;
					//Debug.Log(glove);
					//
					message2 = ExtractString( packet2, 0, packet2.Length );

					//Debug.Log(message2);
					//int nPlanes = GameObject.FindGameObjectsWithTag("Plane").Length;
					int nPlanes = instantiator.nPlanes;
					int lastAction = instantiator.lastAction;
					int lastActionDetails = instantiator.lastActionDetails;
					int positiveScore = scorer.currentScorePositive;
					int negativeScore = (int) scorer.currentScoreNegative;
					instantiator.lastAction = 0;
					try {
						myLog.LogMessage(message2 + "," + nPlanes.ToString() + "," + lastAction.ToString() + "," + lastActionDetails.ToString() + "," + positiveScore.ToString() + "," + negativeScore.ToString());
					}
					catch (Exception e) {
						Debug.Log("Exception!!!! " + e.ToString());
					}
					Debug.Log("Written");
//					message2 = message2.Replace(",",".");
//					string [] messageSplit2 = message2.Split(new Char [] {';'});
//
//
//					try
//					{
//						curr_marker = Convert.ToInt32(messageSplit2[1]);
////						transparency = Convert.ToInt32(messageSplit2[2]);
//						Debug.Log(messageSplit2[0]);
//					}
//					catch (FormatException e)
//					{
//						Debug.Log("Input string is not a sequence of digits.");
//					}
//					if(messageSplit2.Length != 1){
//						
//						//float.TryParse(messageSplit2[0], out x_screen);
//						//float.TryParse(messageSplit2[1], out y_screen);
//						
//						//float.TryParse(messageSplit2[2], out pupil_ray);
//						//float.TryParse(messageSplit2[3], out timestamp);
//					}	
//					
//					//raw_gaze_x = x_screen;
//					//raw_gaze_y = y_screen;
				}

			}
			catch (Exception e)
			{
				Debug.Log( e.ToString() );
			}
			
		}
	}	
	
	public void close(){
		on = false;
		if (lThread != null) lThread.Abort();		
		if( clientPupil != null ) clientPupil.Close();			
		Debug.Log ("Close Calibration");
	}

	void OnApplicationQuit(){
		close ();
		myLog.Close ();
	}	


	private string ExtractString( byte[] packet, int start, int length )
	{
		StringBuilder sb = new StringBuilder();
		for( int i=0; i<packet.Length; i++ ) sb.Append( (char) packet[ i ] );
		return sb.ToString();
	}	

}
