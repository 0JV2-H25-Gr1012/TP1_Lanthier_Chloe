using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{
    public Button flipButton; // Assign UI Button for flipping
    public Button resetButton; // Assign UI Button for resetting
    private Transform lastCheckpoint; // Stores the last checkpoint

    void Start()
    {
        if (flipButton != null)
            flipButton.onClick.AddListener(FlipCar);

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetCar);
    }

    public void FlipCar()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Set rotation upright while keeping Y rotation
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // Stop movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ResetCar()
    {
        if (lastCheckpoint != null) // Check if a checkpoint exists
        {
            transform.position = lastCheckpoint.position;
            transform.rotation = lastCheckpoint.rotation;
        }
        else
        {
            Debug.LogWarning("No checkpoint set! The car will not reset properly.");
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        lastCheckpoint = checkpoint; // Update last checkpoint
    }
}