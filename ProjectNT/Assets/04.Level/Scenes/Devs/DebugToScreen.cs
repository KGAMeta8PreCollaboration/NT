using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugToScreen : MonoBehaviour {
	string myLog = "*begin log";
	Queue myLogQueue = new Queue ();
	public TextMeshProUGUI myLogText;

	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// if (scene.name == "YKD_GameScene")
		// {
		// 	print("찾음 : " + GameObject.Find("DebugText"));
		// }
			myLogText = GameObject.Find("DebugText").GetComponent<TextMeshProUGUI>();
	}

	void OnEnable () {
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable () {
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog (string logString, string stackTrace, LogType type) {
		myLog = logString;
		string newString = "\n [" + type + "] : " + myLog;
		myLogQueue.Enqueue (newString);
		if (type == LogType.Exception) {
			newString = "\n" + stackTrace;
			myLogQueue.Enqueue (newString);
		}
		myLog = string.Empty;
		foreach (string mylog in myLogQueue) {
			myLog += mylog;
		}
	}

	private void Update()
	{
		if (myLogText)
			myLogText.text = myLog;
	}

}
