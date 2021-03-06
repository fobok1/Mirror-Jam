/*
* Copyright (c) B.J. Baye 2017 - with credit to Brackeys for tutorial!
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {

	[Range(1f, 20f)]
    public float speed = 5f;
	public Globals global;
	//[Range(1f, 20f)]
	//public float lookSensitivity = 3f;

    private PlayerMotor motor;


    private void Start()
    {
		//global = FindObjectOfType<Globals>();
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        float _xAxis = Input.GetAxisRaw("Horizontal");
        float _zAxis = Input.GetAxisRaw("Vertical");
        Vector3 movHorizontal = transform.right * _xAxis;
        Vector3 movVertical = transform.forward * _zAxis;

        Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

        motor.Move(velocity);

        // Calculate turning rotation as Vector3

        float _xRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _xRot, 0f) * global.lookSensitivity;

        // Apply

        motor.Rotate(_rotation);

        // Calculate camera rotation as Vector3

        float _yRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _tilt = new Vector3(_yRot, 0f, 0f) * global.lookSensitivity;

        // Apply

        motor.Tilt(_tilt, _yRot * (global.lookSensitivity / 2));
    }

    
    

}
