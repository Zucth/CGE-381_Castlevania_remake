using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible_item : pickup_mother, IInvincible_pickup
{
    public bool Invincible_time()
    {
        Playsound();
        return true; //Invincible time
    }
}
