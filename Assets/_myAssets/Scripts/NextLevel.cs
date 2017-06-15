/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {

	#region Variables
	int levelIndex;
	#endregion

	private void Start()
	{
		levelIndex = SceneManager.GetActiveScene().buildIndex;
	}

	public void LoadNextLevel()
	{
		SceneManager.LoadScene(levelIndex + 1);
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
