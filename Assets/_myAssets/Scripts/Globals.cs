/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour {

	#region Variables
	[HideInInspector]
	public float lookSensitivity = 6f;
	
	#endregion

	#region Start/Update Methods
	private void Awake()
	{
		if (PlayerPrefs.HasKey("lookSensitivity"))
		{
			lookSensitivity = PlayerPrefs.GetFloat("lookSensitivity");
		}
	}

	public void SetSensitivity(float value)
	{
		lookSensitivity = value;
		PlayerPrefs.SetFloat("lookSensitivity", lookSensitivity);
	}
}

	#endregion
