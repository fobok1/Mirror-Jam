/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOptions : MonoBehaviour {

	#region Variables
	[SerializeField]
	private GameObject mainMenu;
	[SerializeField]
	private GameObject options;
	#endregion

	#region Start/Update Methods
	private void Update()
	{
		if (Input.GetButtonDown("Pause"))
		{
			closeOptions();
		}
	}

	public void openOptions()
	{
		options.SetActive(true);
		mainMenu.SetActive(false);
	}

	public void closeOptions()
	{
		options.SetActive(false);
		mainMenu.SetActive(true);
	}
	#endregion
}
