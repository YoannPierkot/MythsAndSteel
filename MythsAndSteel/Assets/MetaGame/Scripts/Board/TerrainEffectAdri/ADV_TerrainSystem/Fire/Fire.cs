using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : TerrainParent
{

    public enum type {
        brasier, feu
    }
    private type Type;

    [SerializeField] private int _Turnleft;
    public FireGestion FireG;

    public int TurnLeft
    {
        get
        {
            return _Turnleft;
        }
        set
        {
            _Turnleft = value;
            Check();
        }
    }

    private void Start()
    {
        if(GetComponentInParent<TileScript>().Unit != null)
        {
            OnUnityAdd(GetComponentInParent<TileScript>().Unit.GetComponent<UnitScript>());
        }
        Check();
    }
    public void Check()
    {
        if (TurnLeft == 2)
        {
            GetComponentInChildren<Animator>().SetBool("Feu", false);
            GetComponentInChildren<Animator>().SetBool("Brasier", true);

            if (GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Feu))
            {
                //GetComponentInParent<TileScript>().TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Feu);
            }
            if (!GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Brasier))
            {
               // GetComponentInParent<TileScript>().TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Brasier);
            }
           // GetComponentInParent<ChildEffect>().Type = MYthsAndSteel_Enum.TerrainType.Brasier;
            Type = type.brasier;

        }
        else if (TurnLeft == 1)
        {
            GetComponentInChildren<Animator>().SetTrigger("Out");
            GetComponentInChildren<Animator>().SetBool("Brasier", false);
            GetComponentInChildren<Animator>().SetBool("Feu", true);

            if (GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Brasier))
            {
                //GetComponentInParent<TileScript>().TerrainEffectList.Remove(MYthsAndSteel_Enum.TerrainType.Brasier);
            }
            if (!GetComponentInParent<TileScript>().TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Feu))
            {
                //GetComponentInParent<TileScript>().TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Feu);
            }
            //GetComponent<ChildEffect>().Type = MYthsAndSteel_Enum.TerrainType.Feu;
            Type = type.feu;
        }
        else if (TurnLeft <= 0)
        {
            GetComponentInChildren<Animator>().SetTrigger("Out");
        }
    }

    public void CheckingEffector()
    {
        if(TurnLeft <= 0)
        {
            FireG.FireActive.Remove(this);
            GetComponentInParent<TileScript>().RemoveEffect(MYthsAndSteel_Enum.TerrainType.Feu);
            GetComponentInParent<TileScript>().RemoveEffect(MYthsAndSteel_Enum.TerrainType.Brasier);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (FireG.FireActive.Contains(this))
        {
            FireG.FireActive.Remove(this);
        }
    }

    public override void OnUnityAdd(UnitScript Unit)
    {
        if (TurnLeft == 2)
        {
            Unit.TakeDamage(2, false, true);
        }
        if (TurnLeft == 1)
        {
            Unit.TakeDamage(1); 
        }
        base.OnUnityAdd(Unit);
    }

    public override void EndTurnEffect(TileScript ts, UnitScript Unit = null)
    {
        TurnLeft--;
        Debug.Log(TurnLeft);
        if (TurnLeft == 2)
        {
            if (Unit != null) Unit.TakeDamage(2);

        }
        if (TurnLeft == 1)
        {
            if (Unit != null) Unit.TakeDamage(1);
        }
        base.EndTurnEffect(ts, Unit);
    }
}
