/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	#region Variables
	public GameObject hitText;
	public PlayerController player;
	public raycastMirror playerRay;
	public Slider charge;
	public GameObject loseText;
	public RaycastReflection2 laserScript;
	#endregion

	#region Methods

	private void Start()
	{
		charge.value = 3;
	}


	public void hitCheck(string tag)
	{
		if (tag == "Target")
		{
			player.enabled = false;
			playerRay.enabled = false;
			hitText.SetActive(true);
		}
		else
		{
			if (!laserScript.hasFired)
			{
				charge.value--;
				if (charge.value <= 0)
				{
					Debug.Log("Game over.");

					player.enabled = false;
					playerRay.enabled = false;
					loseText.SetActive(true);
				}
			}
		}
	}

	#endregion
}
