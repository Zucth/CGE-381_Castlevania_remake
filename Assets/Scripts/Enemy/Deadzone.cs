using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{

    [SerializeField] private GameObject water_sfx; //water sfx
    [SerializeField] private Transform parent_fx_pool; //fx_pool
    [SerializeField] player_controller controller;
    [SerializeField] SoundController sound_con;

    public bool Fx_Only;

    private void Start()
    {
        parent_fx_pool = GameObject.FindWithTag("fx_pool").transform;
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IPlayer playerComponent))
        {
            if (Fx_Only)
            {
                sound_con.Playsound_waterSplash_sfx();
                Instantiate(water_sfx, collision.transform.position, collision.transform.rotation, parent_fx_pool);
            }
            else
            {
                controller.DiedfromFalling = true;
                playerComponent.PlayerTakeDamage(200); //One hit dead
            }
        }

        else if (collision.TryGetComponent(out IEnemy enemy)) //detect for any monster that enter this area
        {
            if (Fx_Only)
            {
                sound_con.Playsound_waterSplash_sfx();
                Instantiate(water_sfx, collision.transform.position, collision.transform.rotation, parent_fx_pool);
            }
            else
            {
                collision.transform.position = new Vector3(-500f, -500f, -500f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPlayer playerComponent))
        {
            if(!Fx_Only)
            {
                controller.DiedfromFalling = true;
                playerComponent.PlayerTakeDamage(200); //One hit dead
            }
        }

        else if (collision.TryGetComponent(out IEnemy enemy)) //detect for any monster that enter this area
        {
            if (!Fx_Only)
            {
                collision.transform.position = new Vector3(-500f, -500f, -500f);
            }
        }
    }
}
