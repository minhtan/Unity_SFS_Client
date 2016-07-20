using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Util {

	public static List<string> GetParamKeys(this IDictionary param){
		List<string> list =  new List<string>();

		foreach(string value in param.Keys){
			list.Add (value);
		}

		return list;
	}

	public static void DebugList(this List<string> list){
		foreach(string value in list){
			Debug.Log (value);
		}
	}
}
