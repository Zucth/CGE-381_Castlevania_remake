using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whip_levelTwo : pickup_mother, IMainweapon_pickup
{
    public int WeaponUpgrade_level()
    {
        Playsound();
        return 2; //level 2
    }
}
