using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axe_subWeapon : pickup_mother, ISubweapon_pickup
{
    public int SubWeapon_Index()
    {
        Playsound();
        return 2; //axe index = 2
    }
}
