using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Event Scriptable")]
public class EventCardClass : ScriptableObject{
    #region Variables
    //Nombre de carte event tirable
    [SerializeField] private int _numberCarteEvent = 0;
    public int NumberCarteEvent => _numberCarteEvent;

    [SerializeField] private List<EventCard> _eventCardList = new List<EventCard>();
    public List<EventCard> EventCardList => _eventCardList;

    #endregion Variables

    public void PointeursLaserOptimisés()
    { }
}

[System.Serializable]
public class EventCard
{
    public string _eventName = "";
    [TextArea] public string _eventDescription = "";
    public MYthsAndSteel_Enum.EventCard _eventCardType = MYthsAndSteel_Enum.EventCard.Activation_de_nodus;
    public Sprite _eventSprite = null;
    public int _eventCost = 0;
}