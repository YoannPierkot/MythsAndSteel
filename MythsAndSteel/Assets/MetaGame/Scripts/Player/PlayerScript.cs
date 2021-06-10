using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PlayerScript : MonoSingleton<PlayerScript>
{
    [SerializeField]
   List<MYthsAndSteel_Enum.EventCard> eventWithCostRessource;
    [SerializeField]
    GameObject prefabIconRessource; 
    [SerializeField] bool _ArmyRedWinAtTheEnd;
    public bool ArmyRedWinAtTheEnd => _ArmyRedWinAtTheEnd;

    [Header("STAT JOUEUR ROUGE")]
    [SerializeField] private Player _redPlayerInfos = new Player();
    public Player RedPlayerInfos => _redPlayerInfos;
    [Header("STAT JOUEUR BLEU")]
    [SerializeField] private Player _bluePlayerInfos = new Player();
    public Player BluePlayerInfos => _bluePlayerInfos;
    [Space]

    [SerializeField] private UnitReference _unitRef = null;
    public UnitReference UnitRef => _unitRef;
    [Space]

    //Liste des unités désactivées
    public List<MYthsAndSteel_Enum.TypeUnite> DisactivateUnitType = new List<MYthsAndSteel_Enum.TypeUnite>();

   
    [Header("Cartes events")]
    [SerializeField] private EventCardList _eventCardList = null;
    public EventCardList EventCardList => _eventCardList;

    public List<MYthsAndSteel_Enum.EventCard> _cardObtain = new List<MYthsAndSteel_Enum.EventCard>();

    [SerializeField] private GameObject RedAnim;
    [SerializeField] private GameObject BlueAnim;


    private void Start(){        
        EventCardList._eventSO.UpdateVisualUI(_eventCardList._eventGamBluePlayer, 2);
        EventCardList._eventSO.UpdateVisualUI(_eventCardList._eventGamRedPlayer, 1);
        RedPlayerInfos.UpdateOrgoneUI(1);
        BluePlayerInfos.UpdateOrgoneUI(2);
        

    }

    IEnumerator WaitEventRed(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        RedAnim.SetActive(false);
    }
    IEnumerator WaitEventBlue(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BlueAnim.SetActive(false);
    }

    [SerializeField] private GameObject UseResourcesAnimRed;
    [SerializeField] private GameObject UseResourcesAnimBlue;
    IEnumerator WaitResRed(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        UseResourcesAnimRed.SetActive(false);
    }
    IEnumerator WaitResBlue(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        UseResourcesAnimBlue.SetActive(false);
    }

    public void AnimRessource(int player)
    {
        
        if (player == 1)
        {
            UseResourcesAnimRed.SetActive(true);

            StartCoroutine(WaitResRed(1.5f));
        }

        else if (player == 2)
        {
            UseResourcesAnimBlue.SetActive(true);
        
            StartCoroutine(WaitResBlue(1.5f));
        }
    }

    #region DesactivationUnitType
    /// <summary>
    /// Désactive un type d'unité
    /// </summary>
    /// <param name="DesactiveUnit"></param>
    public void DesactivateUnitType(MYthsAndSteel_Enum.TypeUnite DesactiveUnit)
    {
        DisactivateUnitType.Add(DesactiveUnit);
    }

    /// <summary>
    /// active tous les types d'unités
    /// </summary>
    public void ActivateAllUnitType()
    {
        DisactivateUnitType.Clear();
    }
    #endregion DesactivationUnitType

    #region CarteEvent
    /// <summary>
    /// Ajoute une carte event random au joueur
    /// </summary>
    /// <param name="player"></param>
    public void GiveEventCard(int player)
    {
        if(_cardObtain.Count < EventCardList._eventSO.NumberOfEventCard){
            int randomCard = UnityEngine.Random.Range(0, EventCardList._eventSO.NumberOfEventCard);

            MYthsAndSteel_Enum.EventCard newCard = EventCardList._eventSO.EventCardList[randomCard]._eventType;

            if(_cardObtain.Contains(newCard))
            {
                GiveEventCard(player);
                return;
            }

            AddEventCard(player, newCard);
            _cardObtain.Add(newCard);
            if (player == 1){
                RedAnim.SetActive(true);
               StartCoroutine(WaitEventRed(0.8f));
           
            }
            else if (player == 2){
                BlueAnim.SetActive(true);
                StartCoroutine(WaitEventBlue(0.8f));
                 
            }
        }
        else{
            Debug.Log("Il n'y a plus de cartes events");
        }
    }

    /// <summary>
    /// Ajoute une carte spécifique au joueur
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    void AddEventCard(int player, MYthsAndSteel_Enum.EventCard card)
    {
        if(player == 1){
            EventCardList._eventCardRedPlayer.Insert(0, card);
        }
        else if(player == 2){
            EventCardList._eventCardBluePlayer.Insert(0, card);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }

        CreateEventCard(player, card);
    }

    /// <summary>
    /// Ajoute la carte event au canvas
    /// </summary>
    /// <param name="player"></param>
    /// <param name="card"></param>
    void CreateEventCard(int player, MYthsAndSteel_Enum.EventCard card){
        if (player == 1) GameManager.Instance.EventCardSO.ResetEventParentPos(1);
        else if (player == 2) GameManager.Instance.EventCardSO.ResetEventParentPos(2);
        GameObject newCard = Instantiate(player == 1? UIInstance.Instance.EventCardObjectRed : UIInstance.Instance.EventCardObjectBlue,
                                         player == 1 ? UIInstance.Instance.RedPlayerEventtransf.GetChild(0).transform.position : UIInstance.Instance.BluePlayerEventtransf.GetChild(0).transform.position,
                                         Quaternion.identity,
                                         player == 1 ? UIInstance.Instance.RedPlayerEventtransf.GetChild(0) : UIInstance.Instance.BluePlayerEventtransf.GetChild(0));

        EventCard newEventCard = new EventCard();
        foreach(EventCard evC in EventCardList._eventSO.EventCardList){
            if(evC._eventType == card){
                newEventCard = evC;
            }
        }
        newCard.GetComponent<EventCardContainer>().AddEvent(newEventCard);

        AddEventToButton(card, newCard);
       if(eventWithCostRessource.Contains(card))
        {
            

            Instantiate(prefabIconRessource, new Vector3(newCard.transform.transform.position.x+30, newCard.transform.transform.position.y-33, newCard.transform.transform.position.z),  Quaternion.identity, newCard.transform);

        }
        if(player == 1){
            EventCardList._eventGamRedPlayer.Insert(0, newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamRedPlayer, 1);
        }
        else if(player == 2){
            EventCardList._eventGamBluePlayer.Insert(0, newCard);
            _eventCardList._eventSO.UpdateVisualUI(EventCardList._eventGamBluePlayer, 2);
        }
        else{
            Debug.LogError("vous essayez d'ajouter une carte event a un joueur qui n'existe pas");
        }
    }

    public void AddEventToButton(MYthsAndSteel_Enum.EventCard card, GameObject cardGam){
        switch(card)
        {
            case MYthsAndSteel_Enum.EventCard.Activation_de_nodus:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchActivationDeNodus);
                break;

            case MYthsAndSteel_Enum.EventCard.Armes_perforantes:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchArmesPerforantes);
                break;

            case MYthsAndSteel_Enum.EventCard.Arme_épidémiologique:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchArmeEpidemiologique);
                break;

            case MYthsAndSteel_Enum.EventCard.Bombardement_aérien:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchBombardementAerien);
                break;

            case MYthsAndSteel_Enum.EventCard.Cessez_le_feu:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchCessezLeFeu);
                break;

            case MYthsAndSteel_Enum.EventCard.Déploiement_accéléré:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchDéploiementAccéléré);
                break;

            case MYthsAndSteel_Enum.EventCard.Vol_de_ravitaillement:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchVolDeRavitaillement);
                break;

            case MYthsAndSteel_Enum.EventCard.Détonation_d_orgone:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchDétonation_d_Orgone);
                break;

            case MYthsAndSteel_Enum.EventCard.Entraînement_rigoureux:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchEntrainementRigoureux);
                break;

            case MYthsAndSteel_Enum.EventCard.Fil_barbelé:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchFils_Barbelés);
                break;

            case MYthsAndSteel_Enum.EventCard.Illusion_stratégique:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchIllusionStratégique);
                break;

            case MYthsAndSteel_Enum.EventCard.Manoeuvre_stratégique:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchManoeuvreStratégique);
                break;

            case MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchOptimisationOrgone);
                break;

            case MYthsAndSteel_Enum.EventCard.Paralysie:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchParalysie);
                break;

            case MYthsAndSteel_Enum.EventCard.Pillage_orgone:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchPillageOrgone);
                break;

            case MYthsAndSteel_Enum.EventCard.Pointeurs_laser_optimisés:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchPointeursLaserOptimisés);
                break;

            case MYthsAndSteel_Enum.EventCard.Reprogrammation:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchReproggramation);
                break;

            case MYthsAndSteel_Enum.EventCard.Réapprovisionnement:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchReapprovisionnement);
                break;

            case MYthsAndSteel_Enum.EventCard.Sabotage:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchSabotage);
                break;

            case MYthsAndSteel_Enum.EventCard.Sérum_expérimental:
                cardGam.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(_eventCardList._eventSO.LaunchSerumExperimental);
                break;

            case MYthsAndSteel_Enum.EventCard.Transfusion_d_orgone:
                break;
        }
    }

    #endregion CarteEvent

    #region Orgone
    /// <summary>
    /// Quand un joueur gagne de l'orgone
    /// </summary>
    /// <param name="value"></param>
    /// <param name="player"></param>
    public void AddOrgone(int value, int player){

        if(player == 1){
            RedPlayerInfos.ChangeOrgone(value, player);
          
        }
        else{
     
        

            BluePlayerInfos.ChangeOrgone(value, player);
            
        }
    }

    /// <summary>
    /// Quand un joueur utilise de l'orgone
    /// </summary>
    /// <param name="value"></param>
    /// <param name="player"></param>
    public void UseOrgone(int value, int player){
        if(player == 1){
            RedPlayerInfos.ChangeOrgone(value, player);
        }
        else{
            BluePlayerInfos.ChangeOrgone(value, player);
        }
    }
    #endregion Orgone

    /// <summary>
    /// Est ce qu'il reste des unités dans l'armée du joueur
    /// </summary>
    /// <param name="Joueur"></param>
    /// <returns></returns>
    public bool CheckArmy(UnitScript unit, int Joueur){
        if(Joueur == 1){
            if(unit.UnitSO.IsInRedArmy){
                return true;
            }
            return false;
        }
        else{
            if(unit.UnitSO.IsInRedArmy){
                return false;
            }
            return true;
        }
    }

    public void ResetPlayerInfo(){
        if(GameManager.Instance.IsPlayerRedTurn){
            RedPlayerInfos.HasCreateUnit = false;
        }
        else{
            BluePlayerInfos.HasCreateUnit = false;
        }
    }
}

/// <summary>
/// Toutes les infos liées aux cartes events
/// </summary>
[System.Serializable]
public class EventCardList
{
    public EventCardClass _eventSO = null;

    //Carte Event du Joueur 1
    public List<MYthsAndSteel_Enum.EventCard> _eventCardRedPlayer = new List<MYthsAndSteel_Enum.EventCard>();

    //Carte Event du Joueur 2
    public List<MYthsAndSteel_Enum.EventCard> _eventCardBluePlayer = new List<MYthsAndSteel_Enum.EventCard>();

    //Carte Gam du Joueur 1
    public List<GameObject> _eventGamRedPlayer = null;

    //Carte Gam du Joueur 2
    public List<GameObject> _eventGamBluePlayer = null;
}
