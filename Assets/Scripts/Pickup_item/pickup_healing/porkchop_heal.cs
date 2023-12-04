using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class porkchop_heal : pickup_mother
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPlayer playerComponent))
        {
            Playsound();
            playerComponent.PlayerTakeHeal(6); //TakeHeal will be write and inheritance on emeny
            Destroy(gameObject);
        }
    }
}
