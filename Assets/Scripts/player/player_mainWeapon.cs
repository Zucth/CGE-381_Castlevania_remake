using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_mainWeapon : mainWeapon_mother //this class was hold with player
{
    public int weapon_level_speaker = 1;
    /*
    private void Awake()
    {
        ResetWhip();
        weapon_level_speaker = weapon_level;
    }*/
    public void MainWeapon_Attack()
    {
        weapon_level_speaker = weapon_level;
        if (weapon_level == 1)
        {
            controller.anim_p.SetInteger("weapon_level", 1);
            controller.anim_p.SetBool("atk", true);
            whip_level1();
        }
        else if (weapon_level == 2)
        {
            controller.anim_p.SetInteger("weapon_level", 2);
            controller.anim_p.SetBool("atk", true);
            whip_level2();
        }
        else if (weapon_level == 3)
        {
            controller.anim_p.SetInteger("weapon_level", 3);
            controller.anim_p.SetBool("atk", true);
            whip_level3();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IMainweapon_pickup mainweapon_Pickup)) //this func is on player body so it should detect when it get touch by player colldier
        {
            weapon_level = mainweapon_Pickup.WeaponUpgrade_level();
            weapon_level_speaker = weapon_level; //speak out for heart condition drop
            colorBlink_Renderer_weapon.ColorChanger();

            //freeze player
            gameManager.FreezeAll();
            timer.timerIsRunning = false;

            Destroy(collision.gameObject); //destroy weapon pickup
        }
    }
}
