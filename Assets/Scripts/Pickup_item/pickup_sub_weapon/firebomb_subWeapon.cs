using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firebomb_subWeapon : pickup_mother, ISubweapon_pickup
{
    public int SubWeapon_Index()
    {
        Playsound();
        return 4; //firebomb index = 4
    }
}
