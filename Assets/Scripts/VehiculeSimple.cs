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

    public float motorTorque = 3000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 30;
    public float centreOfGravityOffset = -1f;

    private float originalMaxSpeed;
    private bool isSlowedDown = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelControl>();
        _rb.centerOfMass += Vector3.up * -6f;
        originalMaxSpeed = maxSpeed;
    }

    void OnMove(InputValue target)
    {
        dir = target.Get<Vector2>();
        dir = new Vector3(dir.x, 0f, dir.y);
    }

    void FixedUpdate()
    {
        float forwardSpeed = Vector3.Dot(transform.forward, _rb.velocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        float currentMotorTorque = Mathf.Lerp(motorTorque * 4f, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, Mathf.Pow(speedFactor, 0.5f));
        bool isAccelerating = Mathf.Sign(dir.z) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.wc.steerAngle = Mathf.Lerp(wheel.wc.steerAngle, dir.x * currentSteerRange, 0.8f);
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

            // Apply light braking when no acceleration input is given
            if (dir.z == 0)
            {
                wheel.wc.brakeTorque = brakeTorque * 0.2f;
            }

            // Increase forward friction to prevent excessive sliding
            WheelFrictionCurve forwardFriction = wheel.wc.forwardFriction;
            forwardFriction.stiffness = 8.0f;  // Increase grip
            wheel.wc.forwardFriction = forwardFriction;

            // Increase sideways friction to prevent drifting
            WheelFrictionCurve sidewaysFriction = wheel.wc.sidewaysFriction;
            sidewaysFriction.stiffness = 8.0f;  // Increase lateral grip
            wheel.wc.sidewaysFriction = sidewaysFriction;
        }

        if (isSlowedDown)
        {
            float currentSpeed = Mathf.Clamp(forwardSpeed, 0, maxSpeed);
            _rb.velocity = transform.forward * currentSpeed;
        }
    }

    public void SlowDown(float percentage, float duration)
    {
        isSlowedDown = true;
        maxSpeed = originalMaxSpeed * percentage;
        Debug.Log("Car slowed down to " + maxSpeed);
        Invoke("ResetSpeed", duration);
    }

    private void ResetSpeed()
    {
        isSlowedDown = false;
        maxSpeed = originalMaxSpeed;
        Debug.Log("Car speed restored to " + maxSpeed);
    }
}