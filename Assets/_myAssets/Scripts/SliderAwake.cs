/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAwake : MonoBehaviour {

	#region Variables
	private Slider self;
	public Globals global;
	#endregion

	#region Start/Update Methods
	// Use this for initialization
	void Start ()
	{
		self = GetComponent<Slider>();
		if (PlayerPrefs.HasKey("lookSensitivity"))
		{
			self.value = PlayerPrefs.GetFloat("lookSensitivity");
		}
		else
		{
			self.value = global.lookSensitivity;
		}
			
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
	#endregion
}
