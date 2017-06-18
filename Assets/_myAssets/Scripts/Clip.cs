/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;

[System.Serializable]
public class Clip {

	public string name;

	public AudioClip sound;

	[Range(0f, 1f)]
	public float volume;

	[Range(0.1f, 3f)]
	public float pitch;

	public bool loop;

	[HideInInspector]
	public AudioSource source;

}
