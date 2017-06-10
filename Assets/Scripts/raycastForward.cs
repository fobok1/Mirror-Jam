/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastForward : MonoBehaviour {

	#region Variables
	[SerializeField]
	private float distance;
	#endregion

	#region Start/Update Methods

	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		Vector3 forward = transform.TransformDirection(Vector3.forward) * distance;

		Debug.DrawRay(transform.position, forward, Color.red);

		if (Physics.Raycast(transform.position, forward, out hit))
		{
			if (hit.collider != null)
			{
				if (hit.distance < distance)
				{
					Debug.Log(hit.collider.gameObject.name);
					if (hit.collider.gameObject.name == "Mirror")
					{
						Transform target = hit.collider.gameObject.transform;
						Rotate(target);
					}
					
				}
			}
		}
	}

	void Rotate(Transform _target)
	{
		Vector3 angles = new Vector3(0f, 0f, 0f);

		if (Input.GetButtonDown("RotateLeft"))
		{
			angles.y = 45;
		}
		else if (Input.GetButtonDown("RotateRight"))
		{
			angles.y = -45;
		}
		else
		{
			return;
		}

		_target.Rotate(angles);

	}

	#endregion
}
