using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    #region Variables
  public  int EventUseLeft;
    [Header("ARMY INFO")]
    //nom de l'armée
    public string ArmyName;
    public string ArmyNameNomMasc;
    public string ArmyNameNomFem;

    [Header("ACTIVATION")]
    //Nombre d'activation restante
    public int ActivationLeft; 
    //Valeur de la carte activation posée
    public int ActivationCardValue;

    [Header("ORGONE")]
    //Nombre de charges d'orgone actuel
    [SerializeField] private int _OrgoneValue;
    public bool OrgoneExploseCancel = true;
    public int OrgoneValue
    {
        get
        {
            return _OrgoneValue;
        }
    }
    //Nombre de pouvoirs d'orgone encore activable
    public int OrgonePowerLeft; 
    //Permet de se souvenir de la dernière valeur d'orgone avant Update
    public int _LastKnownOrgoneValue;
    //Tile qui correspond au centre de la zone d'Orgone
    public GameObject TileCentreZoneOrgone;
    //Save Le joueur dont la jauge explose
    int PLayerOrgoneExplose;

    [Header("RESSOURCE")]
    //Nombre de Ressources actuel
    [SerializeField] private int _Ressource;
    public int Ressource
    {
        get
        {
            return _Ressource;
        }
        set
        {
            
            if (_Ressource > value)
            {
                if (GameManager.Instance.IsPlayerRedTurn)
                {
                    PlayerScript.Instance.AnimRessource(1);
                    
                }
                else if (!GameManager.Instance.IsPlayerRedTurn)
                {
                    PlayerScript.Instance.AnimRessource(2);
                
                }
            }

            _Ressource = value;
            UIInstance.Instance.UpdateRessourceLeft();
        }
    }

    [Header("Objectif actuellement capturé")]
    //Nombre d'objectif actuellement capturé
    public int GoalCapturePointsNumber;     

    public bool HasCreateUnit; //est ce que le joueur a créer une unité durant sont tour
    #endregion Variables

    /// <summary>
    /// Check si l'orgone va exploser
    /// </summary>
    /// <returns></returns>
    public bool OrgoneExplose(){
        return OrgoneValue > 5 ? true : false;
    }
    
    /// <summary>
    /// Change la valeur (pos/neg) de la jauge d'orgone.
    /// </summary>
    /// <param name="Value">Valeur positive ou négative.</param>
    public void ChangeOrgone(int Value, int player){
        if (Value != 0)
        {
            _LastKnownOrgoneValue = _OrgoneValue;
            _OrgoneValue += Value;
            UpdateOrgoneUI(player);
       
        }
    }

    public void CheckOrgone(int player){
 
        GameManager.Instance.IsCheckingOrgone = true;
      
        if (OrgoneExplose() && !GameManager.Instance.ChooseUnitForEvent)
        {
            if (OrgoneExploseCancel == true){
                UpdateOrgoneUI(player);
                OrgoneExploseCancel = false;
            }
          
            
        }
        else
        {
            if (OrgoneExploseCancel == true)
            {
                UpdateOrgoneUI(player);
              
            }
          

            GameManager.Instance.IsCheckingOrgone = false;
            if(GameManager.Instance._waitToCheckOrgone != null)
            {
                GameManager.Instance._waitToCheckOrgone();
            }
        }
    }


    int i = 0;
    public void DealDamageToUnit(){
        if(GameManager.Instance.UnitChooseList.Count > i)
        {
            GameManager.Instance.UnitChooseList[i].GetComponent<UnitScript>().TakeDamage(1);
            i++;
            GameManager.Instance._waitEvent -= DealDamageToUnit;
            GameManager.Instance._waitEvent += DealDamageToUnit;
            GameManager.Instance.WaitToMove(.035f);
        }
        else
        {
            GameManager.Instance._waitEvent -= DealDamageToUnit;

            GameManager.Instance.UnitChooseList.Clear();

            GameManager.Instance.IsCheckingOrgone = false;

            if(GameManager.Instance._waitToCheckOrgone != null)
            {
                GameManager.Instance._waitToCheckOrgone();
            }
        }
    }

    /// <summary>
    /// Update l'UI de la jauge d'orgone en fonction du nombre de charge
    /// </summary>
    public void UpdateOrgoneUI(int player)
    {
        if (player == 1){
            OrgoneManager.Instance.StartOrgoneAnimation(1, _LastKnownOrgoneValue, OrgoneValue);
        }
        else
        {

            OrgoneManager.Instance.StartOrgoneAnimation(2, _LastKnownOrgoneValue, OrgoneValue);
        }

    }

    //AV
    [HideInInspector] public bool dontTouchThis = false;
}
