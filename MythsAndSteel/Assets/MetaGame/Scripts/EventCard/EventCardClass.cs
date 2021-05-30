using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "META/Event Scriptable")]
public class EventCardClass : ScriptableObject{
    //Nombre de cartes events

    [SerializeField] private int _numberOfEventCard = 0;
    public int NumberOfEventCard => _numberOfEventCard;

    //Liste des cartes events
    [SerializeField] private List<EventCard> _eventCardList = new List<EventCard>();
    public List<EventCard> EventCardList => _eventCardList;

    [SerializeField] private float _spaceBetweenTwoEvents = 0f;
    [SerializeField] private float multiplier = 0f;
    [SerializeField] private Vector2 _baseResolution = new Vector2(1920, 1080);

    int _redPlayerPos = 0;
    int _bluePlayerPos = 0;

    private void OnValidate(){
        int number = 0;
        foreach(EventCard card in _eventCardList){
            if(card._isEventInFinalGame){
                number++;
            }
        }

        _numberOfEventCard = number;
    }

    #region RemoveEvent
    /// <summary>
    /// Eneleve la carte event d'un joueur
    /// </summary>
    /// <param name="ev"></param>
    void RemoveEvents(MYthsAndSteel_Enum.EventCard ev){
        if(PlayerScript.Instance.EventCardList._eventCardRedPlayer.Contains(ev)){
            PlayerScript.Instance.EventCardList._eventCardRedPlayer.Remove(ev);
            
            foreach(GameObject gam in PlayerScript.Instance.EventCardList._eventGamRedPlayer){
                if(gam.GetComponent<EventCardContainer>().EventCardInfo._eventType == ev){
                    RemoveEventGam(gam, 1);
                    break;
                }
            }
        }
        else{
            PlayerScript.Instance.EventCardList._eventCardBluePlayer.Remove(ev);

            foreach(GameObject gam in PlayerScript.Instance.EventCardList._eventGamBluePlayer){
                if(gam.GetComponent<EventCardContainer>().EventCardInfo._eventType == ev){
                    RemoveEventGam(gam, 2);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// détruit une carte event
    /// </summary>
    /// <param name="gam"></param>
    void RemoveEventGam(GameObject gam, int player){
        if(player == 1){
            PlayerScript.Instance.EventCardList._eventGamRedPlayer.Remove(gam);
            Destroy(gam);
           GameManager.Instance.victoryScreen.redEventUsed += 1;
        }
        else if(player == 2){
            PlayerScript.Instance.EventCardList._eventGamBluePlayer.Remove(gam);
            Destroy(gam);
          GameManager.Instance.victoryScreen.blueEventUsed += 1;
        }
        else{
            Debug.LogError("Vous essayez d'enlever une carte event a un joueur qui n'existe pas");
        }

        UpdateVisualUI(PlayerScript.Instance.EventCardList._eventGamRedPlayer, 1);
        UpdateVisualUI(PlayerScript.Instance.EventCardList._eventGamBluePlayer, 2);
    }
    #endregion RemoveEvent

    #region UpdateEventUI
    /// <summary>
    /// Met a jour la position des cartes events dans l'interface
    /// </summary>
    /// <param name="gam"></param>
    public void UpdateVisualUI(List<GameObject> gam, int player){
        if(player == 1){
            if(PlayerScript.Instance.EventCardList._eventCardRedPlayer.Count <= 3){
                ResetEventParentPos(1);

                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, false, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos, player);
            }
            else{
                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos, player);
            }
        }
        else if(player == 2)
        {
            if(PlayerScript.Instance.EventCardList._eventCardBluePlayer.Count <= 3){
                ResetEventParentPos(2);

                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, false, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos, player);
            }
            else{
                UpdateEventList(gam, player);
                UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos, player);
            }
        }
        else{
            Debug.LogError("Vous essayez d'd'update l'ui d'un joueur qui n'existe pas");
        }
    }
    
