using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;
using System;

public class LogMsg {

	System.IO.StreamWriter logFile;
	
	public LogMsg (string name, string header) {

		string date = DateTime.Now.ToString("MM_dd_HH_mm");
		string folderPath = @"./logs/" + "/" + date +"/";
		
		if(Directory.Exists(folderPath) == false){
			DirectoryInfo di = Directory.CreateDirectory(folderPath);
			
		}
		string path = folderPath + name + ".csv";
		if(File.Exists(path)){
			path = folderPath + name + "(2).csv";
		}
		logFile = new System.IO.StreamWriter(path, true);
		//logFile = new System.IO.StreamWriter("C:/Users/xim/Desktop/Hello/my.txt", true);
	
		//logFile.WriteLine("#Starting log sesion " + Application.loadedLevelName);	
		logFile.WriteLine("%Timestamp," + header);	
	}
	
	public void LogMessage(string msg){
		string logEntry = "";
		//logEntry =  DateTime.Now+";"+ msg;
		Debug.Log (DateTime.Now.ToString("hh:mm:ss:fff"));
		logEntry = DateTime.Now.ToString("hh:mm:ss:fff") + "," + msg;

		logFile.WriteLine (logEntry);
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

