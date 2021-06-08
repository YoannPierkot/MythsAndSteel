using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Détonation : TerrainParent
{
    public bool _IsInRedArmy = false;
    public int TileID = 0;

    // Joueur qui a posé la détonation.

    public override void EndPlayerTurnEffect(bool IsInRedArmy, UnitScript unit = null)
    {
        Debug.Log("bonsoir");
        if (IsInRedArmy != _IsInRedArmy)
        {
            if (TilesManager.Instance.TileList[TileID].GetComponent<TileScript>().Unit != null)
            {
                TilesManager.Instance.TileList[TileID].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().TakeDamage(2);
            }
            GetComponent<Animator>().SetBool("Detonate", true);
        }
        base.EndPlayerTurnEffect(IsInRedArmy);
    }

    // Call in animation.
    public void Remove()
    {
        this.GetComponentInParent<TileScript>().RemoveEffect(MYthsAndSteel_Enum.TerrainType.Détonation);
    }

}
