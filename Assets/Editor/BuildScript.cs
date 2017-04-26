// Adapted from https://conquertheworldbycode.wordpress.com/2013/11/03/unity3d-jenkins-2-job-configuration-webbuild/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
 
public class BuildScript{
 
	static string[] SCENES = FindEnabledEditorScenes();
	 
	// static string APP_NAME = "<yourAPPName>";
	// static string TARGET_DIR = "<YourTargetDir>";
	static string APP_NAME = "unity-jenkins-test";
	static string TARGET_DIR = "D:/unity-jenkins-test/Builds/";
	 
	// Windows -------

	[MenuItem ("Custom/CI/Build Windows 64-bit")]
	static void PerformWinBuild (){
		string target_dir = APP_NAME + ".exe";
		GenericBuild(SCENES, TARGET_DIR + "/win/" + target_dir, BuildTarget.StandaloneWindows64,BuildOptions.None);
	}

	[MenuItem ("Custom/CI/Build Windows 32-bit")]
	static void PerformWin32Build (){
		string target_dir = APP_NAME + "_32.exe";
		GenericBuild(SCENES, TARGET_DIR + "/win_3232/" + target_dir, BuildTarget.StandaloneWindows,BuildOptions.None);
	}

	// Linux ------

	[MenuItem ("Custom/CI/Build Linux 64-bit")]
	static void PerformLinuxBuild (){
		string target_dir = APP_NAME + "_32.exe";
		GenericBuild(SCENES, TARGET_DIR + "/linux/" + target_dir, BuildTarget.StandaloneLinux64,BuildOptions.None);
	}

	[MenuItem ("Custom/CI/Build Linux 32-bit")]
	static void PerformLinux32Build (){
		string target_dir = APP_NAME + "_32.exe";
		GenericBuild(SCENES, TARGET_DIR + "/linux_32/" + target_dir, BuildTarget.StandaloneLinux,BuildOptions.None);
	}

	// Mac OS X -------
	 
	[MenuItem ("Custom/CI/Build Mac OS X")]
	static void PerformMacOSXBuild (){
		string target_dir = APP_NAME + ".app";
		GenericBuild(SCENES, TARGET_DIR + "/mac/" + target_dir, BuildTarget.StandaloneOSXIntel,BuildOptions.None);
	}

	// Web Build -------
	 
	[MenuItem ("Custom/CI/Build Web")]
	static void PerformWebBuild (){
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