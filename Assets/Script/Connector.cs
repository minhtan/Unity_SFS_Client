using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;

public class Connector : MonoBehaviour {
	
	private SmartFox sfs;

	public string host = "127.0.0.1";
	public int port = 9933;
	public string zone = "MyExtension";
	public bool isDebug = true;

	// Use this for initialization
	void Start () {
		Connect ();
	}
	
	// Update is called once per frame
	void Update () {
		if (sfs != null)
			sfs.ProcessEvents();
	}

	public void Connect(){
		if (sfs == null || !sfs.IsConnected) {
			sfs = new SmartFox ();
			sfs.ThreadSafeMode = true;

			sfs.AddEventListener (SFSEvent.CONNECTION, OnConnection);
			sfs.AddEventListener (SFSEvent.CONNECTION_LOST, OnConnectionLost);

			ConfigData cfg = new ConfigData ();
			cfg.Host = host;
			cfg.Port = port;
			cfg.Zone = zone;
			cfg.Debug = isDebug;

			// Connect to SFS2X
			sfs.Connect (cfg);
		}
	}

	public void Disconnect(){
		sfs.Disconnect();
	}

	void OnConnection(BaseEvent evt){
		if ((bool)evt.Params ["success"]) {
			Debug.Log ("Connected");
		} else {
			reset ();
		}
	}

	void OnConnectionLost(BaseEvent evt){
		reset ();
	}

	void reset(){
		sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);

		sfs = null;
	}
}
