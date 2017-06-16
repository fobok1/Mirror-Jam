/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	#region Variables
	public GameObject hitText;
	public PlayerController player;
	#endregion

	#region Methods
	
	public void hitCheck(string tag)
	{
		if (tag == "Target")
		{
			player.enabled = false;
			hitText.SetActive(true);
		}
	}

	#endregion
}
