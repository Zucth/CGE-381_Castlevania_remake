using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dagger_subWeapon : pickup_mother, ISubweapon_pickup
{
    public int SubWeapon_Index()
    {
        Playsound();
        return 1; //dagger index = 1
    }
}
