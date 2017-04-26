#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class FunctionTesting : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string testName = FindAppName();
		Debug.Log("Test Name: " + testName);
		string testDir = FindTargetDir();
		Debug.Log("Test Dir: " + testDir);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Try to find the app name in the Resources/BuildScriptSettings.ini file
	static string FindAppName(){
		bool successFindName = true;
		string nameOut = "";
		try{
			TextAsset buildScriptSettingsAsset = Resources.Load("BuildScriptSettings") as TextAsset;
			string settingsText = buildScriptSettingsAsset.text;
			if(settingsText.Equals("")) successFindName = false;
			string[] textLines = settingsText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			if(textLines.Length < 2) successFindName = false;
			if(successFindName){
				string[] lineSplit = textLines[0].Split(new string[] { " = ", "=" }, StringSplitOptions.RemoveEmptyEntries);
				if(lineSplit.Length < 2) successFindName = false;
				if(successFindName){
					nameOut = lineSplit[1].Trim();
				}
			}
		}catch {
			nameOut = PlayerSettings.productName;
			successFindName = false;
		}

		if(nameOut.Equals("") || !successFindName) nameOut = PlayerSettings.productName;
		return nameOut;
	}

	// Try to find the target directory in the Resources/BuildScriptSettings.ini file
	static string FindTargetDir(){
		bool successFindDir = true;
		string dirOut = "";
		try{
			TextAsset buildScriptSettingsAsset = Resources.Load("BuildScriptSettings") as TextAsset;
			string settingsText = buildScriptSettingsAsset.text;
			if(settingsText.Equals("")) successFindDir = false;
			string[] textLines = settingsText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			if(textLines.Length < 2) successFindDir = false;
			if(successFindDir){
				string[] lineSplit = textLines[1].Split(new string[] { " = ", "=" }, StringSplitOptions.RemoveEmptyEntries);
				if(lineSplit.Length < 2) successFindDir = false;
				if(successFindDir){
					dirOut = lineSplit[1].Trim();
				}
			}
		}catch {
			dirOut = Application.dataPath + "/../Builds/";
			successFindDir = false;
		}

		if(dirOut.Equals("") || !successFindDir) dirOut = Application.dataPath + "/../Builds/";
		return dirOut;
	}
}
#endif