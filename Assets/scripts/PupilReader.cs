// Copyright (c) 2014, Tokyo University of Science All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met: 
//* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
//* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
//documentation and/or other materials provided with the distribution. * Neither the name of the Tokyo Univerity of Science nor the names 
//of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
//BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
//<COPYRIGHT HOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
//(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
//HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using UnityEngine;
using System.Collections;
using System.Threading;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class PupilReader : MonoBehaviour {
	public int pupBufferSize = 100;
	public int recievePortUDPPupil1 = 5000; //4353;//12987; 
	public UdpClient clientPupil1;
	private IPEndPoint remoteIpEndPointPupil1;
	private Thread pupThread;
	public PupilFrame[] pupilframeBuffer;
	private StreamWriter sw;
	private Stopwatch stopWatch;
	//public string dataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) ;
	public bool dataFile = false;

	public bool on = false;
	
	void Start()
	{
		stopWatch = new Stopwatch();
		//dataPath += "\\" + DateTime.Now.ToString("MMddHHmmssfff") + "_Data.csv";
		clientPupil1 = new UdpClient( recievePortUDPPupil1 );
		//clientPupil1.Client.Blocking = false;
		remoteIpEndPointPupil1 = new IPEndPoint( IPAddress.Any, recievePortUDPPupil1 );
		UnityEngine.Debug.Log ("I am in start pupilreader");
		pupilframeBuffer = new PupilFrame[pupBufferSize];
		StartCoroutine (start1 ());
	}
	private IEnumerator start1()
	{
		pupThread = new Thread( new ThreadStart(ReadPup) );
		pupThread.Name = "UDP listen thread";
	    yield return null;

		stopWatch.Start ();
		PupilFrame addframe = new PupilFrame ();
		string message = null;
		for (int i = 0; i < pupBufferSize; i++) {
			byte[] output = clientPupil1.Receive (ref remoteIpEndPointPupil1);
			message = Encoding.ASCII.GetString (output);
			if (message != null && message.Length > 0) {
				addframe.Set (message);
				pupilframeBuffer [i] = addframe;
			}
		}
		on = true;
		pupThread.Start ();
	}
	/// <summary>
	/// Read a frame and store it in the buffer
	/// </summary>
	private void ReadPup()
	{
		UnityEngine.Debug.Log ("Thread Pupil software");
		while (on)
		{
			try
			{
				PupilFrame eyetrackerData = new PupilFrame();
				int i;
				
				for (i = 0; i < pupBufferSize - 1; i++)
				{
					pupilframeBuffer[i] = pupilframeBuffer[i + 1];
				}
				UnityEngine.Debug.Log (pupThread.ThreadState);
				byte[] output = clientPupil1.Receive (ref remoteIpEndPointPupil1);
				string packet2 = Encoding.ASCII.GetString(output);
				packet2 = DateTime.Now.ToString("hh:mm:ss:fff")+ ";" + packet2;
				UnityEngine.Debug.Log(packet2);
				if (packet2 != null && packet2.Length > 0) {
					eyetrackerData.Set(packet2);
					pupilframeBuffer [i]=eyetrackerData;
					//WriteData(pupilframeBuffer[i]);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.Log( e.ToString() );
			}
		}
	}
	
	/// <summary>
	/// Return the content of the buffer
	/// </summary>
	/// <returns>Return the content of the buffer</returns>
	public PupilFrame[] getBuffer()
	{
		return this.pupilframeBuffer;
	}

	/// <summary>
	/// Save the read data in a file if data_file is true
	/// </summary>
	/// <param name="frame">data read</param>
/*	private void WriteData(PupilFrame frame)
	{
		try
		{
			if (dataFile)
			{
				string message2 = null;
				if (sw == null)
				{
					sw = File.AppendText(dataPath);

					byte[] packet2 = clientPupil1.Receive (ref remoteIpEndPointPupil1);

					if (packet2 != null && packet2.Length > 0) {
					//	Debug.Log ("Receiving packet");
						
						message2 = ExtractString (packet2, 0, packet2.Length);

						sw.WriteLine(message2);
					    sw.Flush();
					}
				}
				sw.WriteLine(CSV_Parser.ToCSV((stopWatch.Elapsed.TotalSeconds) + " " + message2,1));
				sw.Flush();
			}
		}
		catch (Exception e)
		{ UnityEngine.Debug.Log(e); }  
	}*/
	/// <summary>
	/// Stop the connection on the stop of the application
	/// </summary>
	public void close()
	{
		if (on == true) {
			on = false;
			if (pupThread != null)
				pupThread.Abort ();		
		}
		if( clientPupil1 != null ) clientPupil1.Close();			
		UnityEngine.Debug.Log ("Close Calibration");
	}
	
	void OnApplicationQuit()
	{
		close ();
	}	
	
	/// <summary>
	/// get the time since the start of the aquisition
	/// </summary>
	/// <returns>Return the time in S</returns>
	internal string getTime()
	{
		return stopWatch.Elapsed.TotalSeconds.ToString();
	}
	
}
