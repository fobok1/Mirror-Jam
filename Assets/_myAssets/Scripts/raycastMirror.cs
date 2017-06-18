/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastMirror : MonoBehaviour {

	#region Variables
	[SerializeField]
	private float distance;
	[SerializeField]
	private float laserTime = 5f;
	public RaycastReflection2 laserScript;
	public LineRenderer laser;
	public GameObject mirrorTooltip;
	public GameObject laserTooltip;
	public GameObject panel;
	public Camera cam;
	public MeshRenderer button;
	public Material green;
	public Material red;
	#endregion

	#region Start/Update Methods

	// Update is called once per frame

	private void Start()
	{
		cam = GetComponentInChildren<Camera>();
	}

	void Update ()
	{
		RaycastHit hit;
		Vector3 forward = cam.transform.TransformDirection(Vector3.forward) * distance;

		Debug.DrawRay(cam.transform.position, forward, Color.red);

		if (Physics.Raycast(cam.transform.position, forward, out hit))
		{
			if (hit.collider != null)
			{
				if (hit.distance < distance)
				{
					//Debug.Log(hit.collider.gameObject.name);
					if (hit.collider.tag == "Mirror")
					{
						panel.SetActive(true);
						laserTooltip.SetActive(false);
						mirrorTooltip.SetActive(true);
						Transform target = hit.collider.gameObject.transform.parent.gameObject.transform;
						Rotate(target);
					}
					else if (hit.collider.tag == "MirrorTooltip")
					{
						panel.SetActive(true);
						laserTooltip.SetActive(false);
						mirrorTooltip.SetActive(true);
						Transform target = hit.collider.gameObject.transform;
						Rotate(target);
					}
					else if (hit.collider.tag == "Laser")
					{
						panel.SetActive(true);
						mirrorTooltip.SetActive(false);
						laserTooltip.SetActive(true);
						if (Input.GetButtonDown("Activate"))
						{
							StartCoroutine(FireLaser());
						}
					}
					else if (hit.collider.tag == "laserTooltip")
					{
						panel.SetActive(true);
						mirrorTooltip.SetActive(false);
						laserTooltip.SetActive(true);
					}
					else
					{
						laserTooltip.SetActive(false);
						mirrorTooltip.SetActive(false);
						panel.SetActive(false);
					}


				}
			}
			
		}
		else
		{
			laserTooltip.SetActive(false);
			mirrorTooltip.SetActive(false);
			panel.SetActive(false);
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

	IEnumerator FireLaser ()
	{
		AudioManager manager = FindObjectOfType<AudioManager>();
		Material[] mats = button.materials;
		laser.enabled = true;
		laserScript.enabled = true;
		mats[3] = red;
		button.materials = mats;
		manager.Play("Laser");
		yield return new WaitForSeconds(laserTime);
		laser.enabled = false;
		laserScript.enabled = false;
		laserScript.hasFired = false;
		mats[3] = green;
		button.materials = mats;
		manager.Stop("Laser");

	}
}
