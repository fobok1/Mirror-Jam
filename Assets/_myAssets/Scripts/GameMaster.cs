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
	public TMPro.TMP_Text wall;
	//public TMPro.TMP_Text mirror;
	//public TMPro.TMP_Text laser;
	//public GameObject panel;
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
			StartCoroutine(HitAWall(tag));
			

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

	IEnumerator HitAWall(string _tag)
	{
		raycastMirror ray = FindObjectOfType<raycastMirror>();
		if (_tag == "Wall")
		{
			wall.text = "The laser hit a wall!";
		}
		else if (_tag == "MirrorTooltip")
		{
			wall.text = "The laser hit the side or back of a mirror!";
		}
		else if (_tag == "Field")
		{
			wall.text = "The laser hit a force field!";
		}
		else if (_tag == "Player")
		{
			wall.text = "The laser hit you!";
		}
		else 
		{
			wall.text = "The laser hit itself!";
		}

		wall.gameObject.SetActive(true);


		//mirror.gameObject.SetActive(false);
		//laser.gameObject.SetActive(false);
		//panel.SetActive(true);
		yield return new WaitForSeconds(ray.laserTime);
		wall.gameObject.SetActive(false);
		//panel.SetActive(false);
	}

	#endregion
}
