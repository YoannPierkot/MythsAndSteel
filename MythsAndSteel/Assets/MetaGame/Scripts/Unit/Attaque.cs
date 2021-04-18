using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attaque : MonoSingleton<Attaque>
{
    #region Variables
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    public List<int> _newNeighbourId => newNeighbourId;
    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unitÃ©.

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
    [SerializeField] private List<int> _selectedTiles = new List<int>();
    public List<int> SelectedTiles => _selectedTiles;

    GameObject _selectedUnit = null;

    int numberOfTileToSelect = 0;

    [SerializeField] GameObject selectedUnitEnnemy;

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

        //DiceResult = firstDiceInt + secondDiceInt + selectedUnit.GetComponent<UnitScript>().DiceBonus;
        DiceResult = firstDiceInt + secondDiceInt;

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
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum);
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (DiceResult < _numberRangeMin.x)
        {
            ChangeStat();
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0);
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
            AnimationUpdate();
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMinimum);
            Debug.Log("Damage : " + _damageMinimum);
        }
        if (DiceResult >= _numberRangeMax.x && DiceResult <= _numberRangeMax.y)
        {
            ChangeStat();
            AnimationUpdate();
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(_damageMaximum);
            Debug.Log("Damage : " + _damageMaximum);
        }
        if (DiceResult < _numberRangeMin.x)
        {
            ChangeStat();
            selectedUnitEnnemy.GetComponent<UnitScript>().TakeDamage(0);
            Debug.Log("Damage : " + null);
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
    public void Highlight(int tileId, int Range)
    {
        if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                TileScript TileSc = TilesManager.Instance.TileList[ID].GetComponent<TileScript>();
                bool i = false;

                if (TileSc.Unit != null)
                {
                    if (GameManager.Instance.IsPlayerRedTurn == TileSc.Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy)
                    {
                        i = true;
                    }
                }

                if (!i)
                {
                    TileSc.ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _normalAttackSprite, .275f);
                    if (!newNeighbourId.Contains(ID))
                    {
                        newNeighbourId.Add(ID);
                    }
                    Highlight(ID, Range - 1);
                }
            }
        }
    }

    /// <summary>
    /// Ajoute plus d'alÃ©atoire aux lancÃ©s de dÃ©
    /// </summary>
    private void RandomMore()
    {
        if ((GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.RedPlayerInfos.dontTouchThis) ||
        (!GameManager.Instance.IsPlayerRedTurn && PlayerScript.Instance.BluePlayerInfos.dontTouchThis))
        {
            DiceResult += 3;
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
        if(tileId != -1)
        {
            if(!_selectedUnit.GetComponent<UnitScript>()._isActionDone)
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
            GameObject tileSelected = RaycastManager.Instance.ActualTileSelected;

            if(tileSelected != null)
            {
                _selectedUnit = tileSelected.GetComponent<TileScript>().Unit;
                if(!_selectedUnit.GetComponent<UnitScript>()._isActionDone)
                {
                    Debug.Log(_selectedUnit);
                    _selected = true;
                    GetStats();
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

    /// <summary>
    /// PrÃ©pare l'Highlight des tiles ciblables & passe le statut de l'unitÃ© en -> _isInAttack
    /// </summary>
    /// <param name="tileId"></param>
    /// <param name="Range"></param>
    public void StartAttack(int tileId, int Range)
    {
        if (!_isInAttack)
        {
            _isInAttack = true;
            List<int> ID = new List<int>();
            ID.Add(tileId);

            // Lance l'highlight des cases dans la range de l'unitÃ©.
            Highlight(tileId, Range); 
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
                _selectedTiles.Add(tileId);
                TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _selectedSprite, 1);
            }
        }
        else
        {
            RemoveTileFromList(tileId);
        }

    }

    /// <summary>
    /// Retire une case de la liste des cases sélectionnées
    /// </summary>
    /// <param name="tileId"></param>
    void RemoveTileFromList(int tileId){
        _selectedTiles.Remove(tileId);
        TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().ActiveChildObj(MYthsAndSteel_Enum.ChildTileType.AttackSelect, _normalAttackSprite, .275f);
    }

    /// <summary>
    /// ArrÃªte l'attaque de l'unitÃ© select (UI + possibilitÃ© d'attaquer
    /// </summary>
    public void StopAttack()
    {
        RemoveTileSprite();

        // Clear de toutes les listes et stats
        newNeighbourId.Clear();

        _isInAttack = false;
        _selected = false;

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
            _selectedTiles.Clear();
            _newNeighbourId.Clear();
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

                StopAttack();
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
    }

    /// <summary>
    /// Appliques toutes les stats pour obtenir les dégâts
    /// </summary>
    public void ApplyAttack()
    {
        Randomdice();
        IsInAttack = false;
        _selectedUnit.GetComponent<UnitScript>()._isActionDone = true;
        _selectedUnit.GetComponent<UnitScript>().checkActivation();
    }

    /// <summary>
    /// Change les stats en fonction de où se trouve l'unité
    /// </summary>
    public void ChangeStat()
    {
        // Applique les bonus/malus de terrains
        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Bosquet, _selectedUnit.GetComponent<UnitScript>().ActualTiledId)) // Bosquet
        {
            _numberRangeMin.x += 1;
            _numberRangeMin.y += 1;
            _numberRangeMax.x += 1;
            Debug.Log("BosquetEffectApplyed");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Colline, _selectedUnit.GetComponent<UnitScript>().ActualTiledId)) // Colline
        {
            _selectedUnit.GetComponent<UnitScript>().AttackRangeBonus = 1;
            Debug.Log("CollineEffectApplyed");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Plage, _selectedUnit.GetComponent<UnitScript>().ActualTiledId) && _selectedUnit.GetComponent<Unit_SO>().typeUnite == MYthsAndSteel_Enum.TypeUnite.Infanterie) // Plage
        {
            _numberRangeMin.x += -2;
            _numberRangeMin.y += -1;
            _numberRangeMax.x += -1;
            Debug.Log("PlayaEffectApplyed");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Haute_colline, _selectedUnit.GetComponent<UnitScript>().ActualTiledId)) // Haute colline 1
        {
            _selectedUnit.GetComponent<UnitScript>().AttackRangeBonus = 1;
            Debug.Log("Haute collineEffectApplyed");
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
            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Maison);
            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Ruines);
            Debug.Log("IkeaEffectApplyed");
        }

        if (PlayerStatic.CheckTiles(MYthsAndSteel_Enum.TerrainType.Immeuble, selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId)) // Immeubles
        {
            _damageMinimum = 0;
            _damageMaximum = 0;
            Debug.Log("Annulés");
            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Immeuble);
            TilesManager.Instance.TileList[selectedUnitEnnemy.GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Ruines);
            Debug.Log("BigBoumIkeaEffectApplyed");
        }
    }
}