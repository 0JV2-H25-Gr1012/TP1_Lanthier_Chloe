using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the car has the "Player" tag
        {
            CarControl carControl = other.GetComponent<CarControl>();
            if (carControl != null)
            {
                carControl.SetCheckpoint(transform);
            }
        }
    }
}