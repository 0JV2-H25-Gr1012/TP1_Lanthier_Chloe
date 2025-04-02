using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
   [SerializeField] private Points _pointsScript;

    private void OnTriggerEnter(Collider other)
    {
        // VÃ©rifier si l'objet qui entre est bien la voiture
        if (other.CompareTag("Player")) 
        {
            if (_pointsScript.pointsJeu >= 60)
            {
                SceneManager.LoadScene("Victoire");
            }
            else
            {
                SceneManager.LoadScene("Defaite");
            }
        }
    }
}
