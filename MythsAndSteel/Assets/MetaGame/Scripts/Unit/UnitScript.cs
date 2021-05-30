using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyButtons;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitScript : MonoBehaviour
{
    #region Variables
    [Header("--------------- STATS DE BASE DE L'UNITE ---------------")]
    //Scriptable qui contient les stats de base de l'unité
    [SerializeField] Unit_SO _unitSO;

    public int ParalysieStat = 3;
    public Unit_SO UnitSO
    {
        get
        {
            return _unitSO;
        }
        set
        {
            _unitSO = value;
        }
    }

public bool MélodieSinistre = false;
    [Header("------------------- VIE -------------------")]
    [Header("------------------- STAT EN JEU -------------------")]
    //Vie actuelle
    [SerializeField] int _life;
    public int Life
    {
        get
        {
            return _life;
        }
        set
        {
            _life = value;
        }
    }
    [SerializeField]
    SpriteRenderer Renderer;
    bool IsDead = false;
    bool IsDeadByOrgone = false;

    // Bouclier actuelle
    [SerializeField] int _shield;
    public int Shield => _shield;

    //UI de la vie de l'unité
    SpriteRenderer CurrentSpriteLifeHeartUI;

    [Header("-------------------- ATTAQUE -------------------")]
    //Portée
    [SerializeField] int _attackRange;
    public int AttackRange => _attackRange;
    public int AttackRangeBonus = 0;

    [Space]
    //Dégats minimum infligé
    [SerializeField] Vector2 _numberRangeMin;
    public Vector2 NumberRangeMin => _numberRangeMin;
    [SerializeField] int _damageMinimum;
    public int DamageMinimum => _damageMinimum;

    [Space]
    //Dégats maximum infligé
    [SerializeField] Vector2 _numberRangeMax;
    public Vector2 NumberRangeMax => _numberRangeMax;
    [SerializeField] int _damageMaximum;
    public int DamageMaximum => _damageMaximum;

    //Dégât bonus
    [SerializeField] int _damageBonus;
    public int DamageBonus => _damageBonus;

    //Bonus aux lancés de dé
    [SerializeField] public int _diceBonus = 0;
    public int DiceBonus
    {
        get
        {
            return _diceBonus;
        }
        set
        {
            _diceBonus = value;
        }
    }

    public bool RunningCapacity = false;

    [Header("------------------- MOUVEMENT -------------------")]
    //Vitesse de déplacement
    [SerializeField] int _moveSpeed;
    public int MoveSpeed => _moveSpeed;
    public int _MoveSpeedBonus = 0;
    public int MoveSpeedBonus
    {
        get
        {
            return _MoveSpeedBonus;
        }
        set
        {
            _MoveSpeedBonus = value;
        }
    }
    public bool BonusUsed = false;
    // Déplacement réstant de l'unité durant cette activation
    [SerializeField] int _moveLeft;
    public int MoveLeft
    {
        get
        {
            return _moveLeft;
        }
        set
        {
            _moveLeft = value;
        }
    }

    [Header("------------------- COUT DE CREATION -------------------")]
    // Coût de création
    [SerializeField] int _creationCost;
    public int CreationCost => _creationCost;

    [Header("----------------------- SOUND -----------------------------")]

    [SerializeField] AudioClip _SonAttaque;
    public AudioClip SonAttaque => _SonAttaque;

    [SerializeField] AudioClip _SonDeplacement;
    public AudioClip SonDeplacement => _SonDeplacement;

    [SerializeField] AudioClip _SonMort;
    public AudioClip SonMort => _SonMort;

    private AudioSource _SourceAudio;
    public AudioSource SourceAudio => _SourceAudio;

    [Header("------------------- CASE DE L'UNITE -------------------")]
    //Valeur (id) de la case sur laquelle se trouve l'unité
    [SerializeField] int _actualTileld;
    public int ActualTiledId
    {
        get
        {
            return _actualTileld;
        }
        set
        {
            if (_actualTileld != value)
            {
                LastTileId = _actualTileld;
                _actualTileld = value;
            }
        }
    }
    public int LastTileId;

    [HideInInspector] int lastTileId = 0;

#if UNITY_EDITOR
    public void AddTileUnderUnit()
    {
        if (lastTileId != ActualTiledId)
        {
            FindObjectOfType<TilesManager>().TileList[lastTileId].GetComponent<TileScript>().RemoveUnitFromTile();
            lastTileId = ActualTiledId;
        }

        FindObjectOfType<TilesManager>().TileList[_actualTileld].GetComponent<TileScript>().AddUnitToTile(this.gameObject, true);
    }
#endif


    //déplacement actuel de l'unité pour la fonction "MoveWithPath"
    int _i;
    public int i => _i;

    [Header("------------------- ACTIVATION UNITE -------------------")]
    //A commencer à se déplacer
    [SerializeField] private bool hasStartMove = false;
    public bool _hasStartMove
    {
        get
        {
            return hasStartMove;
        }
        set
        {
            hasStartMove = value;
        }
    }

    //lorsque le joueur a fini d'utiliser tous ses points de déplacement
    [SerializeField] bool _isMoveDone;
    public bool IsMoveDone => _isMoveDone;

    //lorsque le joueur a effectué soit une attaque soit un pouvoir actif
    public bool _isActionDone;

    //lorsque l'activation a totalement été finie
    [SerializeField] bool _isActivationDone;
    public bool IsActivationDone => _isActivationDone;

    //Chemin que l'unité va emprunter
    List<int> _pathtomake;
    public List<int> Pathtomake => _pathtomake;

    [Header("------------------- STATU DE L'UNITE -------------------")]
    //Statut que possède l'unité
    [SerializeField] private List<MYthsAndSteel_Enum.UnitStatut> _unitStatuts = new List<MYthsAndSteel_Enum.UnitStatut>();
    public List<MYthsAndSteel_Enum.UnitStatut> UnitStatuts
    {
        get
        {
            return _unitStatuts;
        }
        set
        { 
            _unitStatuts = value;
        }
    }

    [SerializeField] private Animator StatusPrefab;
    [SerializeField] private ScriptableStatus StatusData;

    public int step = 0;
    Coroutine last;
    IEnumerator NextStep()
    {
        if(StatusPrefab.GetComponent<RuntimeAnimatorController>() != null)
        {

        if (StatusPrefab.GetBool("In"))
        {
            StatusPrefab.SetBool("In", false);
            yield return new WaitForSeconds(1f);
        }
        }
        if(UnitStatuts.Count >= step && step >= 0)
        {
            StatusPrefab.runtimeAnimatorController = GetAnimator(_unitStatuts[step]);
        }
        StatusPrefab.SetBool("In", true);
        yield return new WaitForSeconds(3);
        if(UnitStatuts.Count > 1)
        {
            NewStep();
        }
        if (UnitStatuts.Count == 0)
        {
            StatusPrefab.runtimeAnimatorController = null;
            StatusPrefab.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void NewStep()
    {
        if(step >= UnitStatuts.Count - 1)
        {
            step = 0;
        }
        else if(UnitStatuts.Count != 0)
        {
            step++;
        }
        last = StartCoroutine(NextStep());
    }

    public RuntimeAnimatorController GetAnimator(MYthsAndSteel_Enum.UnitStatut ST)
    {
        foreach(Data D in StatusData.Data)
        {
            if(D.Status == ST)
            {
                if(D.Animation != null)
                {
                    return D.Animation;
                }
                else
                {
                    Debug.LogError("Miss animation");
                }
            }
        }
        Debug.LogError("Miss animation");
        return null;
    }

    public void NextStepSetter(MYthsAndSteel_Enum.UnitStatut ST)
    {
        if(last != null)
        {
            StopCoroutine(last);
        }
        step = UnitStatuts.IndexOf(ST);
        last = StartCoroutine(NextStep());
    }

    public bool hasUseActivation = false;
    [SerializeField] private Animator _Animation;
    public Animator Animation => _Animation;

    //Récupération de stats pour l'écran de victoire
 
    public bool GotCapacity()
    {
        if(TryGetComponent<Capacity>(out Capacity C))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #endregion Variables

    private void Start()
    {

       
        LastTileId = ActualTiledId;
        UpdateUnitStat();

        // On instancie l'object qui possède le sprite correspondant à l'UI au point de vie et de bouclier de l'unité.
        GameObject LifeHeartUI = Instantiate(UIInstance.Instance.LifeHeartPrefab, gameObject.transform);


        CurrentSpriteLifeHeartUI = LifeHeartUI.GetComponent<SpriteRenderer>();
        if (_shield > 0)
        {
            if (UnitSO.IsInRedArmy)
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartShieldSprite, _life + _shield);
            }
            else
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartShieldSprite, _life + _shield);
            }

        }
        else
        {
            if (UnitSO.IsInRedArmy)
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartSprite, _life);
            }
            else
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartSprite, _life);
            }
        }
    }

    #region LifeMethods
    /// <summary>
    /// Rajoute de la vie au joueur
    /// </summary>
    /// <param name="Lifeadd"></param>
    public virtual void GiveLife(int Lifeadd)
    {
        _life += Lifeadd;
        if (_shield > 0)
        {
            if (UnitSO.IsInRedArmy)
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartShieldSprite, _life + _shield);
            }
            else
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartShieldSprite, _life + _shield);
            }
        }
        else
        {
            if (UnitSO.IsInRedArmy)
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartSprite, _life);
            }
            else
            {
                UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartSprite, _life);
            }
        }

        if (_life > UnitSO.LifeMax)
        {
            int shieldPlus = _life - UnitSO.LifeMax;
            _life = UnitSO.LifeMax;
            _shield += shieldPlus;
            if (_shield > 0)
            {
                if (UnitSO.IsInRedArmy)
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartShieldSprite, _life + _shield);
                }
                else
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartShieldSprite, _life + _shield);
                }
            }
            else
            {
                if (UnitSO.IsInRedArmy)
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartSprite, _life);
                }
                else
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartSprite, _life);
                }
            }
        }
    }

    /// <summary>
    /// Fait perdre de la vie au joueur
    /// </summary>
    /// <param name="Damage"></param>
    public virtual void TakeDamage(int Damage, bool IsOrgoneDamage = false)
    {
        if(!_unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Invincible))
            {

        if (_shield > 0)
        {
            _shield -= Damage;

            if (_shield < 0)
            {
                _life += _shield;
            }

            if (_shield > 0)
            {
                if (UnitSO.IsInRedArmy)
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartShieldSprite, _life + _shield);
                        Renderer.material.SetFloat("_HitTime", Time.time);
                       
                    }
                else
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartShieldSprite, _life + _shield);
                        Renderer.material.SetFloat("_HitTime", Time.time);
                       
                    }
            }
            else
            {
                if (UnitSO.IsInRedArmy)
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartSprite, _life);
                        Renderer.material.SetFloat("_HitTime", Time.time);
                     
                    }
                else
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartSprite, _life);
                        Renderer.material.SetFloat("_HitTime", Time.time);
                 
                    }
            }
        }
        else
        {
            _life -= Damage;
            if (_shield > 0)
            {
                if (UnitSO.IsInRedArmy)
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartShieldSprite, _life + _shield);
                        Renderer.material.SetFloat("_HitTime", Time.time);
                        Debug.Log("je ne fonctionne pas ");
                    }
                else
                {
                    UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartShieldSprite, _life + _shield);
                        Renderer.material.SetFloat("_HitTime", Time.time);
                   
                    }
            }
            else
            {
                if (_life > 0)
                {
                    if (UnitSO.IsInRedArmy)
                    {
                        UpdateLifeHeartShieldUI(UIInstance.Instance.RedHeartSprite, _life);
                            Renderer.material.SetFloat("_HitTime", Time.time);
                           
                        }
                    else
                    {
                        UpdateLifeHeartShieldUI(UIInstance.Instance.BlueHeartSprite, _life);
                            Renderer.material.SetFloat("_HitTime", Time.time);
                        
                        }
                }
            }
        }
      
            CheckLife();

        //Ajout de l'orgone
        
        if (Damage > 0)
        {
             
            if (TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneRed))
            {
                if (!GameManager.Instance.IsCheckingOrgone)
                {

                    PlayerScript.Instance.AddOrgone(1, 1);
                        FxOrgoneSpawn(true);
                        PlayerScript.Instance.RedPlayerInfos.CheckOrgone(1);
                }
                else
                {
                    if (!IsOrgoneDamage)
                    {
                        PlayerScript.Instance.AddOrgone(1, 1);
                            FxOrgoneSpawn(true);

                            Debug.Log("Ca buuuuuug");
                    }
                    if (GameManager.Instance._waitToCheckOrgone != null) GameManager.Instance._waitToCheckOrgone += AddOrgoneToPlayer;
                }
            }

            if (TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneBlue))
            {
                if (!GameManager.Instance.IsCheckingOrgone)
                {
                    PlayerScript.Instance.AddOrgone(1, 2);
                        FxOrgoneSpawn(false);




                        PlayerScript.Instance.BluePlayerInfos.CheckOrgone(2);
                }
                else
                {
                    if (!IsOrgoneDamage)
                    {
                        PlayerScript.Instance.AddOrgone(1, 2);

                            FxOrgoneSpawn(false);
                            Debug.Log("Ca buuuuuug");
                    }
                    if (GameManager.Instance._waitToCheckOrgone != null) GameManager.Instance._waitToCheckOrgone += AddOrgoneToPlayer;
                }
            }
        }
    }
        }

    /// <summary>
    /// Check si l'orgone a redépassé le joueur
    /// </summary>
    void AddOrgoneToPlayer()
    {
        PlayerScript.Instance.RedPlayerInfos.CheckOrgone(1);
        PlayerScript.Instance.BluePlayerInfos.CheckOrgone(2);

        GameManager.Instance._waitToCheckOrgone = null;
    }

    /// <summary>
    /// Check la vie du joueur
    /// </summary>
    void CheckLife()
    {

        if (_life <= 0 && !IsDead)
        {
            if (UnitSO.IsInRedArmy)
            {
               GameManager.Instance.victoryScreen.redDeadUnits += 1;
            }
            if (!UnitSO.IsInRedArmy)
            {
               GameManager.Instance.victoryScreen.blueDeadUnits += 1;
            }
            Death();
            IsDead = true;
        }
    }


    public void DieByOrgone()
    {
        IsDeadByOrgone = true;
    }
    /// <summary>
    /// Tue l'unité
    /// </summary>
    public virtual void Death()
    {

      
        if (RaycastManager.Instance.ActualUnitSelected == this.gameObject)
        {
            Mouvement.Instance.StopMouvement(false);
        }

        if (UnitSO.IsInRedArmy) PlayerScript.Instance.UnitRef.UnitListRedPlayer.Remove(gameObject);
        else PlayerScript.Instance.UnitRef.UnitListBluePlayer.Remove(gameObject);
        if (!IsDeadByOrgone)
        {
            if (TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneRed))
            {
                Debug.Log("doing orgone" + GameManager.Instance.DoingEpxlosionOrgone);
                PlayerScript.Instance.AddOrgone(1, 1);
                FxOrgoneSpawn(true);
                PlayerScript.Instance.RedPlayerInfos.CheckOrgone(1);
            }
            else if (TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.OrgoneBlue))
            {
                PlayerScript.Instance.AddOrgone(1, 2);
                FxOrgoneSpawn(false);
                PlayerScript.Instance.BluePlayerInfos.CheckOrgone(2);
            }
            else { }
        }
        else
        {
            Debug.Log("fdkj");
            TilesManager.Instance.TileList[ActualTiledId].GetComponent<TileScript>().DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType.EventSelect);
            GameManager.Instance.DeathByOrgone--;
        }

        PlayerScript.Instance.GiveEventCard(UnitSO.IsInRedArmy ? 1 : 2);
        IsDead = false;

        StartCoroutine(DeathAnimation());
    }

    /// <summary>
    /// Lance l'animation de mort et attend la fin de l'animation avant de détuire l'unité.
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathAnimation()
    {
        if (Animation != null)
        {
            
            Animation.SetBool("Dead", true); 

            yield return new WaitForSeconds(Animation.runtimeAnimatorController.animationClips[0].length);
        }

        Destroy(gameObject);
        SoundController.Instance.PlaySound(_SonMort);
    }

    /// <summary>
    /// Cette fonction va permettre de mettre un sprite d'une liste trouvé à partir d'un index à un objet. C'est la fonction qui met à jour l'affichage des boucliers et des points de vie. 
    /// </summary>
    public void UpdateLifeHeartShieldUI(Sprite[] listSprite, int life)
    {
        if(life >= 10)
        {
            life = 10;
        }
        CurrentSpriteLifeHeartUI.sprite = listSprite[life];
    }
    #endregion LifeMethods

    #region Statut

    public void test()
    {
        AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Immobilisation);
    }
    public void test2()
    {
        AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.ArmeEpidemiologique);
    }
    public void poss()
    {
        AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Possédé);
    }
    public void poss1()
    {
        AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasCombattre);
    }
    public void poss2()
    {
        AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.PeutPasPrendreDesObjectifs);
    }
    public void AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut stat)
    {
        if (!_unitStatuts.Contains(stat))
        {
            _unitStatuts.Add(stat);
            NextStepSetter(stat);
        }
    }
    public void RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut stat)
    {
        if (_unitStatuts.Contains(stat))
        {
            _unitStatuts.Remove(stat);
            NextStepSetter(stat);

        }
    }

    #endregion Statut

    #region ChangementStat
    public bool DoingCharg1Blue = false;
    /// <summary>
    /// Ajoute des dégâts supplémentaires aux unités
    /// </summary>
    /// <param name="value"></param>
    public void AddDamageToUnit(int value)
    {
        _damageBonus += value;
    }

    /// <summary>
    /// Ajout une valeur aux lancés de dés de l'unité
    /// </summary>
    /// <param name="value"></param>
    public void AddDiceToUnit(int value)
    {
        _diceBonus += value;
    }
    #endregion ChangementStat

    [EasyButtons.Button]
    /// <summary>
    /// Update les stats de l'unité avec les stats de base
    /// </summary>
    public virtual void UpdateUnitStat()
    {
        //Si il n'y a pas de scriptable object alors ca arrete la fonction
        if (_unitSO == null) return;

        //Assigne les stats
        _life = _unitSO.LifeMax;
        _shield = 0;
        _attackRange = _unitSO.AttackRange;
        _moveSpeed = _unitSO.MoveSpeed;
       Renderer.sprite = _unitSO.Sprite;
        _creationCost = _unitSO.CreationCost;
        _damageMinimum = _unitSO.DamageMinimum;
        _damageMaximum = _unitSO.DamageMaximum;
        _numberRangeMax = _unitSO.NumberRangeMax;
        _numberRangeMin = _unitSO.NumberRangeMin;

        //Assigne le sprite de l'unité

        //Assigne les sons
        _SonAttaque = _unitSO.SonAttaque;
        _SonDeplacement = _unitSO.SonDeplacement;
        _SonMort = _unitSO.SonMort;
        _SourceAudio = GetComponent<AudioSource>();

        ResetTurn();
    }

    /// <summary>
    /// Reset les valeurs nécéssaires pour un nouveau tour
    /// </summary>
    public virtual void ResetTurn()
    {
        _isActivationDone = false;
        _isMoveDone = false;
        _isActionDone = false;

        MoveSpeedBonus = 0;
        AttackRangeBonus = 0;

        hasUseActivation = false;
        _moveLeft = _unitSO.MoveSpeed;
        _hasStartMove = false;
    }

    /// <summary>
    /// Check si l'unité peut encore se déplacer
    /// </summary>
    public void checkMovementLeft()
    {
        if ((_unitSO.IsInRedArmy && !hasUseActivation && !_unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)) || (!_unitSO.IsInRedArmy && _unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé) && !hasUseActivation && !MélodieSinistre)) 
        {
            Debug.Log("bonsoir");
            hasUseActivation = true;
            PlayerScript.Instance.RedPlayerInfos.ActivationLeft--;
        }
        else if ((!_unitSO.IsInRedArmy && !hasUseActivation && !_unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)) || (_unitSO.IsInRedArmy && _unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé) && !hasUseActivation && !MélodieSinistre))
        {
            Debug.Log("bonsoir");
            hasUseActivation = true;
            PlayerScript.Instance.BluePlayerInfos.ActivationLeft--;
        }

        UIInstance.Instance.UpdateActivationLeft();

        if (_moveLeft == 0)
        {
            _isMoveDone = true;
        }
    }

    /// <summary>
    /// Check si l'unité peut encore être activée
    /// </summary>
    public void checkActivation()
    {
        if (_isActionDone)
        {
            _isActivationDone = true;

            //Réduit le nombre d'activation restante
            if ((_unitSO.IsInRedArmy && !hasUseActivation && !_unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)) || (!_unitSO.IsInRedArmy && _unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé) && !MélodieSinistre))
            {
                if (!_hasStartMove) PlayerScript.Instance.RedPlayerInfos.ActivationLeft--;
                UIInstance.Instance.UpdateActivationLeft();
            }
            else if ((!_unitSO.IsInRedArmy && !_unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé)) || (_unitSO.IsInRedArmy && _unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé) && !MélodieSinistre))
            {
                if (!_hasStartMove) PlayerScript.Instance.BluePlayerInfos.ActivationLeft--;
                UIInstance.Instance.UpdateActivationLeft();
            }
        }
    }

    public void StartCapacity()
    {
        CapacitySystem.Instance.CapacityRunning = true;
        RunningCapacity = true; 
        CapacitySystem.Instance.Updatebutton();
        UIInstance.Instance.DesactivateNextPhaseButton();
            if (TryGetComponent<Capacity>(out Capacity T))
            {
                Debug.Log("starrt");
                T.StartCpty();
            }
    }

    public void EndCapacity()
    {
        CapacitySystem.Instance.CapacityRunning = false;

        _isActionDone = true;
        RunningCapacity = false;
        CapacitySystem.Instance.PanelBlockant1.SetActive(false);
        CapacitySystem.Instance.PanelBlockant2.SetActive(false);
        CapacitySystem.Instance.Updatebutton();
        UIInstance.Instance.ActivateNextPhaseButton();
        checkActivation();
    }

    public void StopCapacity(bool FromCptyScript = false)
    {
        CapacitySystem.Instance.CapacityRunning = false;
        CapacitySystem.Instance.PanelBlockant1.SetActive(false);
        CapacitySystem.Instance.PanelBlockant2.SetActive(false);
        UIInstance.Instance.ActivateNextPhaseButton();
        RunningCapacity = false;
        CapacitySystem.Instance.Updatebutton();
        if (TryGetComponent<Capacity>(out Capacity T))
        {
            if (!FromCptyScript)
            {
                T.StopCpty();
            }
        }
    }

    public void ResetStatutPossesion()
    {
        if (_unitStatuts.Contains(MYthsAndSteel_Enum.UnitStatut.Possédé))
        {
            _diceBonus += 4;
          
            ResetTurn();
          RemoveStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Possédé);
            MélodieSinistre = false;
        }
    }

    private void FxOrgoneSpawn(bool player)
    {

        GameObject fx = Instantiate(player ? OrgoneManager.Instance.FxOrgoneGauche : OrgoneManager.Instance.FxOrgoneDroit,transform.position, Quaternion.identity, player ? OrgoneManager.Instance.ForceFieldGauche.transform : OrgoneManager.Instance.ForceFieldDroit.transform) ;
   
       
        Transform fxChild = fx.transform.GetChild(1);
        
        
        List<GameObject> fxList = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {

  fxList.Add(fxChild.GetChild(i).gameObject);
        }
       foreach (GameObject child  in fxList)
        {
            child.GetComponent<ParticleSystem>().trigger.AddCollider(player ? OrgoneManager.Instance.ForceFieldGauche.transform.GetChild(0).GetChild(0) : OrgoneManager.Instance.ForceFieldDroit.transform.GetChild(0).GetChild(0));

     
        }
       
    }
}