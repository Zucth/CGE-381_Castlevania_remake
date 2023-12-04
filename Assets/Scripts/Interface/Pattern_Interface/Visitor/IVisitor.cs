using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitor
{
    //void Visit(IVisitor visitor); //replace inside (with class name and instant of that class)
    void Visit(Score_PickUp score_pickup);
}
