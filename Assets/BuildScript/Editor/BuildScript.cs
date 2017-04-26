// Adapted from https://conquertheworldbycode.wordpress.com/2013/11/03/unity3d-jenkins-2-job-configuration-webbuild/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
 
public class BuildScript{
 
	static string[] SCENES = FindEnabledEditorScenes();
	 
	static string APP_NAME = "<yourAPPName>";
	static string TARGET_DIR = "<YourTargetDir>";
	//static string APP_NAME = "unity-jenkins-test";
	//static string TARGET_DIR = "D:/unity-jenkins-test/Builds/";

	// Directory and name functions -------

	static void SetNameAndDir(){
		APP_NAME = FindAppName();
		TARGET_DIR = FindTargetDir();
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
	 
	// Windows -------

	[MenuItem ("Custom/CI/Build Windows 64-bit")]
	static void PerformWinBuild (){
		SetNameAndDir();
		string target_dir = APP_NAME + ".exe";
		GenericBuild(SCENES, TARGET_DIR + "/win/" + target_dir, BuildTarget.StandaloneWindows64,BuildOptions.None);
	}

	[MenuItem ("Custom/CI/Build Windows 32-bit")]
	static void PerformWin32Build (){
		SetNameAndDir();
		string target_dir = APP_NAME + "_32.exe";
		GenericBuild(SCENES, TARGET_DIR + "/win_3232/" + target_dir, BuildTarget.StandaloneWindows,BuildOptions.None);
	}

	// Linux ------

	[MenuItem ("Custom/CI/Build Linux 64-bit")]
	static void PerformLinuxBuild (){
		SetNameAndDir();
		string target_dir = APP_NAME + "_32.exe";
		GenericBuild(SCENES, TARGET_DIR + "/linux/" + target_dir, BuildTarget.StandaloneLinux64,BuildOptions.None);
	}

	[MenuItem ("Custom/CI/Build Linux 32-bit")]
	static void PerformLinux32Build (){
		SetNameAndDir();
		string target_dir = APP_NAME + "_32.exe";
		GenericBuild(SCENES, TARGET_DIR + "/linux_32/" + target_dir, BuildTarget.StandaloneLinux,BuildOptions.None);
	}

	// Mac OS X -------
	 
	[MenuItem ("Custom/CI/Build Mac OS X")]
	static void PerformMacOSXBuild (){
		SetNameAndDir();
		string target_dir = APP_NAME + ".app";
		GenericBuild(SCENES, TARGET_DIR + "/mac/" + target_dir, BuildTarget.StandaloneOSXIntel,BuildOptions.None);
	}

	// Web Build -------
	 
	[MenuItem ("Custom/CI/Build Web")]
	static void PerformWebBuild (){
		SetNameAndDir();
		string target_dir = APP_NAME + ".unity3d";
		GenericBuild(SCENES, TARGET_DIR + "/web/" + target_dir, BuildTarget.WebPlayer,BuildOptions.None);
	}

	// Actual build functions -------

	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}
	 
	static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options){
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
		if (res.Length > 0) {
			throw new Exception("BuildPlayer failure: " + res);
		}
	}
 
}