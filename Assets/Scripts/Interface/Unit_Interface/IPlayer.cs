using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public int PlayerTakeDamage(int v);
    public int PlayerTakeHeal(int v);
}
