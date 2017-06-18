/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	#region Variables
	public static GameObject instance;
	#endregion

	#region Start/Update Methods
	// Use this for initialization
	void Awake ()
	{
		DontDestroyOnLoad(this);
		if (instance == null)
		{
			instance = this.gameObject;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
	#endregion
}
