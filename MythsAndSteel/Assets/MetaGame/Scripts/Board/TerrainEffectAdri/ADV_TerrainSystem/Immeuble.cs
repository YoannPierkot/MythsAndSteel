using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immeuble : TerrainParent
{
   

    public override int AttackApply(int BaseDamage = 0)
    {
        int i = 0;
        if(BaseDamage > 0)
        {
            i = -BaseDamage;
        }

        Debug.Log(i);
       

        return base.AttackApply(i);
    }
}
