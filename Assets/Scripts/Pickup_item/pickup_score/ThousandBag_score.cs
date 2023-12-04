using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThousandBag_score : pickup_mother, IScore_Pickup
{
    [SerializeField] sfx_text text_drop;
    public int GetScore()
    {
        Playsound();
        text_drop.Summon_text_SFX();
        return 1000; //score return
    }
}
