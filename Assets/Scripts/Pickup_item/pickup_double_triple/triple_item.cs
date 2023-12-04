using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triple_item : pickup_mother, IDoubleTriple
{
    public int Double_Triple_Index()
    {
        Playsound();
        return 1; //return delay sec as 1 seconds
    }
}
