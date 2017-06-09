/*
* Copyright (c) B.J. Baye 2017 - with credit to Brackeys for tutorial!
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 tilt = Vector3.zero;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        PerformMovement();
        PerformRotation();
    }

    public void Move(Vector3 _velocityMove)
    {
        velocity = _velocityMove;
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
    
    public void Rotate(Vector3 _camRotation)
    { 
        rotation = _camRotation;
    }    

    void PerformRotation()
    {
       rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
       
       if (cam != null)
        {
            cam.transform.Rotate(-tilt);
        }
            
    }

    public void Tilt(Vector3 _camTilt)
    {
        tilt = _camTilt;
    }

    //void PerformTilt ()
    //{
    //    if (tilt != Vector3.zero)
    //    {
    //        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    //    }
    //}
}