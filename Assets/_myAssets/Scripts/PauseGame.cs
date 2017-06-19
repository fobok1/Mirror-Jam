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
	[SerializeField]
	private GameObject welcome;
	#endregion

	#region Start/Update Methods
	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 0;
		player.enabled = false;
		playerRay.enabled = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		welcome.SetActive(true);
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
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				pauseMenu.SetActive(true);
				Time.timeScale = 0;
				player.enabled = false;
				playerRay.enabled = false;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}

		}
	}

	public void StartGame()
	{
		Time.timeScale = 1;
		player.enabled = true;
		playerRay.enabled = true;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		welcome.SetActive(false);
	}
	#endregion
}
