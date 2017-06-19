/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	#region Variables
	public GameObject hitText; // Manually set.
	public PlayerController player; // Manually set.
	public raycastMirror playerRay; // Manually set.
	public Slider charge; // Manually set.
	public GameObject loseText; // Manually set.
	public RaycastReflection2 laserScript; // Manually set.
	[SerializeField]
	private GameObject menuPanel;
	private bool hasWon;
	#endregion

	#region Methods

	private void Start()
	{
		hasWon = false;
		charge = FindObjectOfType<Slider>(); // Since manually setting was causing NREs
		charge.value = 3;
	}


	public void hitCheck(string tag)
	{
		if (tag == "Target")
		{
			player.enabled = false;
			playerRay.enabled = false;
			menuPanel.SetActive(true);
			hitText.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			hasWon = true;
		}
		else
		{
			if (!laserScript.hasFired)
			{
				charge.value--;
				if (charge.value <= 0)
				{
					StartCoroutine(LoseGame());
				}
			}
		}
	}

	IEnumerator LoseGame()
	{
		raycastMirror ray = FindObjectOfType<raycastMirror>();
		yield return new WaitForSeconds(ray.laserTime);
		if (!hasWon)
		{
			Debug.Log("Game over.");

			player.enabled = false;
			playerRay.enabled = false;
			menuPanel.SetActive(true);
			loseText.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
	#endregion
}
