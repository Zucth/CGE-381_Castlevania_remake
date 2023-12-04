using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_subWeapon : subWeapon_mother
{
    //update sp_atk_delay whcn collide with double/triple item
    private void Start()
    {
        Empty(); //image = black screen
    }
    public void SpecialWeapon_Attack() //used here
    {
        if (sp_atk_ready && heart_Transfer.player_heart_ >= 1)
        {
            //play animation
            //Debug.Log("Subweapon index =" + subweapon_Index);

            if (subweapon_Index == 1) //dagger -1
            {
                controller.anim_p.SetBool("sp_atk", true);
                sp_atk_ready = false;
                heart_Transfer.player_heart_ -= 1;
                StartCoroutine(Dagger());
                
                StartCoroutine(ResetAttack(sp_atk_delay));
            }
            else if (subweapon_Index == 2) //axe -1
            {
                controller.anim_p.SetBool("sp_atk", true);
                sp_atk_ready = false;
                heart_Transfer.player_heart_ -= 1;
                StartCoroutine(Axe());
                StartCoroutine(ResetAttack(sp_atk_delay));
            }
            else if (subweapon_Index == 3 && (sp_atk_ready && heart_Transfer.player_heart_ >= 5)) //stop_watch -5
            {
                if (!UsedTimer)
                {
                    sp_atk_ready = false;
                    heart_Transfer.player_heart_ -= 5;
                    UsedTimer = true;
                    StopWatch();
                    StartCoroutine(ResetAttack(sp_atk_delay));
                }
            }
            else if (subweapon_Index == 4) //fire_bomb -1
            {
                controller.anim_p.SetBool("sp_atk", true);
                sp_atk_ready = false;
                heart_Transfer.player_heart_ -= 1;
                
                StartCoroutine(Firebomb());
                StartCoroutine(ResetAttack(sp_atk_delay));
            }
            else
            {
                //Debug.Log("Sub_Weapon_Index is error!");
                sp_atk_ready = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ISubweapon_pickup subweapon_Pickup)) //this func is on player body so it should detect when it get touch by player colldier
        {
            //Debug.Log(collision + "switch to sub_weapon id");
            subweapon_Index = subweapon_Pickup.SubWeapon_Index();

            if (subweapon_Index == 1) //dagger
            {
                image.sprite = dagger;
            }
            else if (subweapon_Index == 2) //axe
            {
                image.sprite = axe;
            }
            else if (subweapon_Index == 3) //stopwatch
            {
                image.sprite = stopwatch;
            }
            else if (subweapon_Index == 4) //firebomb
            {
                image.sprite = firebomb;
            }
            //connect with ui 

            //+play sound
            Destroy(collision.gameObject); //destroy sub-weapon pickup
        }

        if (collision.TryGetComponent(out IDoubleTriple doubleTriple)) 
        {
            sp_atk_delay = doubleTriple.Double_Triple_Index();

            if(sp_atk_delay == 2)
            {
                image_2.sprite = _double;
            }
            else if (sp_atk_delay == 1)
            {
                image_2.sprite = _triple;
            }

            //+play sound
            Destroy(collision.gameObject); //destroy sub-weapon pickup
        }
    }
}
