using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

// Code basée sur : https://docs.unity3d.com/2022.3/Documentation/Manual/WheelColliderTutorial.html

public class VehiculeSimple : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 dir;
    private WheelControl[] wheels;

    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelControl>();
        // Adjust center of mass vertically, to help prevent the car from rolling
        _rb.centerOfMass += Vector3.up * centreOfGravityOffset;
    }

    void OnMove(InputValue target)
    {
        dir = target.Get<Vector2>();
        dir = new Vector3(dir.x, 0f, dir.y);
    }

    void FixedUpdate()
    {
        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, _rb.velocity);


        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(dir.z) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.wc.steerAngle = dir.x * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.wc.motorTorque = dir.z * currentMotorTorque;
                }
                wheel.wc.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.wc.brakeTorque = Mathf.Abs(dir.z) * brakeTorque;
                wheel.wc.motorTorque = 0;
            }
        }
    }
}
