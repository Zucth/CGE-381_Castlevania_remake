using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cross_OTK_item : pickup_mother, ICross_OTK
{
    bool _isActive = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //this func is on player body so it should detect when it get touch by player colldier
        {
            if (_isActive)
            {
                _isActive = false;
                Playsound();
            }
        }
    }
}
