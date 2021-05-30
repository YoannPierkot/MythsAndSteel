using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenfortPhase : MonoSingleton<RenfortPhase>
{
    #region AppelDeScript
    Player player;
    UnitReference unitReference;
    
    #endregion

    [SerializeField] private List<GameObject> _createTileJ1;
    public List<GameObject> CreateTileJ1 => _createTileJ1;

    [SerializeField] private List<GameObject> _createTileJ2;
    public List<GameObject> CreateTileJ2 => _createTileJ2;

    [SerializeField] private List<GameObject> _createLeader1;
    public List<GameObject> CreateLeader1 => _createLeader1;

    [SerializeField] private List<GameObject> _createLeader2;
    public List<GameObject> CreateLeader2 => _createLeader2;

    List<GameObject> _usineListRed = new List<GameObject>();
    List<GameObject> _usineListBlue = new List<GameObject>();

    List<GameObject> _leaderListRed = new List<GameObject>();
    List<GameObject> _leaderListBlue = new List<GameObject>();

    int idCreate = -1;
    bool redPlayerCreation = false;

    private void Start()
    {
        foreach(GameObject typeTile in TilesManager.Instance.TileList)
        {
            if(typeTile.GetComponent<TileScript>().TerrainEffectList.Contains((MYthsAndSteel_Enum.TerrainType.UsineRouge)))
            {
                _usineListRed.Add(typeTile);
            }
            else if(typeTile.GetComponent<TileScript>().TerrainEffectList.Contains((MYthsAndSteel_Enum.TerrainType.UsineBleu)))
            {
                _usineListBlue.Add(typeTile);
            }
        }

        foreach(GameObject unit in PlayerScript.Instance.UnitRef.UnitListRedPlayer)
        {
            if(unit.GetComponent<UnitScript>().UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Leader)
            {
                _leaderListRed.Add(unit);
            }
        }

        foreach(GameObject unit in PlayerScript.Instance.UnitRef.UnitListBluePlayer)
        {
            if(unit.GetComponent<UnitScript>().UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Leader)
            {
                _leaderListBlue.Add(unit);
            }
        }
    }

    /// <summary>
    /// Fais une liste des cases sélectionnables autour de l'usine
    /// </summary>
    public void CreateRenfort(bool playerRed)
    {
        if(GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1 ||
            GameManager.Instance.ActualTurnPhase == MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2 || OrgoneManager.Instance.DoingOrgoneCharge)
        {
            AroundCreateTileUsine(playerRed);
            AroundLeader(playerRed);
            ChangeButtonStatut(true, playerRed);
        }
        else
        {
            ChangeButtonStatut(false, playerRed);
        }
    }

    /// <summary>
    /// Active ou désactive les boutons du menu renfort
    /// </summary>
    /// <param name="activate"></param>
    void ChangeButtonStatut(bool activate, bool playerRed){
        UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().interactable = activate;
        UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().blocksRaycasts = activate;

        UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().interactable = activate;
        UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().blocksRaycasts = activate;

        UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().interactable = activate;
        UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().blocksRaycasts = activate;

        UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().interactable = activate;
        UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().blocksRaycasts = activate;

        UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().interactable = activate;
        UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().blocksRaycasts = activate;

        UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().interactable = activate;
        UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().blocksRaycasts = activate;

        if(activate){
            for(int i = 0; i < (playerRed? PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer.Count : PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer.Count); i++){
                switch(i)
                {
                    case 0:
                        if(playerRed){
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource){
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else{
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        else{
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.BluePlayerInfos.Ressource){
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else{
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        break;

                    case 1:
                        if(playerRed)
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        else
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.BluePlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        break;

                    case 2:
                        if(playerRed)
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        else
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.BluePlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        break;

                    case 3:
                        if(playerRed)
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        else
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.BluePlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        break;

                    case 4:
                        if(playerRed)
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        else
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.BluePlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        break;

                    case 5:
                        if(playerRed)
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        else
                        {
                            if(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[i].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.BluePlayerInfos.Ressource)
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().interactable = true;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            }
                            else
                            {
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().interactable = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().blocksRaycasts = false;
                                UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();
                            }
                        }
                        break;
                }
            }
        }
    }

    #region GetTiles
    /// <summary>
    /// Créer une liste des tiles autour des usines
    /// </summary>
    /// <param name="usine1"></param>
    void AroundCreateTileUsine(bool usine1)
    {
        //Si l'armée est jouer par l'armée Bleu.
        if(usine1 == true)
        {
            _createTileJ1 = CreateTileList(_usineListRed);
        }
        else
        {
            _createTileJ2 = CreateTileList(_usineListBlue);
        }
    }

    /// <summary>
    /// Obtient les cases sélectionnables
    /// </summary>
    /// <param name="usineList"></param>
    /// <returns></returns>
    List<GameObject> CreateTileList(List<GameObject> usineList)
    {
        List<GameObject> tempList = new List<GameObject>();

        foreach(GameObject typeTile in usineList)
        {
            int typeTileID = TilesManager.Instance.TileList.IndexOf(typeTile);
            //Debug.Log(typeTile);
            foreach(int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
            {
                //Tu ajoutes la tile correspondant à l'usineJ2
                if(!tempList.Contains(typeTile))
                {
                    if(typeTile.GetComponent<TileScript>().Unit == null)
                    {
                        tempList.Add(typeTile);
                    }
                }
                //Si le numéro n'est pas présent dans la liste, tu l'ajoutes.
                if(!tempList.Contains(TilesManager.Instance.TileList[idtyleIndex]))
                {
                    tempList.Add(TilesManager.Instance.TileList[idtyleIndex]);
                    //Si il y un boolean qui est retourné alors tu enlèves les éléments de la liste.
                    if(CheckConditions(TilesManager.Instance.TileList[idtyleIndex], typeTileID) == true)
                    {
                        tempList.Remove(TilesManager.Instance.TileList[idtyleIndex]);
                    }
                }
            }
        }

        //Pour chaque élément dans la liste, tu ajoutes l'effet TileCréable
        foreach(GameObject typeWithoutEffect in tempList)
        {
            if(!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
            {
                typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
            }
        }

        return tempList;
    }

    /// <summary>
    ///  Fais une liste des cases sélectionnables autour des leader
    /// </summary>
    /// <param name="leaderArmy1"></param>
    void AroundLeader(bool leaderArmy1)
    {
        //Si l'armée est jouer par l'armée Bleu.
        if(leaderArmy1 == true)
        {
            foreach(GameObject unit in _leaderListRed)
            {
                if(unit != null)
                {
                    int typeTileID = unit.GetComponent<UnitScript>().ActualTiledId;

                    //Pour chaque numéro présent dans le PlayerStatic avec la valeur qu'on a convertit précédemment.
                    foreach(int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                    {
                        GameObject tile = TilesManager.Instance.TileList[idtyleIndex];
                        //Si le numéro n'est pas présent dans la liste, tu l'ajoutes.
                        if(!_createLeader1.Contains(tile))
                        {
                            _createLeader1.Add(tile);

                            //Si il y un boolean qui est retourné alors tu enlèves les éléments de la liste.
                            if(CheckConditions(tile, typeTileID) == true)
                            {
                                _createLeader1.Remove(tile);
                            }
                        }
                    }
                }
            }
            //Pour chaque élément dans la liste, tu ajoutes l'effet TileCréable
            foreach(GameObject typeWithoutEffect in _createLeader1)
            {
                if(!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                {
                    typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                }
            }
        }


        else
        {
            foreach(GameObject unit in _leaderListBlue)
            {
                if(unit != null)
                {
                    int typeTileID = unit.GetComponent<UnitScript>().ActualTiledId;

                    //Pour chaque numéro présent dans le PlayerStatic avec la valeur qu'on a convertit précédemment.
                    foreach(int idtyleIndex in PlayerStatic.GetNeighbourDiag(typeTileID, TilesManager.Instance.TileList[typeTileID].GetComponent<TileScript>().Line, false))
                    {
                        GameObject tile = TilesManager.Instance.TileList[idtyleIndex];
                        //Si le numéro n'est pas présent dans la liste, tu l'ajoutes.
                        if(!_createLeader2.Contains(tile))
                        {
                            _createLeader2.Add(tile);

                            //Si il y un boolean qui est retourné alors tu enlèves les éléments de la liste.
                            if(CheckConditions(tile, typeTileID) == true)
                            {
                                _createLeader2.Remove(tile);
                            }
                        }
                    }
                }
            }
            //Pour chaque élément dans la liste, tu ajoutes l'effet TileCréable
            foreach(GameObject typeWithoutEffect in _createTileJ2)
            {
                if(!typeWithoutEffect.GetComponent<TileScript>().EffetProg.Contains(MYthsAndSteel_Enum.EffetProg.Zone_creable))
                {
                    typeWithoutEffect.GetComponent<TileScript>().EffetProg.Add(MYthsAndSteel_Enum.EffetProg.Zone_creable);
                }
            }
        }
    }
    #endregion GetTiles

    /// <summary>
    /// Permet de déterminer si autour des cases adjacentes il y a des effets de terrain qui pourraient gêner le déploiement de troupes.
    /// </summary>
    /// <param name="tileCheck"></param>
    /// <returns></returns>
    bool CheckConditions(GameObject tileCheck, int origin)
    {
        //Y a une unité?
        if(tileCheck.GetComponent<TileScript>().Unit != null)
        {
            return true;
        }

        //Y a de l'eau ou un ravin?
        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Ravin)
            || tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Eau))
        {
            return true;
        }

        //RIVIERE?
        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Nord)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Sud)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Nord))
            {
                return true;
            }
        }

        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Sud)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Nord)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Pont_Sud))
            {
                return true;
            }
        }

        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Est)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest))
            {
                return true;
            }
        }

        if(tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Est)
        && PlayerStatic.CheckDirection(origin, tileCheck.GetComponent<TileScript>().TileId) == MYthsAndSteel_Enum.Direction.Ouest)
        {
            if(!tileCheck.GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Rivière_Est))
            {
                return true;
            }
        }

        return false;

        //Ajouter les barbelets.
    }

    #region CréerUnité
    /// <summary>
    /// Permet de créer une unité
    /// </summary>
    public void craftUnit(int unitId)
    {
        if (GameManager.Instance.IsPlayerRedTurn)
        {
            if (PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[unitId].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource)
            {
                List<GameObject> tileList = new List<GameObject>();
                tileList.AddRange(GameManager.Instance.RenfortPhase.CreateLeader1);
                tileList.AddRange(GameManager.Instance.RenfortPhase.CreateTileJ1);

                idCreate = unitId;
                redPlayerCreation = true;

                GameManager.Instance.StartEventModeTiles(1, true, tileList, "Création d'unité", "Êtes-vous sur de vouloir créer une unité sur cette case");
                GameManager.Instance._eventCall += CreateNewUnit;
                GameManager.Instance._eventCall += UIInstance.Instance.ActivateNextPhaseButton;
                RaycastManager.Instance._mouseCommand.QuitRenfortPanel();
            }
        }
        else
        {
            if (PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[unitId].GetComponent<UnitScript>().UnitSO.CreationCost <= PlayerScript.Instance.RedPlayerInfos.Ressource)
            {
                List<GameObject> tileList = new List<GameObject>();
                tileList.AddRange(GameManager.Instance.RenfortPhase.CreateLeader2);
                tileList.AddRange(GameManager.Instance.RenfortPhase.CreateTileJ2);
                idCreate = unitId;
                redPlayerCreation = false;
                GameManager.Instance.StartEventModeTiles(1, false, tileList, "Création d'unité", "Êtes-vous sur de vouloir créer une unité sur cette case");
                GameManager.Instance._eventCall += CreateNewUnit;
                RaycastManager.Instance._mouseCommand.QuitRenfortPanel();
            }
        }
    }

    /// <summary>
    /// Crée une nouvelle unité sur le terrain au niveau de la tile sélectionnée
    /// </summary>
    void CreateNewUnit(){
        if(redPlayerCreation)
        {
            GameObject obj = Instantiate(PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[idCreate], GameManager.Instance.TileChooseList[0].transform.position, Quaternion.identity);
            GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(obj);
            PlayerScript.Instance.RedPlayerInfos.HasCreateUnit = true;
            PlayerScript.Instance.UnitRef.UnitListRedPlayer.Add(obj);
            if (!OrgoneManager.Instance.DoingOrgoneCharge)
            {
                PlayerScript.Instance.RedPlayerInfos.Ressource -= PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[idCreate].GetComponent<UnitScript>().UnitSO.CreationCost;
                UIInstance.Instance.UpdateRessourceLeft();
            }
            GameManager.Instance.victoryScreen.redRessourcesUsed += PlayerScript.Instance.UnitRef.UnitClassCreableListRedPlayer[idCreate].GetComponent<UnitScript>().UnitSO.CreationCost;

        }
        else
        {
            GameObject obj = Instantiate(PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[idCreate], GameManager.Instance.TileChooseList[0].transform.position, Quaternion.identity);
            GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(obj);
            PlayerScript.Instance.BluePlayerInfos.HasCreateUnit = true;
            PlayerScript.Instance.UnitRef.UnitListBluePlayer.Add(obj);
            if (!OrgoneManager.Instance.DoingOrgoneCharge)
            {
                PlayerScript.Instance.BluePlayerInfos.Ressource -= PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[idCreate].GetComponent<UnitScript>().UnitSO.CreationCost;
                UIInstance.Instance.UpdateRessourceLeft();
            }
            else
            {
                OrgoneManager.Instance.DoingOrgoneCharge = false;
            }

            GameManager.Instance.victoryScreen.blueResourcesUsed += PlayerScript.Instance.UnitRef.UnitClassCreableListBluePlayer[idCreate].GetComponent<UnitScript>().UnitSO.CreationCost;
        }

        GameManager.Instance.TileChooseList.Clear();
    }
    #endregion CréerUnité
}