using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainParent : MonoBehaviour
{

    public virtual void OnUnityAdd(UnitScript Unit)
    {

    }
    public virtual void OnUnityDown(UnitScript Unit)
    {

    }
    public virtual void ComingFromUp(UnitScript Unit)
    {

    }
    public virtual void ComingFromDown(UnitScript Unit)
    {

    }
    public virtual void ComingFromLeft(UnitScript Unit)
    {

    }
    public virtual void ComingFromRight(UnitScript Unit)
    {

    }
    public virtual void QuitToRight(UnitScript Unit)
    {

    }
    public virtual void QuitToLeft(UnitScript Unit)
    {

    }
    public virtual void QuitToUp(UnitScript Unit)
    {

    }
    public virtual void QuitToDown(UnitScript Unit)
    {

    }
    public virtual void UnithasBeenAttacked(UnitScript Unit)
    {

    }
    public virtual void FirstUnitOnCase(UnitScript Unit)
    {

    }

    /// <summary>
    /// Effet appliqué à la case à la fin de chaque tour de jeu.
    /// Cette fonction est appelée même si il n'y a pas d'unité sur la case.
    /// </summary>
    /// <param name="Unit"></param>
    public virtual void EndTurnEffect(TileScript ts, UnitScript Unit = null)
    {

    }
    public virtual void EndPlayerTurnEffect(bool IsInRedArmy, UnitScript Unit = null)
    {

    }
    /// <summary>
    /// Cette fonction sert uniquement à modifier la range d'attaque de l'unité se trouvant sur la case.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public virtual int AttackRangeValue(int i = 0)
    {
        return i;
    }

    public virtual void CibledByAttack(UnitScript AttackerUnit, TileScript AttackerUnitCase)
    {
        // Colline, plage, bosquet 
    }
    public virtual void UnCibledByAttack(UnitScript AttackerUnit)
    {

    }
    public virtual int AttackApply(int BaseDamage = 0)
    {
        return BaseDamage;
    }
}
