/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDetect : MonoBehaviour {

	#region Variables
	private AmplifyMotionEffect amp;
	private HBAO hb;
	private MadGoat_SSAA.MadGoatSSAA mad;
	#endregion

	#region Start/Update Methods
	// Use this for initialization
	private void Awake()
	{
		amp = GetComponent<AmplifyMotionEffect>();
		hb = GetComponent<HBAO>();
		mad = GetComponent<MadGoat_SSAA.MadGoatSSAA>();
		int level = QualitySettings.GetQualityLevel();
		if (level < 4)
		{
			mad.enabled = false;
			hb.enabled = false;
		}
		if (level < 3)
		{
			amp.enabled = false;
		}
	}
	#endregion
}
