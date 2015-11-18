using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;
using System;

public class LogBitPupil{
	
	System.IO.StreamWriter logFile;
	private string dataPath;
	
	private System.IO.StreamWriter Set(string path )
	{
		logFile = new System.IO.StreamWriter(dataPath, true);
		return logFile;
	}
	private System.IO.StreamWriter Get( )
	{
		return logFile;
	}
	
	public LogBitPupil (string dataPa, string name, string header) {
		
		/*if(Directory.Exists(dataPath) == false){
			DirectoryInfo di = Directory.CreateDirectory(dataPath);
			
		}*/
		dataPath = dataPa +"\\" + DateTime.Now.ToString("MMddHHmmssfff") + "_Data.csv";
		Set (dataPath);
		//	Debug.Log (dataPath);
		
		
		//logFile = new System.IO.StreamWriter("C:/Users/xim/Desktop/Hello/my.txt", true);
		//logFile.WriteLine("#Starting log sesion " + Application.loadedLevelName);	
		Get ().WriteLine("%Timestamp," + header);	
		
	}
	
	public void LogMessage(string msg){
		//string logEntry = "";
		//logEntry =  DateTime.Now+";"+ msg;
		//logEntry = DateTime.Now.ToString("MM:dd:hh:mm:ss:fff",CultureInfo.InvariantCulture) + "," + msg;
		
		//logFile.WriteLine (logEntry);
		//	UnityEngine.Debug.Log ("LogMessages");
		//	Debug.Log (dataPath);
		
		//	Debug.Log ("Lo");
		Get ().WriteLine(msg);
		Get ().Flush ();
		//	Debug.Log (msg);
		
		
		
		/*
		logFile.WriteLine(System.DateTime.Now.ToString("HHmmssfff") +";"+ logEntry +";"+
		                  pointer.selectedRegion.ToString() +";"+ pointer.pointer_r.x.ToString() +";" + pointer.pointer_r.y.ToString() 
		                  + ";" +  glove.rightGrabDigit.ToString() + ";"+((int)experiment._condition).ToString() +";"+((int)experiment._task).ToString()  +";"+ 
		                  ((int)Selection._selectedOption).ToString()  +";" + areaInt.mainRegion.ToString() +";"+ areaInt.subRegion.ToString() + ";" + areaInt.combinedRegions.ToString() + ";" 
		                  + experiment.trialOrder.ToString() + ";");
*/
	}
	
	public void Close(){
		//logFile.WriteLine("#Ending log sesion");
		if(logFile != null)
			logFile.Close();		
	}
}

