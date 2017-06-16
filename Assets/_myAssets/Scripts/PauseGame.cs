/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	#region Variables
	[SerializeField]
	private PlayerController player;
	[SerializeField]
	private raycastMirror playerRay;
	[SerializeField]
	private GameObject pauseMenu;
	#endregion

	#region Start/Update Methods
	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Pause"))
		{
			if (pauseMenu.activeSelf)
			{
				pauseMenu.SetActive(false);
				Time.timeScale = 1;
				player.enabled = true;
				playerRay.enabled = true;
			}
			else
			{
				pauseMenu.SetActive(true);
				Time.timeScale = 0;
				player.enabled = false;
				playerRay.enabled = false;
			}

		}
	}
	#endregion
}
