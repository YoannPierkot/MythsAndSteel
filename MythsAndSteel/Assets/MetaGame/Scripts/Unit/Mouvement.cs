using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mouvement : MonoSingleton<Mouvement>
{
    #region ListeTile&Id
    [Header("LISTES DES CASES")]
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unité.
    public List<int> _selectedTileId;


    public List<int> _newNeighbourId => newNeighbourId;
    private List<int> temp = new List<int>(); //

    #endregion

    #region RenduDeplacement
    [SerializeField] private float speed = 1; // Speed de déplacement de l'unité 
    float speed1; // speed de base.
    private GameObject mStart; // mT Start. 
    private GameObject mEnd; // mT End.
    private GameObject mUnit; // mT Unité.
    int MvmtIndex = 1; // Numéro du mvmt actuel dans la liste selectedTileId;
    [SerializeField] bool Launch = false; // Evite les répétitions dans updatingmove();
    #endregion

    #region InfoUnit

    //Déplacement restant de l'unité au départ
    int MoveLeftBase = 0;

    [Header("INFOS DE L UNITE")]
    //Est ce que l'unité a commencé à choisir son déplacement
    [SerializeField] private bool _isInMouvement;
    public bool IsInMouvement
    {
        get
        {
            return _isInMouvement;
        }
        set
        {
            _isInMouvement = value;
        }
    }

    //Est ce qu'une unité est sélectionnée
    [SerializeField] private bool _selected;
    public bool Selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
        }
    }
    private bool RouteBonus = false; // Le bonus de route a-t-il été séléctionné ?
    private bool check = false; // Si une condition bloquant le mvmt de l'unité a été détéctée.
    // Mouvement en cours de traitement ?
    [SerializeField] private bool _mvmtRunning = false;
    public bool MvmtRunning => _mvmtRunning;
    #endregion InfoUnit

    #region RenduSpriteTile
    [Header("SPRITES POUR LES CASES")]
    [SerializeField] private Sprite _tileSprite = null;
    [SerializeField] private Sprite _selectedSprite = null;
    public Sprite selectedSprite
    {
        get
        {
            return _selectedSprite;
        }
    }
    [SerializeField] private List<MYthsAndSteel_Enum.TerrainType> EffectToCheck;
    [SerializeField] private Sprite UpArrow = null;
    [SerializeField] private Sprite DownArrow = null;
    [SerializeField] private Sprite LeftArrow = null;
    [SerializeField] private Sprite RightArrow = null;
    [SerializeField] private Sprite Virage1 = null;
    [SerializeField] private Sprite Virage2 = null;
    [SerializeField] private Sprite Virage3 = null;
    [SerializeField] private Sprite Virage4 = null;
    [SerializeField] private Sprite Horizontal = null;
    [SerializeField] private Sprite Vertical = null;
    [SerializeField] private Sprite Lastup = null;
    [SerializeField] private Sprite Lastdown = null;
    [SerializeField] private Sprite Lastright = null;
    [SerializeField] private Sprite Lastleft = null;


    #endregion RenduSpriteTile

    private void Update()
    {
        // Permet d'effectuer le moveTowards de l'unité à sa prochaine case.
        UpdatingMove(mUnit, mStart, mEnd);

    }

    /// <summary>
    /// Cette fonction "highlight" les cases atteignables par l'unité sur la case sélectionnée.
    /// </summary>
    /// <param name="tileId">Tile centrale</param>
    /// <param name="Range">Range de l'unité</param>
    public void Highlight(int tileId, int Range, int lasttileId)
    {
        // Si il s'agit d'une route et que la range est de 0.
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, tileId) && Range == 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                {
                    i = true;
                }
                foreach (MYthsAndSteel_Enum.TerrainType Type in TileSc.TerrainEffectList)
                {
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Est && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Nord && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Sud && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Ouest && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, ID))
                    {
                        i = true;
                    }
                }
                if (!i)
                {
                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                }
            }
        }
        // Si il s'agit d'une route et que la range est de 1.
        else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, tileId) && Range == 1)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                {
                    i = true;
                }
                foreach (MYthsAndSteel_Enum.TerrainType Type in TileSc.TerrainEffectList)
                {
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Est && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Nord && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Sud && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Ouest && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                    {
                        i = true;
                    }
                }
                if (!i)
                {
                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                    Highlight(ID, 0, lasttileId);
                }
            }
        }
        else if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (GameManager.Instance.IsPlayerRedTurn)
                {
                    if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                    {
                        if (!TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                        {
                            i = true;
                        }
                        if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && Range == 1)
                        {
                            i = true;
                        }
                    }
                }
                if (!GameManager.Instance.IsPlayerRedTurn)
                {
                    if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                    {
                        if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                        {
                            i = true;
                        }
                        if (!TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && Range == 1)
                        {
                            i = true;
                        }
                    }
                }

                if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                {
                    if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, tileId) && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, tileId))
                    {
                        i = true;
                    }

                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, tileId))
                {
                    if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, ID) && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                    {
                        i = true;
                    }
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Est && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Nord && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Sud && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Ouest && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, ID))
                {
                    if (Range >= 2)
                    {
                        TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite);

                        if (!newNeighbourId.Contains(ID))
                        {
                            newNeighbourId.Add(ID);
                        }
                        Highlight(ID, Range - 2, tileId);
                    }
                    i = true;
                }
                if (!i)
                {                        
                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite);

                    if (!newNeighbourId.Contains(ID))
                    {                    
                        newNeighbourId.Add(ID);
                    }

                    Highlight(ID, Range - 1, tileId);
                }
            }
        }
    }

    /// <summary>
    /// Lance le mouvement d'une unité avec une range défini.
    /// </summary>
    /// <param name="tileId">Tile de l'unité</param>
    /// <param name="Range">Mvmt de l'unité</param>
    public void StartMvmtForSelectedUnit()
    {
        GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;
        mUnit = tileSelected.GetComponent<TileScript>().Unit;
        if ((GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.ActivationLeft > 0) || (mUnit.GetComponent<UnitScript>()._hasStartMove && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.ActivationLeft == 0))
        {
            if (tileSelected != null)
            {
                mUnit = tileSelected.GetComponent<TileScript>().Unit;
                if (!mUnit.GetComponent<UnitScript>().IsMoveDone)
                {
                    _selected = true;
                    MoveLeftBase = mUnit.GetComponent<UnitScript>().MoveLeft;
                    UIInstance.Instance.ButtonRenfortJ1.GetComponent<Button>().interactable = false;
                    StartMouvement(TilesManager.Instance.TileList.IndexOf(tileSelected), mUnit.GetComponent<UnitScript>().MoveSpeed - (mUnit.GetComponent<UnitScript>().MoveSpeed - MoveLeftBase) + mUnit.GetComponent<UnitScript>().MoveSpeedBonus);
                }
                else
                {
                    _selected = false;
                }
            }
            else
            {
                _selected = false;
            }
        }
        else if ((!GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.ActivationLeft > 0) || (mUnit.GetComponent<UnitScript>()._hasStartMove && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.ActivationLeft == 0))
        {
            if (tileSelected != null)
            {
                mUnit = tileSelected.GetComponent<TileScript>().Unit;
                if (!mUnit.GetComponent<UnitScript>().IsMoveDone)
                {
                    _selected = true;
                    MoveLeftBase = mUnit.GetComponent<UnitScript>().MoveLeft;
                    UIInstance.Instance.ButtonRenfortJ2.GetComponent<Button>().interactable = false;
                    StartMouvement(TilesManager.Instance.TileList.IndexOf(tileSelected), mUnit.GetComponent<UnitScript>().MoveSpeed - (mUnit.GetComponent<UnitScript>().MoveSpeed - MoveLeftBase) + mUnit.GetComponent<UnitScript>().MoveSpeedBonus);
                }
                else
                {
                    _selected = false;
                }
            }
            else
            {
                _selected = false;
            }
        }
    }

    /// <summary>
    /// Lance le mvmt d'une unité séléctionnée avec sa range.
    /// </summary>
    /// <param name="tileId"></param>
    /// <param name="Range"></param>
    public void StartMouvement(int tileId, int Range)
    {
        mvmtrunning = false;
        if (!_mvmtRunning && !_isInMouvement)
        {
       
            _isInMouvement = true;
            _selectedTileId.Add(tileId);
            List<int> ID = new List<int>();
            ID.Add(tileId);

            // Lance l'highlight des cases dans la range.
            Highlight(tileId, Range, tileId);
            UIInstance.Instance.DesactivateNextPhaseButton();
        }
        DisplayMoveArrow();
    }

    /// <summary>
    /// Arête le Mouvement pour l'unité selectionnée (menu, cases highlights...)
    /// </summary>
    public void StopMouvement(bool forceStop)
    {
        mvmtrunning = false;
        if(Last != null)
        {
            StopCoroutine(Last);
        }
        if (newNeighbourId.Count > 0)
        {
            foreach (int Neighbour in newNeighbourId) // Supprime toutes les tiles.
            {
                if (TilesManager.Instance.TileList[Neighbour] != null && TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>()._Child.Count != 0)
                {
                    TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect);
                }
            }
        }

        if (_selectedTileId.Count >= 1)
        {
            foreach (int NeighbourSelect in _selectedTileId) // Si un path de mvmt était séléctionné.
            {
                if (TilesManager.Instance.TileList[NeighbourSelect] != null)
                {
                    TilesManager.Instance.TileList[NeighbourSelect].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect);
                }
            }
        }

        // Clear de toutes les listes et stats.
        RouteBonus = false;
        _selectedTileId.Clear();
        newNeighbourId.Clear();
        mStart = null;
        mEnd = null;
        _isInMouvement = false;
        _selected = false;

        if (mUnit != null) mUnit.GetComponent<UnitScript>().MoveLeft = forceStop ? MoveLeftBase : mUnit.GetComponent<UnitScript>().MoveLeft;

        if (!forceStop) if (mUnit != null) mUnit.GetComponent<UnitScript>().checkMovementLeft();

        mUnit = null;
       if( !CapacitySystem.Instance.capacityTryStart)
        {

        RaycastManager.Instance.ActualTileSelected = null;
            Debug.Log("fdjks");
        }
        


        UIInstance.Instance.ActivationUnitPanel.CloseMovementPanel();

        UIInstance.Instance.ActivateNextPhaseButton();

        _mvmtRunning = false;
        if(GameManager.Instance.IsPlayerRedTurn)
        {
            if (UIInstance.Instance.RedRenfortCount == 0)
            {

            UIInstance.Instance.ButtonRenfortJ1.GetComponent<Button>().interactable = true;

            }

        }
        else
        {
            if (UIInstance.Instance.BlueRenfortCount == 0)
            {

            UIInstance.Instance.ButtonRenfortJ2.GetComponent<Button>().interactable = true;
            }
        }
        Attaque.Instance.Attack();
        DisplayMoveArrow();
    }

    /// <summary>
    /// Ajoute la tile à TileSelected. Pour le mvmt du joueur => Check egalement toutes les conditions de déplacement.
    /// </summary>
    /// <param name="tileId">Tile</param>
    public void AddMouvement(int tileId)
    {
        check = false;
        if (_isInMouvement)
        {
            if (newNeighbourId.Contains(tileId)) // Si cette case est dans la range de l'unité.
            {
                if (_selectedTileId.Contains(tileId))// Si cette case est déjà selectionnée.
                {                        
                    // Supprime toutes les cases sélectionnées à partir de l'ID tileId.
                    for (int i = _selectedTileId.IndexOf(tileId); i < _selectedTileId.Count; i++)
                    {

                        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, _selectedTileId[i]) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, _selectedTileId[i]))
                        {
                            // Redistribution du Range à chaque suppression de case.
                            if (RouteBonus)
                            {
                                RouteBonus = false;
                                if (mUnit.GetComponent<UnitScript>().MoveLeft + 1 > mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed)
                                {
                                    mUnit.GetComponent<UnitScript>().MoveSpeedBonus += 1;
                                    Debug.Log("mvt");
                                }
                                else
                                {
                                    mUnit.GetComponent<UnitScript>().MoveLeft += 1;
                                }
                            }
                            else
                            {
                                int moveToAdd = 2;
                                for (int w = 1; w <= moveToAdd; w++)
                                {
                                    if (mUnit.GetComponent<UnitScript>().MoveLeft < mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed)
                                    {
                                        mUnit.GetComponent<UnitScript>().MoveLeft++;
                                    }
                                    else
                                    {
                                        mUnit.GetComponent<UnitScript>().MoveSpeedBonus++;
                                    }
                                }
                            }

                            temp.Add(_selectedTileId[i]);
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite); // Repasse les sprites en apparence "séléctionnable".
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveArrow);
                        }
                        else
                        {
                            // Redistribution du Range à chaque suppression de case.
                            if (mUnit.GetComponent<UnitScript>().MoveLeft + 1 > mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed)
                            {
                                int moveToAdd = 1 - (mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed - mUnit.GetComponent<UnitScript>().MoveLeft);
                                mUnit.GetComponent<UnitScript>().MoveLeft = mUnit.GetComponent<UnitScript>().UnitSO.MoveSpeed;
                                mUnit.GetComponent<UnitScript>().MoveSpeedBonus += moveToAdd;
                                Debug.Log(moveToAdd);

                            }
                            else
                            {
                                if (RouteBonus)
                                {
                                    RouteBonus = false;
                                }
                                else
                                {
                                    mUnit.GetComponent<UnitScript>().MoveLeft += 1;
                                }
                            }

                            temp.Add(_selectedTileId[i]);
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect, _selectedSprite); // Repasse les sprites en apparence "séléctionnable".

                        }
                    }
                    foreach (int i in temp)
                    {
                        _selectedTileId.Remove(i);
                    }
                    temp.Clear();

                    Attaque.Instance.RemoveTileSprite();
                    Attaque.Instance.StartAttackSelectionUnit(_selectedTileId[_selectedTileId.Count - 1]);

                }  
                else if (PlayerStatic.IsNeighbour(tileId, _selectedTileId[_selectedTileId.Count - 1], TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
                {
                    // et qu'il reste du mvmt, on assigne la nouvelle case selectionnée à la liste SelectedTile.
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, _selectedTileId[_selectedTileId.Count - 1]) == MYthsAndSteel_Enum.Direction.Est && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                    {
                        check = true;
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, _selectedTileId[_selectedTileId.Count - 1]) == MYthsAndSteel_Enum.Direction.Nord && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                    {
                        check = true;
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, _selectedTileId[_selectedTileId.Count - 1]) == MYthsAndSteel_Enum.Direction.Sud && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                    {
                        check = true;
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, _selectedTileId[_selectedTileId.Count - 1]) == MYthsAndSteel_Enum.Direction.Ouest && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                    {
                        check = true;
                    }
                    if(PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, tileId))
                    {
                        if(!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, _selectedTileId[_selectedTileId.Count - 1]) && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, _selectedTileId[_selectedTileId.Count - 1]))
                        {
                            check = true;
                        }
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, _selectedTileId[_selectedTileId.Count - 1]))
                    {
                        if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, tileId) && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, tileId))
                        {
                            check = true;
                        }
                    }
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, tileId) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, tileId))
                    {
                        if (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus >= 2 && !check)
                        {
                            Debug.Log("La tile d'ID : " + tileId + " est une foret ou un mont.");
                            int moveToDecrease = 2;
                            check = true;

                            for (int i = 1; i <= moveToDecrease; i++)
                            {
                                if(mUnit.GetComponent<UnitScript>().MoveLeft > 0)
                                {
                                    mUnit.GetComponent<UnitScript>().MoveLeft--;
                                }
                                else
                                {
                                    mUnit.GetComponent<UnitScript>().MoveSpeedBonus--;
                                }
                            }
                            _selectedTileId.Add(tileId);
                            Attaque.Instance.RemoveTileSprite();
                            Attaque.Instance.StartAttackSelectionUnit(tileId);
                        }
                        else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, _selectedTileId[_selectedTileId.Count - 1]) && (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus) == 1 && !RouteBonus && !check)
                        {
                            Debug.Log("Bonus");
                            RouteBonus = true;
                            check = true;
                            if (mUnit.GetComponent<UnitScript>().MoveLeft > 0)
                            {
                                mUnit.GetComponent<UnitScript>().MoveLeft--;
                            }
                            else
                            {
                                mUnit.GetComponent<UnitScript>().MoveSpeedBonus--;
                            }
                            _selectedTileId.Add(tileId);
                            Attaque.Instance.RemoveTileSprite();
                            Attaque.Instance.StartAttackSelectionUnit(tileId);
                        }
                        else
                        {
                            check = true;
                            Debug.Log("La tile d'ID : " + tileId + " est une foret ou un mont.");
                        }
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, _selectedTileId[_selectedTileId.Count - 1]) && (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus) == 0 && !RouteBonus)
                    {
                        RouteBonus = true;
                        check = true;
                        _selectedTileId.Add(tileId);
                        Attaque.Instance.RemoveTileSprite();
                        Attaque.Instance.StartAttackSelectionUnit(tileId);
                    }
                    if (!check)
                    {
                        if (mUnit.GetComponent<UnitScript>().MoveLeft > 0)
                        {
                            mUnit.GetComponent<UnitScript>().MoveLeft--; // sup 1 mvmt.
                            _selectedTileId.Add(tileId);

                            Attaque.Instance.RemoveTileSprite();
                            Attaque.Instance.StartAttackSelectionUnit(tileId);
                        }
                        else if (mUnit.GetComponent<UnitScript>().MoveSpeedBonus > 0)
                        {
                            mUnit.GetComponent<UnitScript>().MoveSpeedBonus--; // sup 1 mvmt.
                            _selectedTileId.Add(tileId);

                            Attaque.Instance.RemoveTileSprite();
                            Attaque.Instance.StartAttackSelectionUnit(tileId);
                        }
                    }
                } // Sinon, si cette case est bien voisine de l'ancienne selection. 
                else // Sinon cette case est trop loin de l'ancienne seletion.
                {
                    Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + _selectedTileId[_selectedTileId.Count - 1]);
                }
            }
            // Sinon cette case est hors de la range de l'unité.
            else
            {
                Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + _selectedTileId[_selectedTileId.Count - 1]);
            }
        }
        if (_selectedTileId.Count > 1 && TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().Unit == null)
        {
            UIInstance.Instance.ActivationUnitPanel.ShowMovementPanel();
        }
        else
        {
            UIInstance.Instance.ActivationUnitPanel.CloseMovementPanel();
        }
        DisplayMoveArrow();
    }

    /// <summary>
    /// Détruit les enfants qui ne sont pas dans la liste de déplacement.
    /// </summary>
    public void DeleteChildWhenMove()
    {
        foreach (int Neighbour in newNeighbourId) // Supprime toutes les tiles.
        {
            if (TilesManager.Instance.TileList[Neighbour] != null && !_selectedTileId.Contains(Neighbour))
            {
                TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect);
            }
        }
    }

    [SerializeField] private bool _mvmtrunning = false;
    public bool mvmtrunning
    {
        get
        {
            return _mvmtrunning;
        }
        set
        {
            _mvmtrunning = value;
            CapacitySystem.Instance.Updatebutton();
        }
    }

    /// <summary>
    /// Assigne le prochain mouvement demandé à l'unité. Change les stats de l'ancienne et de la nouvelle case. Actualise les informations de position de l'unité.
    /// </summary>
    public void ApplyMouvement()
    {
        if (_selectedTileId.Count > 1)
        {
            if (TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().Unit != null)
            {
                if (GameManager.Instance.IsPlayerRedTurn == TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                {
                    UIInstance.Instance.ActivationUnitPanel.ShowMovementPanel();
                    Debug.Log("Vous ne pouvez pas terminer votre mouvement sur une unité alliée.");
                    return;
                }
            }

            GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;

            if (tileSelected != null && (_selectedTileId.Count != 0 && _selectedTileId.Count != 1))
            {
                _mvmtRunning = true;
                mStart = tileSelected; // Assignation du nouveau départ.
                mEnd = TilesManager.Instance.TileList[_selectedTileId[MvmtIndex]];  // Assignation du nouvel arrirée.

                mUnit.GetComponent<UnitScript>()._hasStartMove = true;

                foreach (int Neighbour in newNeighbourId) // Désactive toutes les cases selectionnées par la fonction Highlight.
                {
                    if (!_selectedTileId.Contains(Neighbour))
                    {
                        TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect); // Assigne un sprite empty à toutes les anciennes cases "neighbour"
                    }
                }
            }
        }
        else
        {
            Attaque.Instance.Attack();
        }
        mvmtrunning = true;
        UIInstance.Instance.DesactivateNextPhaseButton();
        foreach(int ID in PlayerStatic.GetNeighbourDiag(_selectedTileId[_selectedTileId.Count - 1], TilesManager.Instance.TileList[_selectedTileId.Count - 1].GetComponent<TileScript>().Line, false))
        {
          if(ID >= 81)
            {
                break;
            }


            TilesManager.Instance.TileList[ID].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveArrow);
            
        }

       
            SoundController.Instance.PlaySound(RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>().SonDeplacement);
       

        Attaque.Instance.RemoveTileSprite(true);

        //Ferme le panneau de déplacement
        UIInstance.Instance.ActivationUnitPanel.CloseMovementPanel();
    }

    bool TileAlreadyUsed = false;
    [Header("Unit collision.")]
    [SerializeField] List<UnitScript> ActualUnit;
    [SerializeField] List<UnitScript> LastUnit;
    [SerializeField] List<TileScript> TileUnit;
    /// <summary>
    /// Coroutine d'attente entre chaque case. Probablement pendant ce temps que l'on devra appliquer les effets de case.
    /// </summary>
    /// <returns>Temps à définir</returns>
    private IEnumerator MvmtEnd()
    {
        mEnd.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveSelect); // La case dépassée redevient une "empty"
        if (mEnd.GetComponent<TileScript>().Unit == null)
        {
            mEnd.GetComponent<TileScript>().AddUnitToTile(mUnit);
        }
        else
        {
            ActualUnit.Add(mEnd.GetComponent<TileScript>().Unit.GetComponent<UnitScript>());
            if (mEnd.GetComponent<TileScript>().LastUnit != null)
            {
                LastUnit.Add(mEnd.GetComponent<TileScript>().LastUnit.GetComponent<UnitScript>());
            }
            else
            {
                LastUnit.Add(null);
            }
            TileUnit.Add(mEnd.GetComponent<TileScript>());
            mEnd.GetComponent<TileScript>().ClearUnitInfo();
            mEnd.GetComponent<TileScript>().AddUnitToTile(mUnit);
        }
        // L'unité de la case d'arrivée devient celle de la case de départ.
        if (mStart != null)
        {
            if (mStart.GetComponent<TileScript>().Unit == mUnit) 
            {
                mStart.GetComponent<TileScript>().RemoveUnitFromTile();
            }
        }
        // L'ancienne case n'a plus d'unité.
        if(mUnit != null)
        {
            mUnit.GetComponent<UnitScript>().ActualTiledId = TilesManager.Instance.TileList.IndexOf(mEnd);
        }

        RaycastManager.Instance.ActualTileSelected = mEnd;
        mStart = mEnd;
        mEnd = null;

        yield return new WaitForSeconds(.25f); // Temps d'attente.
        if (MvmtIndex < _selectedTileId.Count - 1) // Si il reste des mvmts à effectuer dans la liste SelectedTile.
        {
            MvmtIndex++;
            ApplyMouvement();
        }
        else // Si il ne reste aucun mvmt dans la liste SelectedTile.
        {
            MvmtIndex = 1;
            if (TileUnit.Count > 0)
            {
                for (int i = 0; i < TileUnit.Count; i++)
                {
                    Debug.Log("apply");
                    TileUnit[i].AddUnitInfo(ActualUnit[i], LastUnit[i]);
                }
            }
            StopMouvement(false); // Arête le mvmt de l'unité.
        }
        Launch = false; // Reset de la bool Launch
    }

    /// <summary>
    /// Cette fonction lance l'animation de translation de l'unité entre les cases.
    /// </summary>
    /// <param name="Unit">The unit gameobject.</param>
    /// <param name="StartPos">start position tile</param>
    /// <param name="EndPos">end position tile</param>
    private Coroutine Last;
    private void UpdatingMove(GameObject Unit, GameObject StartPos, GameObject EndPos)
    {
        if (Unit != null && StartPos != null && EndPos != null)
        {
            AnimationUpdate(Unit, EndPos);
            Unit.transform.position = Vector2.MoveTowards(Unit.transform.position, EndPos.transform.position, speed1); // Application du mvmt.
            speed1 = Mathf.Abs((Vector2.Distance(mUnit.transform.position, mEnd.transform.position) * speed * Time.deltaTime)); // Régulation de la vitesse. (effet de ralentissement) 
            if (Vector2.Distance(mUnit.transform.position, mEnd.transform.position) <= 0.05f && Launch == false) // Si l'unité est arrivée.
            {
                Unit.GetComponent<UnitScript>().Animation.SetFloat("X", 0);
                Unit.GetComponent<UnitScript>().Animation.SetFloat("Y", 0);
                Launch = true;
                Last = StartCoroutine(MvmtEnd()); // Lancer le prochain mvmt avec délai. 
            }
            else // Sinon appliqué l'opacité à la case d'arrivée en fonction de la distance unité - arrivée.
            {
                for (int i = 0; i < mEnd.GetComponent<TileScript>()._Child.Count; i++)
                {
                    if (mEnd.GetComponent<TileScript>()._Child[i].tag == "Moveselectable")
                    {
                        mEnd.GetComponent<TileScript>()._Child[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Vector2.Distance(mUnit.transform.position, mEnd.transform.position));
                    }
                }

            }
        }
    }

    /// <summary>
    /// Lance l'animation de déplacement de l'unité
    /// </summary>
    /// <param name="Unit"></param>
    /// <param name="EndPos"></param>
    private void AnimationUpdate(GameObject Unit, GameObject EndPos)
    {
        Unit.GetComponent<UnitScript>().Animation.SetFloat("X", EndPos.transform.position.x - Unit.transform.position.x);
        Unit.GetComponent<UnitScript>().Animation.SetFloat("Y", EndPos.transform.position.y - Unit.transform.position.y);
        if (Unit.GetComponent<UnitScript>().Animation.GetFloat("X") < 0)
        {

        Unit.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(Unit.GetComponent<UnitScript>().Animation.GetFloat("X") > 0)
            {
            Unit.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    public List<int> GetNeighbourDirect(int tileId, int Range)
    {
        List<int> Temp = new List<int>();
        // Si il s'agit d'une route et que la range est de 0.
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, tileId) && Range == 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                {
                    i = true;
                }
                foreach (MYthsAndSteel_Enum.TerrainType Type in TileSc.TerrainEffectList)
                {
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Est && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Nord && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Sud && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Ouest && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, ID))
                    {
                        i = true;
                    }
                }
                if (!i)
                {
                    if (!Temp.Contains(ID) && !_selectedTileId.Contains(ID))
                    {
                        Temp.Add(ID);
                    }
                }
            }
        }
        // Si il s'agit d'une route et que la range est de 1.
        else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Route, tileId) && Range == 1)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                {
                    i = true;
                }
                foreach (MYthsAndSteel_Enum.TerrainType Type in TileSc.TerrainEffectList)
                {
                    if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Est && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Nord && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Sud && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Ouest && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                    {
                        i = true;
                    }
                    else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                    {
                        i = true;
                    }
                }
                if (!i)
                {
                    if (!Temp.Contains(ID) && !_selectedTileId.Contains(ID))
                    {
                        Temp.Add(ID);
                    }
                }
            }
        }
        else if (Range > 0)
        {
                foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;
                if (GameManager.Instance.IsPlayerRedTurn)
                {
                    if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                    {
                        if (!TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                        {
                            i = true;
                        }
                        if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && Range == 1)
                        {
                            i = true;
                        }
                    }
                }
                if (!GameManager.Instance.IsPlayerRedTurn)
                {
                    if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit != null)
                    {
                        if (TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                        {
                            i = true;
                        }
                        if (!TilesManager.Instance.TileList[ID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && Range == 1)
                        {
                            i = true;
                        }
                    }
                }

                if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Ravin, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Eau, ID))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                {
                    if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, tileId) && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, tileId))
                    {
                        i = true;
                    }

                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, tileId))
                {
                    if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, ID) && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, ID))
                    {
                        i = true;
                    }
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Est, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Est && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Est, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Nord, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Nord && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Nord, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Sud, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Sud && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Sud, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Rivière_Ouest, tileId) && PlayerStatic.CheckDirection(tileId, ID) == MYthsAndSteel_Enum.Direction.Ouest && !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Pont_Ouest, tileId))
                {
                    i = true;
                }
                else if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Mont, ID) || PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Forêt, ID))
                {
                    if(Range < 2)
                    {
                        i = true;
                    }
                }
                if (!i)
                {
                    if (!Temp.Contains(ID) && !_selectedTileId.Contains(ID))
                    {
                        Temp.Add(ID);
                    }
                }
            }
        }
        return Temp;
    }

     List<int> ArrowedTile = new List<int>();
     List<int> PathTile = new List<int>();
    private void DisplayMoveArrow()
    {
        if (PathTile.Count > 0)
        {
            foreach (int T in PathTile)
            {
                TilesManager.Instance.TileList[T].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath);
            }
            PathTile.Clear();
        }
        if(ArrowedTile.Count > 0)
        {
            foreach (int T in ArrowedTile)
            {
                TilesManager.Instance.TileList[T].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveArrow);
            }
        }
        ArrowedTile = new List<int>();

        if (_selectedTileId.Count > 0)
        {
            if (GetNeighbourDirect(_selectedTileId[_selectedTileId.Count - 1], mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus).Count > 0)
            {
                if (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus == 0 && RouteBonus == true)
                {

                }
                else if (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus == 1 && RouteBonus == true)
                {

                }
                else
                {
                    foreach (int ID in GetNeighbourDirect(_selectedTileId[_selectedTileId.Count - 1], mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus))
                    {
                        if (newNeighbourId.Contains(ID))
                        {
                            switch (PlayerStatic.CheckDirection(_selectedTileId[_selectedTileId.Count - 1], ID))
                            {
                                case MYthsAndSteel_Enum.Direction.Nord:
                                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveArrow, UpArrow);

                                    break;
                                case MYthsAndSteel_Enum.Direction.Sud:
                                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveArrow, DownArrow);

                                    break;
                                case MYthsAndSteel_Enum.Direction.Est:
                                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveArrow, RightArrow);

                                    break;
                                case MYthsAndSteel_Enum.Direction.Ouest:
                                    TilesManager.Instance.TileList[ID].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MoveArrow, LeftArrow);
                                    break;
                            }
                            ArrowedTile.Add(ID);
                        }
                    }
                }
            }
        }
        if(_selectedTileId.Count >= 2)
        {
            for (int i = 1; i < _selectedTileId.Count - 1; i++)
            {
                MYthsAndSteel_Enum.Direction avant = PlayerStatic.CheckDirection(_selectedTileId[i - 1], _selectedTileId[i]);
                MYthsAndSteel_Enum.Direction apres = PlayerStatic.CheckDirection(_selectedTileId[i + 1], _selectedTileId[i]);
                Debug.Log(_selectedTileId[i]);
                if(avant == MYthsAndSteel_Enum.Direction.Sud)
                {
                    switch (apres)
                    {
                        case MYthsAndSteel_Enum.Direction.Nord: 
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Vertical);
                            break;
                        case MYthsAndSteel_Enum.Direction.Est:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage1);
                            break;
                        case MYthsAndSteel_Enum.Direction.Ouest:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage2);
                            break;
                    }
                }
                else if (avant == MYthsAndSteel_Enum.Direction.Nord)
                {
                    switch (apres)
                    {
                        case MYthsAndSteel_Enum.Direction.Sud:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Vertical);
                            break;
                        case MYthsAndSteel_Enum.Direction.Est:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage4);
                            break;
                        case MYthsAndSteel_Enum.Direction.Ouest:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage3);
                            break;
                    }
                }
                else if (avant == MYthsAndSteel_Enum.Direction.Est)
                {
                    switch (apres)
                    {
                        case MYthsAndSteel_Enum.Direction.Ouest:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Horizontal);
                            break;
                        case MYthsAndSteel_Enum.Direction.Sud:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage1);
                            break;
                        case MYthsAndSteel_Enum.Direction.Nord:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage4);
                            break;
                    }
                }
                else if (avant == MYthsAndSteel_Enum.Direction.Ouest)
                {
                    switch (apres)
                    {
                        case MYthsAndSteel_Enum.Direction.Est:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Horizontal);
                            break;
                        case MYthsAndSteel_Enum.Direction.Sud:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage2);
                            break;
                        case MYthsAndSteel_Enum.Direction.Nord:
                            TilesManager.Instance.TileList[_selectedTileId[i]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Virage3);
                            break;
                    }
                }
                PathTile.Add(_selectedTileId[i]);

            }
            switch (PlayerStatic.CheckDirection(_selectedTileId[_selectedTileId.Count - 1], _selectedTileId[_selectedTileId.Count - 2]))
            {
                case MYthsAndSteel_Enum.Direction.Nord:
                    PathTile.Add(_selectedTileId[_selectedTileId.Count - 1]);
                    if(mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus == 0)
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, DownArrow);
                    }
                    else
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Lastup);
                    }

                    break;
                case MYthsAndSteel_Enum.Direction.Sud:
                    PathTile.Add(_selectedTileId[_selectedTileId.Count - 1]);
                    if (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus == 0)
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, UpArrow);
                    }
                    else
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Lastdown);
                    }
                    break;
                case MYthsAndSteel_Enum.Direction.Est:
                    PathTile.Add(_selectedTileId[_selectedTileId.Count - 1]);
                    if (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus == 0)
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, LeftArrow);
                    }
                    else
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Lastright);
                    }
                    break;
                case MYthsAndSteel_Enum.Direction.Ouest:
                    PathTile.Add(_selectedTileId[_selectedTileId.Count - 1]);
                    if (mUnit.GetComponent<UnitScript>().MoveLeft + mUnit.GetComponent<UnitScript>().MoveSpeedBonus == 0)
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, RightArrow);
                    }
                    else
                    {
                        TilesManager.Instance.TileList[_selectedTileId[_selectedTileId.Count - 1]].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.MovePath, Lastleft);
                    }
                    break;
            }
        }
    }
}
