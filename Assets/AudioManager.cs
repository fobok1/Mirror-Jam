/*
* Copyright (c) B.J. Baye 2017
*/

﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

	#region Variables
	public Clip[] clips;
	#endregion

	#region Start/Update Methods
	// Use this for initialization
	void Awake ()
	{
		foreach (Clip sound in clips)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.sound;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;
		}
	}

	// Update is called once per frame
	public void Play(string name)
	{
		Clip s = Array.Find(clips,sound => sound.name == name);
		s.source.Play();
	}

	public void Stop(string name)
	{
		Clip s = Array.Find(clips, sound => sound.name == name);
		s.source.Stop();
	}
	#endregion
}
