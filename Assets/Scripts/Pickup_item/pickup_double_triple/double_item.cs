using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class double_item : pickup_mother, IDoubleTriple
{
    public int Double_Triple_Index()
    {
        Playsound();    
        return 2; //return delay sec as 2 seconds
    }
}
