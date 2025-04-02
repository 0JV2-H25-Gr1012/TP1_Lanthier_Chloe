using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private CollecteurMonnaie _collecteur;
    [SerializeField] private TextMeshProUGUI _etiquettePoints;

    public int pointsJeu = 0;

    void Start()
    {
        pointsJeu = 0;
    }

    void Update()
    {
        // Actualiser la valeur du points du jeu
        pointsJeu = _collecteur.points;
        // Actualiser l'UI
        _etiquettePoints.text =   pointsJeu.ToString()+" Points";
    }
}
