using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollecteurMonnaie : MonoBehaviour
{
    public int points;


    private void Start()
    {
        points = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monnaie"))
        {
            Destroy(other.gameObject);
            points++;
        }
    }
}
