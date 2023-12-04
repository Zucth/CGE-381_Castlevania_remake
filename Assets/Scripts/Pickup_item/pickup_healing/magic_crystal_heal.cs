using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic_crystal_heal : pickup_mother
{
    [SerializeField] player_heart_transfer heart_Transfer;

    private void Start()
    {
        heart_Transfer = (player_heart_transfer)FindObjectOfType(typeof(player_heart_transfer));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));

        _setAd_active();
    }

    public void _setAd_active()
    {
        StartCoroutine(fally());
    }

    IEnumerator fally()
    {
        yield return new WaitForSeconds(1.25f);
        Fall_delay = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPlayer playerComponent))
        {
            Playsound();
            playerComponent.PlayerTakeHeal(16); //TakeHeal will be write and inheritance on emeny
            Destroy(gameObject);
            heart_Transfer.GameEnd = true; //end game = true
        }
    }
}
