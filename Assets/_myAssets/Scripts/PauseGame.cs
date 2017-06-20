/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	#region Variables
	[SerializeField]
	private GameObject menuPanel;
	[SerializeField]
	private PlayerController player;
	[SerializeField]
	private raycastMirror playerRay;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject welcome;
	[SerializeField]
	private GameObject optionsMenu;
	//private bool optionsShow = false;
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
				menuPanel.SetActive(false);
				pauseMenu.SetActive(false);
				Time.timeScale = 1;
				player.enabled = true;
				playerRay.enabled = true;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				optionsMenu.SetActive(false);
				menuPanel.SetActive(true);
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

	public void OpenOptions()
	{
		optionsMenu.SetActive(true);
		menuPanel.SetActive(true);
		pauseMenu.SetActive(false);
		Time.timeScale = 0;
		player.enabled = false;
		playerRay.enabled = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	#endregion

	public void CloseOptions()
	{
		optionsMenu.SetActive(false);
		menuPanel.SetActive(true);
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
		player.enabled = false;
		playerRay.enabled = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
