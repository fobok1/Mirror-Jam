/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]

public class RaycastReflection2 : MonoBehaviour
{
	public bool hasFired = false;

	public GameMaster gm;

    //this game object's Transform

    private Transform goTransform;

	//the attached line renderer

	private LineRenderer lineRenderer;

	//a ray

	private Ray ray;

	//a RaycastHit variable, to gather informartion about the ray's collision

	private RaycastHit hit;

	//reflection direction

	private Vector3 direction;

	//the number of reflections

	public int nReflections = 2;

	//max length

	public float maxLength = 100f;

	//the number of points at the line renderer

	private int numPoints;

	//private int pointCount;

	void Awake()

	{

		//get the attached Transform component  

		goTransform = this.GetComponent<Transform>();

		//get the attached LineRenderer component  

		lineRenderer = this.GetComponent<LineRenderer>();
		//lineRenderer.SetPosition(0, new Vector3(999f, 999f, 999f));


		lineRenderer.positionCount = 1;

		lineRenderer.SetPosition(0, goTransform.position);

	}

	void Update()

	{

		//clamp the number of reflections between 1 and int capacity  

		nReflections = Mathf.Clamp(nReflections, 1, nReflections);

		ray = new Ray(goTransform.position, goTransform.forward);

		lineRenderer.positionCount = 1;

		lineRenderer.SetPosition(0, goTransform.position);

		float remainingLength = maxLength;

		for (int i = 0; i <= nReflections; i++)
		{

			if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
			{

				lineRenderer.positionCount += 1;

				lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

				remainingLength -= Vector3.Distance(ray.origin, hit.point);

				ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

				if (hit.collider.tag != "Mirror")
				{
					
					gm.hitCheck(hit.collider.tag);
					hasFired = true;
					break;
				}

			}
			else
			{

				lineRenderer.positionCount += 1;

				lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);

				gm.hitCheck("None");
				hasFired = true;

				break;

			}

		}

	}

}

