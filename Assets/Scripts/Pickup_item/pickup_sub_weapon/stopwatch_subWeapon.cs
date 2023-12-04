using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopwatch_subWeapon : pickup_mother, ISubweapon_pickup
{
    public int SubWeapon_Index()
    {
        Playsound();
        return 3; //stopwatch index = 3
    }
}
