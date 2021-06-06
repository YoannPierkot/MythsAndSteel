using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maison :TerrainParent
{
    public override int AttackApply(int BaseDamage = 0)
    {
        int i = 0;
        if (BaseDamage > 0)
        {
            i = -1;
        }

        Debug.Log(i);


        return base.AttackApply(i);
    }
}
