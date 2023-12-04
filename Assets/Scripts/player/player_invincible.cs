using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_invincible : MonoBehaviour
{
    [SerializeField] private player_controller controller;
    public bool invincible_time = false;

    [SerializeField] private Color player_color; //chose in unity inspector
    [SerializeField] private Renderer player_color_changer; 

    private void Start()
    {
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        //player_color_changer.GetComponent<Renderer>();
    }

    private void Update()
    {
        if(invincible_time == true)
        {
            gameObject.layer = 11; //set player layer == enemy
            player_color_changer.material.color = player_color;
            controller.isInvincible = true;
            StartCoroutine(ReturnSec());
        }
        else
        {
            return;
        }
    }

    private IEnumerator ReturnSec()
    {
        yield return new WaitForSeconds(2);
        player_color_changer.material.color = Color.white;
        yield return new WaitForSeconds(1);
        gameObject.layer = 3; //set player layer == default
        invincible_time = false;
        controller.isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInvincible_pickup invincible_Pickup)) //this func is on player body so it should detect when it get touch by player colldier
        {
            invincible_time = invincible_Pickup.Invincible_time();

            //+play sound
            Destroy(collision.gameObject);
        }
    }
}