    /// <summary>
    /// Update la position des events dans la liste
    /// </summary>
    /// <param name="gam"></param>
    /// <param name="player"></param>
    void UpdateEventList(List<GameObject> gam, int player)
    {
        if(player == 1){
            if(_redPlayerPos == 0){
                //Déplace les events à leurs bonnes positions
                gam[0].transform.position = UIInstance.Instance.RedEventDowntrans.position;

                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y - _spaceBetweenTwoEvents * (Screen.height / _baseResolution.y),
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
            else{
                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y - _spaceBetweenTwoEvents * (Screen.height / _baseResolution.y),
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
        }

        else if(player == 2){
            if(_bluePlayerPos == 0){
                //Déplace les events à leurs bonnes positions
                gam[0].transform.position = UIInstance.Instance.BlueEventDowntrans.position;

                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){ 
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y - _spaceBetweenTwoEvents * (Screen.height / _baseResolution.y),
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
            else{
                if(gam.Count > 1){
                    for(int i = 1; i < gam.Count; i++){
                        gam[i].transform.position = new Vector3(gam[i - 1].transform.position.x,
                                                                gam[i - 1].transform.position.y - _spaceBetweenTwoEvents * (Screen.height / _baseResolution.y),
                                                                gam[i - 1].transform.position.z);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Met a jour le visuel des boutons et leur interaction
    /// </summary>
    /// <param name="upButton"></param>
    /// <param name="downButton"></param>
    /// <param name="active"></param>
    void UpdateButtonPlayer(GameObject upButton, GameObject downButton, bool active, int numberOfCard, int pos, int player){
        downButton.GetComponent<Image>().sprite = active ? (pos == numberOfCard - 3?  UIInstance.Instance.FlecheSpriteRef._grisArrowDown : (player == 1 ? UIInstance.Instance.FlecheSpriteRef._redArrowDown : UIInstance.Instance.FlecheSpriteRef._blueArrowDown)) : UIInstance.Instance.FlecheSpriteRef._grisArrowDown;
        downButton.GetComponent<Button>().interactable = pos == numberOfCard - 3 ? false : active;
        upButton.GetComponent<Image>().sprite = active ? (pos == 0 ? UIInstance.Instance.FlecheSpriteRef._grisArrowUp : (player == 1 ? UIInstance.Instance.FlecheSpriteRef._redArrowUp : UIInstance.Instance.FlecheSpriteRef._blueArrowUp)) : UIInstance.Instance.FlecheSpriteRef._grisArrowUp;
        upButton.GetComponent<Button>().interactable = pos == 0 ? false : active;
    }

    /// <summary>
    /// Update la position du parent des cartes events d'un joueur
    /// </summary>
    void UpdateEventsParentPos(int player)
    {
        if(player == 1)
        {
            UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition = new Vector3(UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition.x, +_spaceBetweenTwoEvents * _redPlayerPos / multiplier, UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition.z);
        }
        else if(player == 2)
        {
            UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition = new Vector3(UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition.x, +_spaceBetweenTwoEvents * _bluePlayerPos / multiplier, UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition.z);
        }
        else
        {
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }

    /// <summary>
    /// Reset la position du parent des cartes events d'un joueur
    /// </summary>
    /// <param name="player"></param>
 public   void ResetEventParentPos(int player)
    {
        if(player == 1)
        {
            _redPlayerPos = 0;
            UIInstance.Instance.RedPlayerEventtransf.GetChild(0).localPosition = Vector3.zero;
        }
        else if(player == 2)
        {
            _bluePlayerPos = 0;
            UIInstance.Instance.BluePlayerEventtransf.GetChild(0).localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }
    #endregion UpdateEventUI

    #region ButtonInput
    /// <summary>
    /// Quand le joueur appuie sur le bouton pour monter dans la liste des cartes events
    /// </summary>
    /// <param name="player"></param>
    public void GoUp(int player){
        if(player == 1){
            _redPlayerPos--;
            UpdateEventsParentPos(1);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos, player);
        }
        else if(player == 2){
            _bluePlayerPos--;
            UpdateEventsParentPos(2);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos, player);
        }
        else{
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }

    /// <summary>
    /// Quand le joueur appuie pour descendre dans la liste des cartes events
    /// </summary>
    /// <param name="player"></param>
    public void GoDown(int player){
        if(player == 1){
            _redPlayerPos++;
            UpdateEventsParentPos(1);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventRedPlayer._upButton, UIInstance.Instance.ButtonEventRedPlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamRedPlayer.Count, _redPlayerPos, player);
        }
        else if(player == 2){
            _bluePlayerPos++;
            UpdateEventsParentPos(2);
            UpdateButtonPlayer(UIInstance.Instance.ButtonEventBluePlayer._upButton, UIInstance.Instance.ButtonEventBluePlayer._downButton, true, PlayerScript.Instance.EventCardList._eventGamBluePlayer.Count, _bluePlayerPos, player);
        }
        else{
            Debug.LogError("vous essayez de déplacer les cartes events d'un joueur qui n'existe pas");
        }
    }
    #endregion ButtonInput

    #region Evenement

    //A finir dès qu'on peut ajouter une unité
    #region Déploiement Accéléré
    public void DéploiementAccéléré()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject tile in GameManager.Instance.TileChooseList){
            Debug.Log("Le jeu ajoute une infante");
        }

        GameManager.Instance.TileChooseList.Clear();
        RemovePlayerRessource(MYthsAndSteel_Enum.EventCard.Déploiement_accéléré);
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Déploiement_accéléré);
        EventCardUse();
    }

    public void LaunchDéploiementAccéléré(){
        UIInstance.Instance.ActivateNextPhaseButton();

        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Déploiement_accéléré);
        List<GameObject> gamList = new List<GameObject>();
        
        //obtien les cases voisines pour chaque unité de l'armée
        foreach(GameObject unit in player == 1? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer){
            List<int> neighTile = PlayerStatic.GetNeighbourDiag(unit.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[unit.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);

            //Check les effets de terrain pour voir si il doit ajouter la tile à la liste
            foreach (int i in neighTile)
            {
                //Obtient la direction de la case par rapport à l'unité
                MYthsAndSteel_Enum.Direction dir = PlayerStatic.CheckDirection(unit.GetComponent<UnitScript>().ActualTiledId, i);
                if (PlayerScript.Instance.RedPlayerInfos.Ressource >= 1 && player == 1 || PlayerScript.Instance.BluePlayerInfos.Ressource >= 1 && player == 2)
                {


                    if (TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Ravin) || TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Eau))
                    {
                        //La tile n'est pas ajoutée
                    }
                    else
                    {
                        switch (dir)
                        {
                            case MYthsAndSteel_Enum.Direction.Nord:
                                if (!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Sud))
                                {
                                    gamList.Add(TilesManager.Instance.TileList[i]);
                                }
                                break;
                            case MYthsAndSteel_Enum.Direction.Sud:
                                if (!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Nord))
                                {
                                    gamList.Add(TilesManager.Instance.TileList[i]);
                                }
                                break;
                            case MYthsAndSteel_Enum.Direction.Est:
                                if (!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest))
                                {
                                    gamList.Add(TilesManager.Instance.TileList[i]);
                                }
                                break;
                            case MYthsAndSteel_Enum.Direction.Ouest:
                                if (!TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Est))
                                {
                                    gamList.Add(TilesManager.Instance.TileList[i]);
                                }
                                break;
                        }
                    }
                    
                }
            }
        }

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 && Attaque.Instance.IsInAttack|| GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) && 
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0))){
            RenfortPhase.Instance.craftUnit(1);
            LaunchEventTile(1, player == 1 ? true : false, gamList, "Déploiement accéléré", "Êtes-vous sur de vouloir créer une unité d'infanterie sur cette case?", false);
            GameManager.Instance._eventCall += DéploiementAccéléré;
        }
    }
    #endregion Reprogrammation

    #region IllusionStratégique
    public void IllusionStratégique()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int firstTileId = GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId;

        TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[1].GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0].gameObject);
        TilesManager.Instance.TileList[firstTileId].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[1].gameObject);

        GameManager.Instance.UnitChooseList.Clear();
        GameManager.Instance.IllusionStratégique = false;

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Illusion_stratégique);
        EventCardUse();
    }

    public void LaunchIllusionStratégique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Illusion_stratégique);
        List<GameObject> unitList = new List<GameObject>();

        if(player == 1){
            foreach(GameObject gam in PlayerScript.Instance.UnitRef.UnitListRedPlayer)
            {
                if(gam.GetComponent<UnitScript>().IsActivationDone == true)
                {
                    unitList.Add(gam);
                }
            }

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
        }
        else{
            foreach(GameObject gam in PlayerScript.Instance.UnitRef.UnitListBluePlayer)
            {
                if(gam.GetComponent<UnitScript>().IsActivationDone == true)
                {
                    unitList.Add(gam);
                }
            }

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);
        }


        GameManager.Instance.IllusionStratégique = true;

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList, "Illusion Stratégique", "Êtes-vous sur de vouloir échanger la position de ces deux unités sur le plateau?");
            GameManager.Instance._eventCall += IllusionStratégique;
        }
        unitList.Clear();
      
    }
    #endregion IllusionStratégique

    #region OptimisationOrgone
    public void OptimisationOrgone()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone);
        if(player == 1){
            PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft++;
        }
        else{
            PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft++;
        }

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone);
        EventCardUse();
    }

    public void LaunchOptimisationOrgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Optimisation_de_l_orgone);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
                GameManager.Instance._eventCall += OptimisationOrgone;
            if (PlayerPrefs.GetInt("Avertissement") == 0)
            {

                GameManager.Instance._eventCall();
            }

            UIInstance.Instance.ShowValidationPanel("Optimisation de l'orgone", "Êtes-vous sur de vouloir augmenter votre nombre d'utilisation de charge d'orgones de ");
        }
    }
    #endregion OptimisationOrgone

    #region PillageOrgone
    public void PillageOrgone()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Pillage_orgone);

        foreach(GameObject gam in GameManager.Instance.TileChooseList){
            gam.GetComponent<TileScript>().RemoveRessources(1, player);
        }

        GameManager.Instance.TileChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Pillage_orgone);
        EventCardUse();
    }

    public void LaunchPillageOrgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Pillage_orgone);
        
        List<GameObject> tileList = new List<GameObject>();
        tileList.AddRange(TilesManager.Instance.ResourcesList);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventTile(2, player == 1 ? true : false, tileList, "Pillage d'orgone", "Êtes-vous sur de vouloir voler deux Ressources sur ces cases?", true);
            GameManager.Instance._eventCall += PillageOrgone;
        }

        tileList.Clear();
    }

    #endregion PillageOrgone
    #region Fils Barbelés
    public void Fils_Barbelés()
    {
        UIInstance.Instance.ActivateNextPhaseButton();


foreach (GameObject element in TilesManager.Instance.TileList)
        {
         
            element.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);

        }
    MYthsAndSteel_Enum.Direction  barbelposition =  PlayerStatic.CheckDirection(GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId, GameManager.Instance.TileChooseList[1].GetComponent<TileScript>().TileId);
        BarbelGestion.Instance.CreateBarbel(GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().TileId, barbelposition);

        RemovePlayerRessource(MYthsAndSteel_Enum.EventCard.Fil_barbelé);
        //Remove la carte event chez le bon joueur
        GameManager.Instance.TileChooseList.Clear();
        GameManager.Instance.filBbarbelés = false;
        barbelposition =MYthsAndSteel_Enum.Direction.None;
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Fil_barbelé);
        EventCardUse();
    }

    public void LaunchFils_Barbelés()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Fil_barbelé);

        List<GameObject> tileList = new List<GameObject>();
        tileList.AddRange(TilesManager.Instance.TileList);

        if ((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            if (PlayerScript.Instance.RedPlayerInfos.Ressource >= 1 && player == 1 || PlayerScript.Instance.BluePlayerInfos.Ressource >= 1 && player == 2)
            {
                LaunchEventTile(2, player == 1 ? true : false, tileList, "Fils Barbelés", "Êtes-vous sur de vouloir faire apparaitre un barbelé entre deux cases?", true);
                GameManager.Instance.filBbarbelés = true;
                GameManager.Instance._eventCall += Fils_Barbelés;
            }
        }

        tileList.Clear();
    }
    #endregion

    #region Détonation d'Orgone
    public void Détonation_d_Orgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Détonation_d_orgone);
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach  (GameObject element in GameManager.Instance.TileChooseList)
        {
         GameObject détonation = Instantiate(GameManager.Instance.détonationPrefab, element.transform);
            if (player == 1) détonation.GetComponent<Détonation>()._IsInRedArmy = true;
            else if (player == 2) détonation.GetComponent<Détonation>()._IsInRedArmy = false;

            détonation.GetComponent<Détonation>().TileID = element.GetComponent<TileScript>().TileId;
            element.GetComponent<TileScript>().AddEffectToList(MYthsAndSteel_Enum.TerrainType.Détonation);
            element.GetComponent<TileScript>()._Child.Add(détonation);
        }
      

       

        RemovePlayerRessource(MYthsAndSteel_Enum.EventCard.Détonation_d_orgone);
        //Remove la carte event chez le bon joueur
        GameManager.Instance.TileChooseList.Clear();
   
      
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Détonation_d_orgone);
        EventCardUse();
    }

    public void LaunchDétonation_d_Orgone()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Détonation_d_orgone);

        List<GameObject> tileList = new List<GameObject>();
        tileList.AddRange(TilesManager.Instance.TileList);

        if ((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            if (PlayerScript.Instance.RedPlayerInfos.Ressource >= 1 && player == 1 || PlayerScript.Instance.BluePlayerInfos.Ressource >= 1 && player == 2)
            {
                LaunchEventTile(3, player == 1 ? true : false, tileList, "Détonation d'orgone", "Êtes-vous sur de vouloir faire faire apparaitre trois détonations d'orgone?", true);
             
                GameManager.Instance._eventCall += Détonation_d_Orgone;
            }
        }

        tileList.Clear();
    }
    #endregion

    #region PointeursLaser
    public void PointeursLaserOptimisés()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AttackRangeBonus += 1;
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Pointeurs_laser_optimisés);
        EventCardUse();
    }

    public void LaunchPointeursLaserOptimisés()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Pointeurs_laser_optimisés);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 2 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList, "Pointeurs Laser Optimisé", "Êtes-vous sur de vouloir augmenter de 1 la portée de ces 2 unités?");
            GameManager.Instance._eventCall += PointeursLaserOptimisés;
        }

        unitList.Clear();
    }
    #endregion PointeursLaser

    #region ArmeEpidemiologique
    public void ArmeEpidemiologique()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        GameManager.Instance.armeEpidemelogiqueStat = DeterminArmy(MYthsAndSteel_Enum.EventCard.Arme_épidémiologique);

        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.ArmeEpidemiologique);

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Arme_épidémiologique);
        EventCardUse
            ();
    }

    public void LaunchArmeEpidemiologique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Arme_épidémiologique);

        List<GameObject> unitList = new List<GameObject>();
        unitList.AddRange(player == 2? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventUnit(1, player == 1 ? true : false, unitList, "Arme épidémiologique", "Êtes-vous sur de vouloir Infliger l'effet à cette unité? Toutes unités adjacentes à cette unité se verra perdre un point de vie à la fin du tour adverse ainsi que les unités adjacentes.");
            GameManager.Instance._eventCall += ArmeEpidemiologique;
        }

        unitList.Clear();
    }
    #endregion ArmeEpidemiologique

    #region ManeouvreStratégique
    public void ManoeuvreStratégique()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Manoeuvre_stratégique);
        
        if(player == 1){
            PlayerScript.Instance.RedPlayerInfos.ActivationLeft++;
            UIInstance.Instance.UpdateActivationLeft();
        }
        else{
            PlayerScript.Instance.BluePlayerInfos.ActivationLeft++;
            UIInstance.Instance.UpdateActivationLeft();
        }

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Manoeuvre_stratégique);
        EventCardUse();
    }

    public void LaunchManoeuvreStratégique()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Manoeuvre_stratégique);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            GameManager.Instance._eventCall += ManoeuvreStratégique;
            if(PlayerPrefs.GetInt("Avertissement") == 0)
            {
                GameManager.Instance._eventCall();
            }
            UIInstance.Instance.ShowValidationPanel("Manoeuvre stratégique", "Êtes-vous sur de vouloir activer une unité supplémentaire durant ce tour?");
        }
    }
    #endregion ManeouvreStratégique

    #region SerumExpérimental
    public void SerumExperimental()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().MoveSpeedBonus++;
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Sérum_expérimental);
        EventCardUse();
    }

    public void LaunchSerumExperimental()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Sérum_expérimental);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 2 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList, "Sérum Expérimental", "Êtes-vous sur de vouloir augmenter d'1 point le déplacement de ces deux unités?");
            GameManager.Instance._eventCall += SerumExperimental;
        }

        unitList.Clear();
    }
    #endregion SerumExpérimental

    #region ActivationDeNodus
    public void ActivationDeNodus()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Activation_de_nodus);
        if(player == 1){
            PlayerScript.Instance.RedPlayerInfos.OrgonePowerLeft++;
        }
        else{
            PlayerScript.Instance.BluePlayerInfos.OrgonePowerLeft++;
        }

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Activation_de_nodus);
        EventCardUse();
    }

    public void LaunchActivationDeNodus()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Activation_de_nodus);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            GameManager.Instance._eventCall += ActivationDeNodus;
            if (PlayerPrefs.GetInt("Avertissement") == 0)
            {
                GameManager.Instance._eventCall();
            }
            UIInstance.Instance.ShowValidationPanel("Activation de Nodus", "Êtes-vous sur de vouloir utiliser un pouvoir orgonique durant votre phase d'action?");
        }
    }
    #endregion ActivationDeNodus

    //Check si l'unité peut aller sur la case sinon faire un dégât à l'unité
    #region BombardementAerien
    public void BombardementAerien()
    {
        LaunchDeplacementBombardement(GameManager.Instance.UnitChooseList[0]);
        GameManager.Instance._eventCall -= BombardementAerien;
        GameManager.Instance._eventCall += MakeDamageBombardement;
    }

    public void MakeDamageBombardement(){
        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().TakeDamage(1);
        GameManager.Instance._eventCall -= MakeDamageBombardement;

        UIInstance.Instance.ActivateNextPhaseButton();
    }

    public void MoveUnitBombardement(){
        GameManager.Instance._eventCall = null;

        while(GameManager.Instance.UnitChooseList[0].transform.position != GameManager.Instance.TileChooseList[0].transform.position){
            GameManager.Instance.UnitChooseList[0].transform.position = Vector3.MoveTowards(GameManager.Instance.UnitChooseList[0].transform.position, GameManager.Instance.TileChooseList[0].transform.position, .7f);
            GameManager.Instance._waitEvent -= MoveUnitBombardement;
            GameManager.Instance._waitEvent += MoveUnitBombardement;
            GameManager.Instance.WaitToMove(.025f);
            return;
        }

        GameManager.Instance._waitEvent -= MoveUnitBombardement;
        TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().RemoveUnitFromTile();
        GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0]);

        GameManager.Instance.TileChooseList.Clear();
        GameManager.Instance.UnitChooseList.Clear();

        RemoveEvents(MYthsAndSteel_Enum.EventCard.Bombardement_aérien);
        EventCardUse();
    }

    public void LaunchBombardementAerien()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Bombardement_aérien);
        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 1 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventUnit(1, player == 1 ? true : false, unitList, "Bombardement Aérien", "Êtes-vous sur de vouloir infliger des dégâts à cette unité?");
            GameManager.Instance._eventCall += BombardementAerien;
        }

        unitList.Clear();
    }

    void LaunchDeplacementBombardement(GameObject unit){
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Bombardement_aérien);
        List<GameObject> tileList = new List<GameObject>();

        List<int> unitNeigh = PlayerStatic.GetNeighbourDiag(unit.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[unit.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);
        foreach(int i in unitNeigh){
            tileList.Add(TilesManager.Instance.TileList[i]);
        }

        LaunchEventTile(1, player == 1 ? true : false, tileList, "Bombardement Aérien", "Êtes-vous sur de vouloir déplacer l'unité attaquée sur cette case?", false) ;
        GameManager.Instance._eventCall += MoveUnitBombardement;

        tileList.Clear();
    }
    #endregion Reprogrammation

    #region Reprogrammation
    public void Reproggramation(){
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList){
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Possédé);
            unit.GetComponent<UnitScript>().AddDiceToUnit(-4);
        }
        GameManager.Instance.possesion = true;
        GameManager.Instance.UnitChooseList.Clear();
        RemovePlayerRessource(MYthsAndSteel_Enum.EventCard.Reprogrammation);
        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Reprogrammation);
        EventCardUse();
    }

    public void LaunchReproggramation(){
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Reprogrammation);
        List<GameObject> unitList = new List<GameObject>();

       
        foreach(GameObject T in player == 1 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer)
        {
            if(!T.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé))
            {
                unitList.Add(T);
            }
        }
        if ((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            if(PlayerScript.Instance.RedPlayerInfos.Ressource >= 1 && player == 1 || PlayerScript.Instance.BluePlayerInfos.Ressource >= 1 && player == 2)
            {

            LaunchEventUnit(1, player == 1? true : false, unitList, "Reproggramation", "Êtes-vous sur de vouloir activer cette unité adverse durant ce tour?");
            GameManager.Instance._eventCall += Reproggramation;
            }
        }

        unitList.Clear();
    }
    #endregion Reprogrammation

    #region CessezLeFeu
    public void CessezLeFeu()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre);
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Invincible);
            unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs);
        }

        GameManager.Instance.UnitChooseList.Clear();
        RemovePlayerRessource(MYthsAndSteel_Enum.EventCard.Cessez_le_feu);
        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Cessez_le_feu);
        EventCardUse();
    }

    public void LaunchCessezLeFeu()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Cessez_le_feu);
        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))

        {
            if (PlayerScript.Instance.RedPlayerInfos.Ressource >= 1 && player == 1 || PlayerScript.Instance.BluePlayerInfos.Ressource >= 1 && player == 2)
            {

                LaunchEventUnit(1, player == 1 ? true : false, unitList, "Cessez le feu!", "Êtes-vous sur de vouloir empêcher cette unité de prendre des dégâts, capturer un objectif et d'attaquer durant ce tour?");
            GameManager.Instance._eventCall += CessezLeFeu;
            }
        }

        unitList.Clear();
    }

    #endregion Reprogrammation

    #region Vol de Ravitaillement

    public void VolDeRavitaillement()
    {
     
        UIInstance.Instance.ActivateNextPhaseButton();
        if(GameManager.Instance.IsPlayerRedTurn)
        {

        GameManager.Instance.VolDeRavitaillementStat = 1;
        }
        else
        {
            GameManager.Instance.VolDeRavitaillementStat = 2;
        }
      
        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Vol_de_ravitaillement);
        EventCardUse();

    }
        
   public void LaunchVolDeRavitaillement()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Vol_de_ravitaillement);

       if ((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
         

         
            GameManager.Instance._eventCall += VolDeRavitaillement;

            if (PlayerPrefs.GetInt("Avertissement") == 0)
            {
               GameManager.Instance._eventCall();

            }
            UIInstance.Instance.ShowValidationPanel("Vol de Ravitaillement", "Êtes-vous sur de vouloir de voler les ressources récupérées par votre adversaire pendant ce tour?");
        }
    }
    #endregion
    #region Sabotage
    public void Sabotage()
    {
        UIInstance.Instance.ActivateNextPhaseButton();
        if (GameManager.Instance.IsPlayerRedTurn)
        {

            GameManager.Instance.SabotageStat = 1;
        }
        else
        {
            GameManager.Instance.SabotageStat = 2;
        }

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Sabotage);
        EventCardUse();
    }
    public void LaunchSabotage()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Sabotage);
        if ((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
      ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            GameManager.Instance._eventCall += Sabotage;
            if (PlayerPrefs.GetInt("Avertissement") == 0)
            {
                GameManager.Instance._eventCall();

            }
            UIInstance.Instance.ShowValidationPanel("Sabotage", "Êtes-vous sur de vouloir de réduire la valeur d'activation de votre adversaire lors de sa prochaine phase d'action?");
        }
    }
    #endregion
    #region Paralysie
    public void Paralysie()
    {
        List<GameObject> unitList = new List<GameObject>();
        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);
        UIInstance.Instance.ActivateNextPhaseButton();
        MYthsAndSteel_Enum.TypeUnite typeUnitChoose = GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().UnitSO.typeUnite;
        foreach (GameObject unit in unitList)
        {
            if(unit.GetComponent<UnitScript>().UnitSO.typeUnite == typeUnitChoose)
            {
                unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Paralysie);
            }
        if (GameManager.Instance.IsPlayerRedTurn)
        {

            unit.GetComponent<UnitScript>().ParalysieStat = 1;
        }
        else
        {
           unit.GetComponent<UnitScript>().ParalysieStat = 2;
        }
        }
        RemovePlayerRessource(MYthsAndSteel_Enum.EventCard.Paralysie);
        unitList.Clear();
        typeUnitChoose = MYthsAndSteel_Enum.TypeUnite.Autre;
        GameManager.Instance.UnitChooseList.Clear();
        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Paralysie);
        EventCardUse();
    }
    public void LaunchParalysie()
    {
        List<GameObject> unitList = new List<GameObject>();

        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Paralysie);
        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);
        unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);
        if ((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2) &&
      ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            if ((player == 1 && PlayerScript.Instance.RedPlayerInfos.Ressource > 0) || (player == 2 && PlayerScript.Instance.BluePlayerInfos.Ressource > 0))
            {
              

            GameManager.Instance._eventCall += Paralysie;
               

                LaunchEventUnit(1, player == 1 ? true : false, unitList, "Paralysie", "Êtes-vous sur de vouloir empecher l'activation des unités ayant le même type que celle séléctionnée?");
               
            }
        
            }
        unitList.Clear();
    }

    #endregion
    #region Réapprovisionnement
    public void Reapprovisionnement()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
         
            

            unit.GetComponent<UnitScript>().GiveLife(1);
            
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Réapprovisionnement);
        EventCardUse();
    }

    public void LaunchReapprovisionnement()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Réapprovisionnement);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 2 ? PlayerScript.Instance.UnitRef.UnitListBluePlayer : PlayerScript.Instance.UnitRef.UnitListRedPlayer);

        if ((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0)))
        {
        
                
            
   

            LaunchEventUnit(2, player == 1 ? true : false, unitList, "Réapprovisionnement", "Êtes-vous sur de vouloir soigner ces deux unités de 1 point de vie?");
            GameManager.Instance._eventCall += Reapprovisionnement;

            
        }

        unitList.Clear();
    }
    #endregion Réapprovisionnement

    #region ArmesPerforantes
    public void ArmesPerforantes()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AddDamageToUnit(1);
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Armes_perforantes);
        EventCardUse();
    }

    public void LaunchArmesPerforantes()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Armes_perforantes);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 1 ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn) && PlayerScript.Instance.RedPlayerInfos.EventUseLeft > 0 || (player == 2 && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventUnit(2, player == 1 ? true : false, unitList, "Armes perforantes", "Êtes-vous sur de vouloir augmenter d'1 les dégâts de ces deux unités?");
            GameManager.Instance._eventCall += ArmesPerforantes;

        }

        unitList.Clear();
    }
    #endregion ArmesPerforantes

    #region EntrainementRigoureux
    public void EntrainementRigoureux()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach(GameObject unit in GameManager.Instance.UnitChooseList)
        {
            unit.GetComponent<UnitScript>().AddDiceToUnit(3);
        }

        GameManager.Instance.UnitChooseList.Clear();

        //Remove la carte event chez le bon joueur
        RemoveEvents(MYthsAndSteel_Enum.EventCard.Entraînement_rigoureux);
        EventCardUse();
    }

    public void LaunchEntrainementRigoureux()
    {
        int player = DeterminArmy(MYthsAndSteel_Enum.EventCard.Entraînement_rigoureux);

        List<GameObject> unitList = new List<GameObject>();

        unitList.AddRange(player == 1 ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer);

        if((GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 || GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2) &&
            ((player == 1 && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.EventUseLeft>0) || (player == 2 && !GameManager.Instance.IsPlayerRedTurn &&  PlayerScript.Instance.BluePlayerInfos.EventUseLeft > 0)))
        {
            LaunchEventUnit(1, player == 1 ? true : false, unitList, "Entraînement rigoureux", "Êtes-vous sur de vouloir donner un bonus de 3 aux chances d'attaques de cette unité?");
            GameManager.Instance._eventCall += EntrainementRigoureux;
        }

        unitList.Clear();
    }
    #endregion EntrainementRigoureux

    /// <summary>
    /// Lance un événement en appellant la fonction dans le gameManager pour choisir une unité
    /// </summary>
    /// <param name="unitNumber"></param>
    /// <param name="opponent"></param>
    /// <param name="army"></param>
    /// <param name="redPlayer"></param>
    void LaunchEventUnit(int unitNumber, bool redPlayer, List<GameObject> unitList, string titleValidation, string descriptionValidation)
    {
        GameManager.Instance.StartEventModeUnit(unitNumber, redPlayer, unitList, titleValidation, descriptionValidation) ;
    }

    /// <summary>
    /// Lance un événement en appellant la fonction dans le gameManager pour choisir une ou plusieurs cases du plateau
    /// </summary>
    /// <param name="numberOfTiles"></param>
    /// <param name="redPlayer"></param>
    /// <param name="gamList"></param>
    void LaunchEventTile(int numberOfTiles, bool redPlayer, List<GameObject> gamList, string titleValidation, string descriptionValidation, bool multiple){
        GameManager.Instance.StartEventModeTiles(numberOfTiles, redPlayer, gamList, titleValidation, descriptionValidation, multiple);
    }

    /// <summary>
    /// Determine à quelle armée appartient la carte
    /// </summary>
    int DeterminArmy(MYthsAndSteel_Enum.EventCard cardToFind){
        foreach(MYthsAndSteel_Enum.EventCard card in PlayerScript.Instance.EventCardList._eventCardRedPlayer)
        {
            if(card == cardToFind)
            {
                return 1;
            }
        }

        foreach(MYthsAndSteel_Enum.EventCard card in PlayerScript.Instance.EventCardList._eventCardBluePlayer)
        {
            if(card == cardToFind)
            {
                return 2;
            }
        }

        return 0;
    }
    #endregion Evenement
    void EventCardUse()
    {
        if (GameManager.Instance.IsPlayerRedTurn)
        {
            PlayerScript.Instance.RedPlayerInfos.EventUseLeft --;
        }
        else
        {
            PlayerScript.Instance.BluePlayerInfos.EventUseLeft --;
        }
    }
    void RemovePlayerRessource(MYthsAndSteel_Enum.EventCard eventCardEnum)
{
        if (DeterminArmy(eventCardEnum) == 1)
        {

            PlayerScript.Instance.RedPlayerInfos.Ressource -= 1;
        }
        else
        {
            PlayerScript.Instance.BluePlayerInfos.Ressource -= 1;
        }
    }
}
/// <summary>
/// Class qui regroupe toutes les variables pour une carte event
/// </summary>
[System.Serializable]
public class EventCard {
    public string _eventName = "";
    [TextArea] public string _description = "";
    public MYthsAndSteel_Enum.EventCard _eventType = MYthsAndSteel_Enum.EventCard.Activation_de_nodus;
    public int _eventCost = 0;
    public bool _isEventInFinalGame = true;
    public Sprite _eventSprite = null;
    public GameObject _effectToSpawn = null;
    [SerializeField] private VictoryScreen victoryScreen;
}