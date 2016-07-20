using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Logging;
using System.Collections.Generic;
using Sfs2X.Entities;

public class Connector : MonoBehaviour {
	
	private SmartFox sfs;

	public string host = "127.0.0.1";
	public int port = 9933;
	public string zone = "MyExtension";
	public bool isDebug = true;

	// Use this for initialization
	void Start () {
		_Connect ();
	}
	
	// Update is called once per frame
	void Update () {
		if (sfs != null)
			sfs.ProcessEvents();
	}

	#region connection

	public void _Connect(){
		if (sfs == null || !sfs.IsConnected) {
			sfs = new SmartFox ();
			sfs.ThreadSafeMode = true;

			sfs.AddEventListener (SFSEvent.CONNECTION, OnConnection);
			sfs.AddEventListener (SFSEvent.CONNECTION_LOST, OnConnectionLost);
			sfs.AddEventListener (SFSEvent.CONNECTION_RETRY, OnConnectionRetry);
			sfs.AddEventListener (SFSEvent.CONNECTION_RESUME, OnConnectionResume);

			sfs.AddLogListener(LogLevel.INFO, OnInfoMessage);
			sfs.AddLogListener(LogLevel.WARN, OnWarnMessage);
			sfs.AddLogListener(LogLevel.ERROR, OnErrorMessage);

			ConfigData cfg = new ConfigData ();
			cfg.Host = host;
			cfg.Port = port;
			cfg.Zone = zone;
			cfg.Debug = isDebug;

			// Connect to SFS2X
			sfs.Connect (cfg);
		}
	}

	public void _Disconnect(){
		sfs.Disconnect();
	}

	public void _KillConnection(){
		sfs.KillConnection ();
	}

	void OnConnection(BaseEvent evt){
		if ((bool)evt.Params ["success"]) {
			Debug.Log ("Connected" + evt.Params.Count);
		} else {
			reset ();
		}
	}

	void OnConnectionLost(BaseEvent evt){
		Debug.Log ("Disconnected " + (string)evt.Params["reason"]);
		reset ();
	}

	void OnConnectionRetry(BaseEvent evt){
		Debug.Log ("Connection loss, attemping to reconnect");
	}

	void OnConnectionResume(BaseEvent evt){
		Debug.Log ("Connection resumed");
	}

	public void OnInfoMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("INFO", message);
	}

	public void OnWarnMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("WARN", message);
	}

	public void OnErrorMessage(BaseEvent evt) {
		string message = (string)evt.Params["message"];
		ShowLogMessage("ERROR", message);
	}

	private void ShowLogMessage(string level, string message) {
		message = "[SFS > " + level + "] " + message;
		Debug.Log(message);
	}

	#endregion

	#region login

	public void _Login(){
		sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

		sfs.Send (new Sfs2X.Requests.LoginRequest ("test1","123456",zone));
	}

	void OnLogin(BaseEvent evt){
		User user = (User) evt.Params["user"];
		Debug.Log ("Logged in " + user.Name);
		evt.Params.GetParamKeys ().DebugList ();
	}

	void OnLoginError(BaseEvent evt){
		sfs.Disconnect ();
		Debug.Log ("Log in failed");
		evt.Params.GetParamKeys ().DebugList ();
	}

	#endregion


	void reset(){
		sfs.RemoveAllEventListeners ();
		sfs = null;
	}
}
