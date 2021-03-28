using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInstance : MonoSingleton<UIInstance>{
    [Header("UI Manager")]
    //Avoir en référence l'UI Manager pour appeler des fonctions à l'intérieur
    [SerializeField] private UIManager _uiManager = null;
    public UIManager UiManager => _uiManager;

    //Le canvas en jeu pour afficher des menus
    [SerializeField] private GameObject _canvasTurnPhase = null;
    public GameObject CanvasTurnPhase => _canvasTurnPhase;

    [Header("Phase De Jeu")]
    //Le panneau à afficher lorsque l'on change de phase
    [SerializeField] private GameObject _switchPhaseObject = null;
    public GameObject SwitchPhaseObject => _switchPhaseObject;

    //Le canvas en jeu pour la phase d'activation
    [SerializeField] private GameObject _canvasActivation = null;
    public GameObject CanvasActivation => _canvasActivation;

    [Header("Cartes Événements")]
    //L'objet d'event à afficher lorsqu'une nouvelle carte event est piochée
    [SerializeField] private GameObject _eventCardObject = null;
    public GameObject EventCardObject => _eventCardObject;

    //Transform pour les cartes events du J1
    [SerializeField] private Transform _eventCardJ1Transform = null;
    public Transform EventCardJ1Transform => _eventCardJ1Transform;

    //Transform pour les cartes events du J2
    [SerializeField] private Transform _eventCardJ2Transform = null;
    public Transform EventCardJ2Transform => _eventCardJ2Transform;
}
