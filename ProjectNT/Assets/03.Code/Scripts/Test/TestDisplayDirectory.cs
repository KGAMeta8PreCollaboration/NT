 using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestDisplayDirectory : MonoBehaviour
{
	private TextMeshProUGUI _textMeshProUGUI;

	private void Start()
	{
		_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
	}
	
	private void Update()
	{
		_textMeshProUGUI.text = "경로 : " + Application.persistentDataPath;
	}
}
