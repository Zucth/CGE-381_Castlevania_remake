using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whip_levelThree : pickup_mother, IMainweapon_pickup
{
    public int WeaponUpgrade_level()
    {
        Playsound();
        return 3; //level 3
    }
}
