using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : TerrainParent
{
    public void Awake()
    {
        GetComponentInParent<TileScript>().AddEffectToList(MYthsAndSteel_Enum.TerrainType.Point_de_ressource);
        GetComponentInParent<TileScript>().ResourcesCounter = 2;
    }

    public override void FirstUnitOnCase(UnitScript Unit)
    {
        if (Unit != null)
        {
            Debug.LogWarning("Deux ressources supp. car vous êtes arrivés en premier sur le bunker. Prévoir un rendu visuel de récupération ?");
            if (Unit.UnitSO.IsInRedArmy)
            {
                GetComponentInParent<TileScript>().RemoveRessources(2, 1);
            }
            else
            {
                GetComponentInParent<TileScript>().RemoveRessources(2, 2); 
            }
        }
        base.FirstUnitOnCase(Unit);
    }

    public override int AttackApply(int BaseDamage = 0)
    {
        int i = 0;
        if(BaseDamage > 0)
        {
            i--;
        }

        Debug.Log("attack low");

        return base.AttackApply(i);
    }
}
