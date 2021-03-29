using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlayerWait : MonoSingleton<ScriptPlayerWait>{
    //Carte Event du Joueur 1
    [SerializeField] private List<MYthsAndSteel_Enum.EventCard> _eventCardJ1 = new List<MYthsAndSteel_Enum.EventCard>();
    public List<MYthsAndSteel_Enum.EventCard> EventCardJ1 => _eventCardJ1;

    //Carte Event du Joueur 2
    [SerializeField] private List<MYthsAndSteel_Enum.EventCard> _eventCardJ2 = new List<MYthsAndSteel_Enum.EventCard>();
    public List<MYthsAndSteel_Enum.EventCard> EventCardJ2 => _eventCardJ2;

    /// <summary>
    /// Ajoute une carte event random au joueur
    /// </summary>
    /// <param name="player"></param>
    public void GiveEventCard(int player){
        List<MYthsAndSteel_Enum.EventCard> combineEventCards = new List<MYthsAndSteel_Enum.EventCard>();
        combineEventCards.AddRange(_eventCardJ1);
        combineEventCards.AddRange(_eventCardJ2);

        int randomCard = Random.Range(0, UIInstance.Instance.UiManager.EventCardClass.NumberCarteEvent + 1);
        Debug.Log((MYthsAndSteel_Enum.EventCard)randomCard);

        if(combineEventCards.Contains((MYthsAndSteel_Enum.EventCard)randomCard)){
            GiveEventCard(player);
            return;
        }

        AddEventCard(player, (MYthsAndSteel_Enum.EventCard)randomCard);
    }

    /// <summary>
    /// Ajoute une carte spécifique au joueur
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    public void AddEventCard(int player, MYthsAndSteel_Enum.EventCard card){
        if(player == 1){
            _eventCardJ1.Add(card);
        }
        else{
            _eventCardJ2.Add(card);
        }

        //Update l'UI
        UIInstance.Instance.UiManager.UpdateEventUI();
    }
}
