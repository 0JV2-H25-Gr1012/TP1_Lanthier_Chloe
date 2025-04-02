using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class VehiculeSimple : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 dir;
    private WheelControl[] wheels;

    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;  // Original max speed
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    private float originalMaxSpeed; // Stores original speed
    private bool isSlowedDown = false; // Flag to track if car is slowed

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelControl>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        _rb.centerOfMass += Vector3.up * centreOfGravityOffset;

        originalMaxSpeed = maxSpeed; // Store initial max speed
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
        float currentMotorTorque = Mathf.Lerp(motorTorque * 2f, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(dir.z) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.wc.steerAngle = dir.x * currentSteerRange;
            }

            if (isAccelerating)
            {
                if (wheel.motorized)
                {
                    wheel.wc.motorTorque = dir.z * currentMotorTorque;
                }
                wheel.wc.brakeTorque = 0;
            }
            else
            {
                wheel.wc.brakeTorque = Mathf.Abs(dir.z) * brakeTorque;
                wheel.wc.motorTorque = 0;
            }
        }

        // If the car is slowed down, we may also want to limit the acceleration itself
        if (isSlowedDown)
        {
            // Cap the car's acceleration at a slower rate but still allow it to accelerate slowly
            float currentSpeed = Mathf.Clamp(forwardSpeed, 0, maxSpeed);
            _rb.velocity = transform.forward * currentSpeed;
        }
    }

    // Function to slow down the car
    public void SlowDown(float percentage, float duration)
    {
        isSlowedDown = true;
        maxSpeed = originalMaxSpeed * percentage; // Reduce speed
        Debug.Log("Car slowed down to " + maxSpeed);

        Invoke("ResetSpeed", duration); // Restore speed after duration
    }

    // Function to reset speed
    private void ResetSpeed()
    {
        isSlowedDown = false;
        maxSpeed = originalMaxSpeed;
        Debug.Log("Car speed restored to " + maxSpeed);
    }
}