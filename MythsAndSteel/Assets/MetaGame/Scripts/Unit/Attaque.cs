using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class Attaque : MonoSingleton<Attaque>
{
    #region Variables
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    public List<int> _newNeighbourId => newNeighbourId;
    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unitÃ©.
   
    public GameObject PanelBlockant1;
  
    public GameObject PanelBlockant2;
    public GameObject PanelBlockantOrgone1;
    public GameObject PanelBlockantOrgone2;
    //Est ce que l'unitÃ© a commencÃ© Ã  choisir son dÃ©placement ?
    [SerializeField] private bool _isInAttack;
    public bool IsInAttack
    {
        get
        {
            return _isInAttack;
        }
        set
        {
            _isInAttack = value;
        }
    }

    //Est ce qu'une unitÃ© est sÃ©lectionnÃ©e ?
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
    //Bool pour savoir si l'unité dévie ou pas à remplacer par l'attribut déviation avec l'enum
    [SerializeField] bool _isAttackDeviation;
    public bool IsAttackDeviation => _isAttackDeviation;

    [SerializeField] Sprite _DeviationSelectUI;
    public Sprite DeviationSelectUI => _DeviationSelectUI;
    [SerializeField] Sprite _DeviationOriginalSpriteCase;
    public Sprite DeviationOriginalSpriteCase => _DeviationOriginalSpriteCase;
 


    List<int> SetEnnemyUnitListTileNeighbourDiagUI;
    //Portée d'attaque
    [SerializeField] int _attackRange;
    public int AttackRange => _attackRange;

    //DÃ©gats minimum infligÃ©s 
    [SerializeField] int _damageMinimum;
    public int DamageMinimum => _damageMinimum;

    // Range d'attaque (-) 
    [SerializeField] Vector2 _numberRangeMin;
    public Vector2 NumberRangeMin => _numberRangeMin;

    // DÃ©gats maximum infligÃ©s
    [SerializeField] int _damageMaximum;
    public int DamageMaximum => _damageMaximum;

    // Range d'attaque (+) 
    [SerializeField] Vector2 _numberRangeMax;
    public Vector2 NumberRangeMax => _numberRangeMax;

    // Range d'attaque (+) 
    [SerializeField] bool _isActionDone;
    public bool IsActionDone => _isActionDone;

    int firstDiceInt, secondDiceInt, DiceResult;

    // Cases sélectionnées par le joueur
    [SerializeField] private List<int> SelectedTiles = new List<int>();
    public List<int> _selectedTiles
    {
        get
        {         
            return SelectedTiles;
        }
        set
        {            
            SelectedTiles = value;
        }
    }

    GameObject _selectedUnit = null;

    int numberOfTileToSelect = 0;
    private GameObject _selectedUnitEnemy;
   public GameObject selectedUnitEnnemy
    {
        get
        {
            return _selectedUnitEnemy;
        }
        set
        {
            _selectedUnitEnemy = value;
        }
    }

    [Header("SPRITES POUR LES CASES")]
    [SerializeField] private Sprite _normalAttackSprite = null;
    [SerializeField] private Sprite _selectedSprite = null;

    public Sprite selectedSprite
    {
        get
        {
            return _normalAttackSprite;
        }
    }

    #endregion Variables

    /// <summary>
    /// Fait un lancé de dé
    /// </summary>
    void Randomdice()
    {
        firstDiceInt = Random.Range(1, 7);
        secondDiceInt = Random.Range(1, 7);

        DiceResult = firstDiceInt + secondDiceInt + RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>().DiceBonus;
        //DiceResult = firstDiceInt + secondDiceInt;

        RandomMore();
    }

    /// <summary>
    /// Attaque d'une unité avec un range d'attaque
    /// </summary>
    /// <param name="_numberRangeMin"></param>
    /// <param name="_damageMinimum"></param>
    /// <param name="DiceResult"></param>
    void UnitAttackOneRange(Vector2 _numberRangeMin, int _damageMinimum, int DiceResult)
    {
        if (DiceResult >= _numberRangeMin.x && DiceResult <= _numberRangeMin.y)
        {
            ChangeStat();  
            AnimationUpdate();
            int AttackVariation = 0;
            TileScript T = TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>();
            foreach (MYthsAndSteel_Enum.TerrainType T1 in T.TerrainEffectList)
            {
                foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                    {
                        if (T1 == T2)
                        {
                            if (Type.Child != null)
                            {
                                if (Type.MustBeInstantiate)
                                {
                                    foreach (GameObject G in T._Child)
                                    {
                                        if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                        {
                                            if (Try2.Type == T1)
                                            {
                                                if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                {
                                                    AttackVariation += Try3.AttackApply(_damageMinimum);
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                    {
                                        AttackVariation += Try.AttackApply(_damageMinimum);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum + AttackVariation);
            SoundController.Instance.PlaySound(_selectedUnit.GetComponent<UnitScript>().SonAttaque);
            Debug.Log("Damage : " + _damageMinimum);
            StopAttack();
        }
        if (DiceResult < _numberRangeMin.x)
        {
            if (_isAttackDeviation == true)
            {
                AnimationUpdate();
                ChangeStat();
                _damageMinimum = this._damageMinimum;
                StartDeviation();
            }
            else
            {
                ChangeStat();
                int AttackVariation = 0;
                TileScript T = TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>();
                foreach (MYthsAndSteel_Enum.TerrainType T1 in T.TerrainEffectList)
                {
                    foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                    {
                        foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                        {
                            if (T1 == T2)
                            {
                                if (Type.Child != null)
                                {
                                    if (Type.MustBeInstantiate)
                                    {
                                        foreach (GameObject G in T._Child)
                                        {
                                            if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                            {
                                                if (Try2.Type == T1)
                                                {
                                                    if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                    {
                                                        AttackVariation += Try3.AttackApply(0);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                        {
                                            AttackVariation += Try.AttackApply(0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0 + AttackVariation);
                SoundController.Instance.PlaySound(_selectedUnit.GetComponent<UnitScript>().SonAttaque);
                Debug.Log("Damage : " + null);
                StopAttack();
            }
        }
    }

    /// <summary>
    /// Attaque d'une unité avec deux ranges d'attaque
    /// </summary>
    /// <param name="_numberRangeMin"></param>
    /// <param name="_damageMinimum"></param>
    /// <param name="_numberRangeMax"></param>
    /// <param name="_damageMaximum"></param>
    /// <param name="DiceResult"></param>
    void UnitAttackTwoRanges(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, int DiceResult)
    {
        if (DiceResult >= _numberRangeMin.x && DiceResult <= _numberRangeMin.y)
        {
            ChangeStat();
            _damageMinimum = this._damageMinimum;
            _damageMaximum = this._damageMaximum;
            AnimationUpdate();
            int AttackVariation = 0;
            TileScript T = TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>();
            foreach (MYthsAndSteel_Enum.TerrainType T1 in T.TerrainEffectList)
            {
                foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                    {
                        if (T1 == T2)
                        {
                            if (Type.Child != null)
                            {
                                if (Type.MustBeInstantiate)
                                {
                                    foreach (GameObject G in T._Child)
                                    {
                                        if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                        {
                                            if (Try2.Type == T1)
                                            {
                                                if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                {
                                                    AttackVariation += Try3.AttackApply(_damageMinimum);
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                    {
                                        AttackVariation += Try.AttackApply(_damageMinimum);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum + AttackVariation);
            SoundController.Instance.PlaySound(_selectedUnit.GetComponent<UnitScript>().SonAttaque);
            Debug.Log("Damage : " + _damageMinimum);
            StopAttack();
        }
        if (DiceResult >= _numberRangeMax.x && DiceResult <= _numberRangeMax.y)
        {
            ChangeStat();
          
            _damageMaximum = this._damageMaximum;
            AnimationUpdate();
            int AttackVariation = 0;
            TileScript T = TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>();
            foreach (MYthsAndSteel_Enum.TerrainType T1 in T.TerrainEffectList)
            {
                foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                    {
                        if (T1 == T2)
                        {
                            if (Type.Child != null)
                            {
                                if (Type.MustBeInstantiate)
                                {
                                    foreach (GameObject G in T._Child)
                                    {
                                        if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                        {
                                            if (Try2.Type == T1)
                                            {
                                                if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                {
                                                    AttackVariation += Try3.AttackApply(_damageMinimum);
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                    {
                                        AttackVariation += Try.AttackApply(_damageMinimum);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMaximum + AttackVariation);
            SoundController.Instance.PlaySound(_selectedUnit.GetComponent<UnitScript>().SonAttaque);
            Debug.Log("Damage : " + _damageMaximum);
            StopAttack();
        }
        if (DiceResult < _numberRangeMin.x)
        {
            if (_isAttackDeviation == true)
            {
                ChangeStat();
                AnimationUpdate();
                this._damageMinimum = _damageMinimum;
                StartDeviation();
            }
            else
            {
                ChangeStat();
                int AttackVariation = 0;
                TileScript T = TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>();
                foreach (MYthsAndSteel_Enum.TerrainType T1 in T.TerrainEffectList)
                {
                    foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                    {
                        foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                        {
                            if (T1 == T2)
                            {
                                if (Type.Child != null)
                                {
                                    if (Type.MustBeInstantiate)
                                    {
                                        foreach (GameObject G in T._Child)
                                        {
                                            if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                            {
                                                if (Try2.Type == T1)
                                                {
                                                    if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                    {
                                                        AttackVariation += Try3.AttackApply(0);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                        {
                                            AttackVariation += Try.AttackApply(0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0 + AttackVariation);
                SoundController.Instance.PlaySound(_selectedUnit.GetComponent<UnitScript>().SonAttaque);
                Debug.Log("Damage : " + null);
                StopAttack();
            }

        }
    }

    /// <summary>
    /// Lance l'animation d'attaque
    /// </summary>
    void AnimationUpdate()
    {
        GameObject ActualUnit = RaycastManager.Instance.ActualUnitSelected;
        
        if(ActualUnit.GetComponent<UnitScript>().Animation != null)
        {
            GameObject ActualEnemy = selectedUnitEnnemy;

            float X = ActualEnemy.transform.position.x - ActualUnit.transform.position.x;
            float Y = ActualEnemy.transform.position.y - ActualUnit.transform.position.y;

            if(X >= 0)
            {
                if(Mathf.Abs(X) > Mathf.Abs(Y))
                {
                    ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 1); //right
                    ActualUnit.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if(Mathf.Abs(X) <= Mathf.Abs(Y))
                {
                    if(Y > 0)
                    {
                        ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 2); // up
                    }
                    else if(Y < 0)
                    {
                        ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 3); // down
                    }
                }
            }
            if(X < 0)
            {
                if(Mathf.Abs(X) > Mathf.Abs(Y))
                {
                    ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 1); // left
                    ActualUnit.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if(Mathf.Abs(X) <= Mathf.Abs(Y))
                {
                    if(Y > 0)
                    {
                        ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 2); // up
                    }
                    else if(Y < 0)
                    {
                        ActualUnit.GetComponent<UnitScript>().Animation.SetInteger("A", 3); // down
                    }
                }
            }
            ActualUnit.GetComponent<UnitScript>().Animation.SetBool("Attack", true);
            StartCoroutine(AnimationWait(ActualUnit.GetComponent<UnitScript>().Animation, "Attack"));
        }
    }

    public IEnumerator AnimationWait(Animator AnimToWait, string BoolName)
    {
        if(AnimToWait.runtimeAnimatorController != null)
        {
            yield return new WaitForSeconds(AnimToWait.runtimeAnimatorController.animationClips[0].length);
            AnimToWait.SetBool(BoolName, false);
        }
    }

    
    public bool Go = false;
    /// <summary>
    /// Choisit le type d'attaque
    /// </summary>
    /// <param name="_numberRangeMin"></param>
    /// <param name="_damageMinimum"></param>
    /// <param name="_numberRangeMax"></param>
    /// <param name="_damageMaximum"></param>
    /// <param name="DiceResult"></param>
    void ChooseAttackType(Vector2 _numberRangeMin, int _damageMinimum, Vector2 _numberRangeMax, int _damageMaximum, int DiceResult)
    {
        Go = false;

        //Debug.Log("Dice: " + (firstDiceInt + secondDiceInt));
        _JaugeAttack.SynchAttackBorne(RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>());
        _JaugeAttack.Attack(firstDiceInt + secondDiceInt);

        if (_numberRangeMax.x == 0 && _numberRangeMax.y == 0)
        {
            UnitAttackOneRange(_numberRangeMin, _damageMinimum, DiceResult);
        }
        else
        {
            UnitAttackTwoRanges(_numberRangeMin, _damageMinimum, _numberRangeMax, _damageMaximum, DiceResult);
        }
    }

    /// <summary>
    /// Highlight des cases dans la range d'attaque de l'unitÃ©
    /// </summary>
    /// <param name="tileId"></param>
    /// <param name="Range"></param>
    public void Highlight(int tileId, int currentID, int Range)
    {
        UIInstance.Instance.DesactivateNextPhaseButton();
        if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;

              if(ID == currentID )
                {
                    i = true;

                }

                if (!i)
                {
                    TileSc.ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _normalAttackSprite, 1);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                    Highlight(ID, currentID, Range - 1); ;
                }
            }
        }
    }

    /// <summary>
    /// Ajoute plus d'alÃ©atoire aux lancÃ©s de dÃ©
    /// </summary>
    private void RandomMore()
    {
        if (GameManager.Instance.IsPlayerRedTurn || !GameManager.Instance.IsPlayerRedTurn) 
        {
            if (_selectedUnit.GetComponent<UnitScript>().DoingCharg1Blue == true)
            {
                List<int> unitNeigh = PlayerStatic.GetNeighbourDiag(_selectedUnit.GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[_selectedUnit.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);

                foreach (int i in unitNeigh)
                {
                    if(TilesManager.Instance.TileList[i].GetComponent<TileScript>().Unit != null)
                    {

                    DiceResult++;
                    }
                    Debug.Log(DiceResult);
                }
                _selectedUnit.GetComponent<UnitScript>().DoingCharg1Blue = false;
            }
            if (DiceResult > 12)
            {
                DiceResult = 12;
            }
        }
    }

    /// <summary>
    /// VÃ©rifie si l'unitÃ© selectionnÃ© peut attaquÃ© + rÃ©cupÃ¨re la portÃ©e de l'unitÃ©
    /// </summary>
    public void StartAttackSelectionUnit(int tileId = -1)
    {
        
        GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;
        _selectedUnit = tileSelected.GetComponent<TileScript>().Unit;
        foreach (int i in _selectedTiles)
        {
            foreach (MYthsAndSteel_Enum.TerrainType T1 in TilesManager.Instance.TileList[i].GetComponent<TileScript>().TerrainEffectList)
            {
                foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                    {
                        if (T1 == T2)
                        {
                            if (Type.Child != null)
                            {
                                Debug.Log(Type._terrainName);
                                if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                {
                                    Try.UnCibledByAttack(RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>());
                                }
                            }
                        }
                    }
                }
            }
        }

        _selectedTiles.Clear();
        _newNeighbourId.Clear();
        
        if ((GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.ActivationLeft > 0) || (_selectedUnit.GetComponent<UnitScript>()._hasStartMove && GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.ActivationLeft == 0))
        {
            if (tileId != -1)
            {


                if (!_selectedUnit.GetComponent<UnitScript>()._isActionDone && !_selectedUnit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre))
                {
                   
                    _isInAttack = false;
                    StartAttack(tileId, _selectedUnit.GetComponent<UnitScript>().AttackRange + _selectedUnit.GetComponent<UnitScript>().AttackRangeBonus);
                    
                        
                }
                else
                {
                    _selected = false;
                }

            }
            else
            {
                

                if (tileSelected != null)
                {
                
                    if (!_selectedUnit.GetComponent<UnitScript>()._isActionDone && !_selectedUnit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre))
                    {

                        GetStats();
                        _selected = true;
                        UpdateJauge(tileId);
                        StartAttack(tileSelected.GetComponent<TileScript>().TileId, _selectedUnit.GetComponent<UnitScript>().AttackRange + _selectedUnit.GetComponent<UnitScript>().AttackRangeBonus);
                        
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
        else if ((!GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.ActivationLeft > 0) || (_selectedUnit.GetComponent<UnitScript>()._hasStartMove && !GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.ActivationLeft == 0))
        {
            if (tileId != -1)
            {
                if (!_selectedUnit.GetComponent<UnitScript>()._isActionDone && !_selectedUnit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre))
                {
                    _isInAttack = false;
                    StartAttack(tileId, _selectedUnit.GetComponent<UnitScript>().AttackRange + _selectedUnit.GetComponent<UnitScript>().AttackRangeBonus);
                   

                }
                else
                {
                    _selected = false;
                }
            }
            else
            {
               

                if (tileSelected != null)
                {
           
                    if (!_selectedUnit.GetComponent<UnitScript>()._isActionDone && !_selectedUnit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre))
                    {
                        Debug.Log(_selectedUnit);
                        _selected = true;
                     
                        GetStats();
                           UpdateJauge(tileId);
                        StartAttack(tileSelected.GetComponent<TileScript>().TileId, _selectedUnit.GetComponent<UnitScript>().AttackRange + _selectedUnit.GetComponent<UnitScript>().AttackRangeBonus);
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
    }
    [Header("Jauge d'attaque")]
    [SerializeField] public AttaqueUI1 _JaugeAttack;
    public void UpdateJauge(int TileId = -1)
    {
        if (TileId != -1)
        {
            if (TilesManager.Instance.TileList[TileId].TryGetComponent(out TileScript u) && u.Unit != null)
            {
                if (GameManager.Instance.IsPlayerRedTurn && u.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy || (GameManager.Instance.IsPlayerRedTurn && !u.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && u.Unit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)))
                {
                    _JaugeAttack.SynchAttackBorne(u.Unit.GetComponent<UnitScript>());
                }
                else if (!GameManager.Instance.IsPlayerRedTurn && !u.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy || (!GameManager.Instance.IsPlayerRedTurn && !u.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy && u.Unit.GetComponent<UnitScript>().UnitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)))
                {
                    _JaugeAttack.SynchAttackBorne(u.Unit.GetComponent<UnitScript>());
                }
            }
        }
    }
    /// <summary>
    /// PrÃ©pare l'Highlight des tiles ciblables & passe le statut de l'unitÃ© en -> _isInAttack
    /// </summary>
    /// <param name="tileId"></param>
    /// <param name="Range"></param>
    List<int> ID = new List<int>();
    public void StartAttack(int tileId, int Range)
    {
        if (!_isInAttack)
        {
            _isInAttack = true;

                PanelBlockant1.SetActive(true);
            
                PanelBlockant2.SetActive(true);
            
            ID = new List<int>();
            ID.Add(tileId);

            // Lance l'highlight des cases dans la range de l'unitÃ©.
            UpdateJauge(tileId);
            int Range2 = Range;
            foreach (MYthsAndSteel_Enum.TerrainType T1 in TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().TerrainEffectList)
            {
                foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                    {
                        if (T1 == T2)
                        {
                            if (Type.Child != null)
                            {
                                if (Type.MustBeInstantiate)
                                {
                                    foreach (GameObject G in TilesManager.Instance.TileList[tileId].GetComponent<TileScript>()._Child)
                                    {
                                        if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                        {
                                            if (Try2.Type == T1)
                                            {
                                                if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                {
                                                    Range2 += Try3.AttackRangeValue(0);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                    {
                                        Range2 += Try.AttackRangeValue(0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Highlight(tileId, tileId, Range2);
            
        }
    }

    [SerializeField] private bool _attackselected = false;
    public bool attackselected
    {
        get
        {
            return _attackselected;
        }
        set
        {
            _attackselected = value;
            CapacitySystem.Instance.Updatebutton();
        }
    }
    /// <summary>
    /// Ajout une case d'attaque à la liste
    /// </summary>
    /// <param name="tileId"></param>
    public void AddTileToList(int tileId){
        if(!_selectedTiles.Contains(tileId))
        {
            if(_selectedTiles.Count < numberOfTileToSelect && newNeighbourId.Contains(tileId))
            {
                TileScript currentTileScript = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>();
                if (currentTileScript.Unit != null  )
                {
                if(currentTileScript.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy != GameManager.Instance.IsPlayerRedTurn)
                    {
                        foreach (MYthsAndSteel_Enum.TerrainType T1 in TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().TerrainEffectList)
                        {
                            foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                            {
                                foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                                {
                                    if (T1 == T2)
                                    {
                                        if (Type.Child != null)
                                        {
                                            Debug.Log(Type._terrainName);
                                            if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                            {
                                                Try.CibledByAttack(RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>(), TilesManager.Instance.TileList[RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        attackselected = true;
                        _selectedTiles.Add(tileId);
                        TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _selectedSprite, 1);
                    }
                }
            }
        }
        else
        {
            attackselected = false;
            RemoveTileFromList(tileId);
        }

    }

    /// <summary>
    /// Retire une case de la liste des cases sélectionnées
    /// </summary>
    /// <param name="tileId"></param>
    void RemoveTileFromList(int tileId)
    {
        foreach (MYthsAndSteel_Enum.TerrainType T1 in TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().TerrainEffectList)
        {
            foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
            {
                foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                {
                    if (T1 == T2)
                    {
                        if (Type.Child != null)
                        {
                            Debug.Log(Type._terrainName);
                            if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                            {
                                Try.UnCibledByAttack(RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>());
                            }
                        }
                    }
                }
            }
        }
        _selectedTiles.Remove(tileId);
        TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _normalAttackSprite, 1);
    }

    /// <summary>
    /// ArrÃªte l'attaque de l'unitÃ© select (UI + possibilitÃ© d'attaquer
    /// </summary>
    public void StopAttack()
    {
        attackselected = false;
        attackselected = false;
        selectedUnitEnnemy = null;
        _isInAttack = false;
        _selected = false;
        PanelBlockant1.SetActive(false);

        PanelBlockant2.SetActive(false);
        RemoveTileSprite();

        // Clear de toutes les listes et stats
        newNeighbourId.Clear();

        DiceResult = 0;
        firstDiceInt = 0;
        secondDiceInt = 0;
        _attackRange = 0;
        _damageMinimum = 0;
        _damageMaximum = 0;
        _numberRangeMin.x = 0;
        _numberRangeMin.y = 0;
        _numberRangeMax.x = 0;
        _numberRangeMax.y = 0;
        
  
   
        RaycastManager.Instance.ActualTileSelected = null;
    }

    /// <summary>
    /// Supprime l'effet de case
    /// </summary>
    /// <param name="WithoutSelected"></param>
    public void RemoveTileSprite(bool WithoutSelected = false){
        if(!WithoutSelected)
        {
            foreach(int Neighbour in newNeighbourId) // Supprime toutes les tiles.
            {
                if(TilesManager.Instance.TileList[Neighbour] != null)
                {
                    TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect);
                }
            }
        }
        else
        {
            foreach(int Neighbour in newNeighbourId) // Supprime toutes les tiles.
            {
                if(TilesManager.Instance.TileList[Neighbour] != null && !_selectedTiles.Contains(Neighbour))
                {
                    TilesManager.Instance.TileList[Neighbour].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect);
                }
            }
        }
          
    }

    /// <summary>
    /// Lance l'attaque de l'unité
    /// </summary>
    public void Attack()
    {
        if (_isInAttack)
        {
            if (_selectedTiles.Count != 0)
            {
                ApplyAttack();
                foreach(int i in _selectedTiles)
                {
                    selectedUnitEnnemy = TilesManager.Instance.TileList[i].GetComponent<TileScript>().Unit;
                    if(selectedUnitEnnemy != null)
                    {
                        ChooseAttackType(_numberRangeMin, _damageMinimum, _numberRangeMax, _damageMaximum, DiceResult);
                    }
                    else
                    {
                        StopAttack();
                    }
                }
            }
            else
            {
                StopAttack();
            }
        }
        else
        {
            StopAttack();
        }
    }

    /// <summary>
    /// obtient les stats de l'unité sélectionnée
    /// </summary>
    public void GetStats()
    {
        _attackRange = _selectedUnit.GetComponent<UnitScript>().AttackRange; // Récupération de la Portée        
        _damageMinimum = _selectedUnit.GetComponent<UnitScript>().DamageMinimum; // Récupération des Dégats Maximum
        _damageMaximum = _selectedUnit.GetComponent<UnitScript>().DamageMaximum; // Dégats Minimums
        _numberRangeMin.x = _selectedUnit.GetComponent<UnitScript>().NumberRangeMin.x; // Récupération de la Range min - x
        _numberRangeMin.y = _selectedUnit.GetComponent<UnitScript>().NumberRangeMin.y; // Récupération de la Range min - y 
        _numberRangeMax.x = _selectedUnit.GetComponent<UnitScript>().NumberRangeMax.x; // Récupération de la Range min - x
        _numberRangeMax.y = _selectedUnit.GetComponent<UnitScript>().NumberRangeMax.y; // Récupération de la Range min - y
        numberOfTileToSelect = _selectedUnit.GetComponent<UnitScript>().UnitSO.numberOfUnitToAttack;
        _isAttackDeviation = false;
        

        if (!_isAttackDeviation)
        foreach (MYthsAndSteel_Enum.Attributs element in _selectedUnit.GetComponent<UnitScript>().UnitSO.UnitAttributs)
        {
            if(element == MYthsAndSteel_Enum.Attributs.Déviation)
                {
                    _isAttackDeviation = true;
                }
        }
            
     
    }

    /// <summary>
    /// Appliques toutes les stats pour obtenir les dégâts
    /// </summary>
    public void ApplyAttack()
    {
        Randomdice();
        CapacitySystem.Instance.Updatebutton();
        IsInAttack = false;
        _selectedUnit.GetComponent<UnitScript>()._isActionDone = true;
        
        _selectedUnit.GetComponent<UnitScript>().checkActivation();
        _selectedUnit.GetComponent<UnitScript>().checkMovementLeft(); 
    }

    /// <summary>
    /// Change les stats en fonction de où se trouve l'unité
    /// </summary>
    public void ChangeStat()
    {
        // Applique les bonus/malus de terrains
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Bosquet, _selectedUnit.GetComponent<UnitScript>().ActualTiledId)) // Bosquet
        {
            _numberRangeMin.x -= 1;
            _numberRangeMin.y -= 1;
            _numberRangeMax.x -= 1;
            Debug.Log("BosquetEffectApplyed");
        }



        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Plage, _selectedUnit.GetComponent<UnitScript>().ActualTiledId) && _selectedUnit.GetComponent<Unit_SO>().typeUnite == MYthsAndSteel_Enum.TypeUnite.Infanterie) // Plage
        {
            _numberRangeMin.x += -2;
            _numberRangeMin.y += -1;
            _numberRangeMax.x += -1;
            Debug.Log("PlayaEffectApplyed");
        }


        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId)) // Haute colline 2
        {
            if (!PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, _selectedUnit.GetComponent<UnitScript>().ActualTiledId) || !PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, _selectedUnit.GetComponent<UnitScript>().ActualTiledId))
            {
                _numberRangeMin.x += 2;
                _numberRangeMin.y += 2;
                _numberRangeMax.x += 2;
                Debug.Log("HautecollinesamerelapEffectApplyed");

            }
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Maison, selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId)) // Maison
        {
            _damageMinimum -= 1;
            _damageMaximum -= 1;
            Debug.Log("Dégats Reduits");
            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().RemoveEffect(MYthsAndSteel_Enum.TerrainType.Maison);
            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().CreateEffect(MYthsAndSteel_Enum.TerrainType.Ruines);
            Debug.Log("IkeaEffectApplyed");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Immeuble, selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId)) // Immeubles
        {
            _damageMinimum = 0;
            _damageMaximum = 0;
            Debug.Log("Annulés");

            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().RemoveEffect(MYthsAndSteel_Enum.TerrainType.Immeuble);
            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().CreateEffect(MYthsAndSteel_Enum.TerrainType.Ruines);
            Debug.Log("BigBoumIkeaEffectApplyed");

        }
    }

    /// <summary>
    /// Initialisation de la déviation : on va récupérer quelques variables et illuminer les cases concernées par la déviation 
    /// </summary>
    public void StartDeviation()
    {
        foreach(int i in _selectedTiles){
            TilesManager.Instance.TileList[i].gameObject.GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect);
        }

        //récupération de variable
        int ennemyUnitIdTile = selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId;
        bool endDeviation = false;

        SetEnnemyUnitListTileNeighbourDiagUI = PlayerStatic.GetNeighbourDiag(ennemyUnitIdTile, TilesManager.Instance.TileList[ennemyUnitIdTile].GetComponent<TileScript>().Line, IsAttackDeviation);

        List<int> ennemyUnitListTileNeighbourDiagUI = new List<int>();
        SetEnnemyUnitListTileNeighbourDiagUI.Add(ennemyUnitIdTile);
        ennemyUnitListTileNeighbourDiagUI.AddRange(SetEnnemyUnitListTileNeighbourDiagUI);
        ennemyUnitListTileNeighbourDiagUI.Sort();

        //Illumination des cases
        foreach(int id in SetEnnemyUnitListTileNeighbourDiagUI)
        {
            TilesManager.Instance.TileList[id].GetComponent<SpriteRenderer>().sprite = _DeviationSelectUI;
            TilesManager.Instance.TileList[id].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        //Lancement de l'animation Les coroutines ne se lancent pas les une après les autres c'est pour cela qu'il y a un delay qui va s'incrémenter au fur et à mesure de for 
        int x = 0;
        for(int i = 0; i < ennemyUnitListTileNeighbourDiagUI.Count; i++, x++)
        {
            StartCoroutine(ColorTile(i, x, ennemyUnitListTileNeighbourDiagUI, ennemyUnitIdTile, endDeviation));
        }
    }

    /// <summary>
    /// L'animation de la déviation où un sprite rouge va "se déplacer" de case en case. Cette animation dépend de la taille d'une liste choisi. 
    /// </summary>
    IEnumerator ColorTile(int id, int delay, List<int> listTile, int ennemyIDTile, bool endDeviation)
    {
        float z = (float)delay;
        int idMax =  listTile.Count-1;
        //Si la case actuel n'est pas la première case de la liste alors la précédente case a son sprite qui devient bleu et l'actuelle qui devient un sprite rouge
        if(id != 0)
        {
            yield return new WaitForSeconds(z / 3);
            TilesManager.Instance.TileList[listTile[id - 1]].GetComponent<SpriteRenderer>().sprite = DeviationSelectUI;
            TilesManager.Instance.TileList[listTile[id]].GetComponent<SpriteRenderer>().sprite = _selectedSprite;
            //Si on est à la fin de la list et qu'on est a la première animation appel la fonction RandomCase.
            if(id == idMax && endDeviation == false)
            {
                yield return new WaitForSeconds(z / 20);
                TilesManager.Instance.TileList[listTile[id]].GetComponent<SpriteRenderer>().sprite = DeviationSelectUI;

                RandomCase(listTile, ennemyIDTile, endDeviation);
            }
            //Si on est à la fin de la list et qu'on est a la deusième animation appel la fonction ApplyDeviation.
            else if(id == idMax && endDeviation == true)
            {
                yield return new WaitForSeconds(z / 2);
                TilesManager.Instance.TileList[listTile[id]].GetComponent<SpriteRenderer>().sprite = DeviationSelectUI;

                ApplyDeviation();
            }
        }
        //Si la case actuel est la première case alors son sprite bleu devient un sprite rouge.
        else
        {
            TilesManager.Instance.TileList[listTile[id]].GetComponent<SpriteRenderer>().sprite = _selectedSprite;
            if(id == idMax && endDeviation == true)
            {
                yield return new WaitForSeconds(1f);
                TilesManager.Instance.TileList[listTile[id]].GetComponent<SpriteRenderer>().sprite = DeviationSelectUI;

                ApplyDeviation();
            }

        }
    }

    /// <summary>
    /// Cette fonction va déterminer la case ou l'attaque y sera devié et adapte la taille de la liste pour que l'id de cette case soit la dernière de la liste.
    /// </summary>
    void RandomCase(List<int> listIdUI, int ennemyUnitIDTile, bool endDeviation)
    {
        int x = 0;
        int indexEnnemyUnitIdTile = listIdUI.IndexOf(ennemyUnitIDTile);
        // On rajoute la case où il y a l'unité visé car l'attaque 2/10 d'être dévié sur la case où se trouve l'unité visée.  
        listIdUI.Add(ennemyUnitIDTile);


        //On tire au hasard une tile de la list et on applique les dégâts minimums si il y a une unité sur la tile.
        int DeviationIdTileIndex = Random.Range(0, listIdUI.Count);

        int DeviationIdTile = listIdUI[DeviationIdTileIndex];
        selectedUnitEnnemy = TilesManager.Instance.TileList[DeviationIdTile].GetComponent<TileScript>().Unit;
        //Si lors du random on tombe sur l'idée case de l'unité visée rajouté précédemment on la rapporte à son valeur correspondante. 
        if(DeviationIdTileIndex == listIdUI.Count - 1)
        {
            DeviationIdTileIndex = indexEnnemyUnitIdTile;

        }
        listIdUI.Remove(listIdUI[listIdUI.Count - 1]);
        listIdUI.Sort();
        //On redimensionne la list pour que l'id de la case où l'attaque est dévié soit la dernière de la list  
        while(listIdUI.Count > DeviationIdTileIndex + 1)
        {
            listIdUI.Remove(listIdUI[listIdUI.Count - 1]);
        }
        endDeviation = true;
        // On lance la deuxsième animation
        for(int i = 0; i < listIdUI.Count; i++, x++)
        {
            StartCoroutine(ColorTile(i, x, listIdUI, ennemyUnitIDTile, endDeviation));
        }
        Debug.Log(DeviationIdTile);
    }

    /// <summary>
    /// Cette fonction va appliquer les dégats de la déviation si il y a une unité sur la case et va reset le SpriteRenderer des cases.
    /// </summary>
    void ApplyDeviation()
    {
        foreach(int item in SetEnnemyUnitListTileNeighbourDiagUI)
        {
            TilesManager.Instance.TileList[item].GetComponent<SpriteRenderer>().sprite = DeviationOriginalSpriteCase;
            TilesManager.Instance.TileList[item].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.19f);


        }
        if(selectedUnitEnnemy == null)

        {
            SoundController.Instance.PlaySound(_selectedUnit.GetComponent<UnitScript>().SonAttaque);
            Debug.Log("Damage : " + null);
            StopAttack();

        }
        else
        {
            int AttackVariation = 0;
            TileScript T = TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>();
            foreach (MYthsAndSteel_Enum.TerrainType T1 in T.TerrainEffectList)
            {
                foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                    {
                        if (T1 == T2)
                        {
                            if (Type.Child != null)
                            {
                                if (Type.MustBeInstantiate)
                                {
                                    foreach (GameObject G in T._Child)
                                    {
                                        if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                        {
                                            if (Try2.Type == T1)
                                            {
                                                if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                {
                                                    AttackVariation += Try3.AttackApply(_damageMinimum);
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                    {
                                        AttackVariation += Try.AttackApply(_damageMinimum);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Debug.Log(AttackVariation);
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum + AttackVariation);
            SoundController.Instance.PlaySound(_selectedUnit.GetComponent<UnitScript>().SonAttaque);
            Debug.Log("Damage : " + _damageMinimum);
            StopAttack();
        }
    }
}