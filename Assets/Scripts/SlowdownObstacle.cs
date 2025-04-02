using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownObstacle : MonoBehaviour
{
    public float slowPercentage = 0.2f; // 20% speed
    public float slowDuration = 3f; // Duration in seconds

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VehiculeSimple car = other.GetComponent<VehiculeSimple>();
            if (car != null)
            {
                Debug.Log("Car hit slowdown obstacle!");
                car.SlowDown(slowPercentage, slowDuration);
            }
        }
    }
}
