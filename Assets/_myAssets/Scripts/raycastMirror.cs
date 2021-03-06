/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class raycastMirror : MonoBehaviour {

	#region Variables
	[SerializeField]
	private float distance;
	public float laserTime = 5f;
	public RaycastReflection2 laserScript;
	public LineRenderer laser;
	public GameObject mirrorTooltip;
	public GameObject laserTooltip;
	public GameObject panel;
	public Camera cam;
	public MeshRenderer button;
	public Material green;
	public Material red;
	public GameObject fireLight;
	public AudioManager manager;
	public Slider charge;
	public Light spot;
	public SphereCollider lightSphere;
	public MeshRenderer console;
	#endregion

	#region Start/Update Methods

	// Update is called once per frame

	private void Start()
	{
		cam = GetComponentInChildren<Camera>();
	}

	/// <summary>
	/// Detects what the player is looking at.
	/// </summary>

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
					if (hit.collider.tag == "Mirror")
					{
						if (panel != null) // in case I take out tooltips in a later level.
						{
							panel.SetActive(true);
							laserTooltip.SetActive(false);
							mirrorTooltip.SetActive(true);
						}
						Transform target = hit.collider.gameObject.transform.parent.gameObject.transform; //mirror object is a parent of overall object
						Rotate(target);
					}
					else if (hit.collider.tag == "MirrorTooltip")
					{
						if (panel != null)
						{
							panel.SetActive(true);
							laserTooltip.SetActive(false);
							mirrorTooltip.SetActive(true);
						}
						Transform target = hit.collider.gameObject.transform;
						Rotate(target);
					}
					else if (hit.collider.tag == "Laser")
					{
						if (panel != null)
						{
							panel.SetActive(true);
							mirrorTooltip.SetActive(false);
							laserTooltip.SetActive(true);
						}
						if (Input.GetButtonDown("Activate"))
						{
							StartCoroutine(FireLaser());
						}
					}
					else if (hit.collider.tag == "laserTooltip")
					{
						if (panel != null)
						{
							panel.SetActive(true);
							mirrorTooltip.SetActive(false);
							laserTooltip.SetActive(true);
						}
					}
					else if (hit.collider.tag == "Switch")
					{
						if (Input.GetButtonDown("Activate"))
						{
							charge.maxValue = 5;
							charge.value = 5;
							spot.enabled = false;
							lightSphere.enabled = false;
							Material[] _mats = console.materials;
							_mats[1] = green;
							console.materials = _mats;
						}
						Debug.Log("Switch!");

					}
					else
					{
						if (panel != null)
						{
							laserTooltip.SetActive(false);
							mirrorTooltip.SetActive(false);
							panel.SetActive(false);
						}
					}


				}
			}
			
		}
		else
		{
			if (panel != null)
			{
				laserTooltip.SetActive(false);
				mirrorTooltip.SetActive(false);
				panel.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Rotates a mirror.
	/// </summary>

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
		manager.Play("Creak");

	}

	#endregion


	/// <summary>
	/// Fires a laser on a 5 second timer, and plays necessary sounds and effects.
	/// </summary>
	/// <returns></returns>

	IEnumerator FireLaser ()
	{
		AudioManager manager = FindObjectOfType<AudioManager>();
		Material[] mats = button.materials;
		laser.enabled = true;
		laserScript.enabled = true;
		mats[3] = red;
		button.materials = mats;
		manager.Play("Laser");
		fireLight.SetActive(true);
		yield return new WaitForSeconds(laserTime);
		laser.enabled = false;
		laserScript.enabled = false;
		laserScript.hasFired = false;
		mats[3] = green;
		button.materials = mats;
		manager.Stop("Laser");
		laserScript.sparks.SetActive(false);
		laserScript.sparksLight.gameObject.SetActive(false);
		fireLight.SetActive(false);

	}
}
