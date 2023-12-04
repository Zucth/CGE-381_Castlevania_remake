using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_PickUp : MonoBehaviour, IpickUp_Element //this class will use for storing the score, #or even for update the scoreboard UI
{
    public int score_stored = 0;

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
