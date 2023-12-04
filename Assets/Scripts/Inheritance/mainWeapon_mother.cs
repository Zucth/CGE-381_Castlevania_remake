using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainWeapon_mother : MonoBehaviour
{

    // a base that save Main_Weapon data
    [SerializeField] protected int weapon_level = 1;
    [SerializeField] protected int weapon_damage = 250; //up to difference weapon level //250,

    [SerializeField] protected GameObject weapon_short;
    [SerializeField] protected GameObject weapon_long;

    [SerializeField] protected on_weapon Onweapon, Onweapon_2;
    [SerializeField] protected player_controller controller;
    [SerializeField] protected GameManager_controller gameManager;
    [SerializeField] protected player_timer timer;
    [SerializeField] protected ColorBlink_Renderer colorBlink_Renderer_weapon;

    private void Start() //add the inheritance that hold this class to the clone_player
    {
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        timer = (player_timer)FindObjectOfType(typeof(player_timer));

        weapon_short.SetActive(false);
        weapon_long.SetActive(false);
    }

    protected void ResetWhip() //use when reset after dead
    {
        weapon_level = 1;
    }

    protected void whip_level1()  //weapon_short + dmg = 1 
    {
        
        weapon_damage = 250;
        Onweapon.update_weapon_damage = weapon_damage;
        StartCoroutine(SetAttack1());
    }
    protected void whip_level2() //weapon_short + dmg = 3
    {
        
        weapon_damage = 375;
        Onweapon.update_weapon_damage = weapon_damage;
        StartCoroutine(SetAttack1());
    }
    protected void whip_level3() //weapon_short + weapon_long + dmg = 3
    {
        
        weapon_damage = 375;
        Onweapon.update_weapon_damage = weapon_damage;
        StartCoroutine(SetAttack2());
    }

    protected IEnumerator SetAttack1()
    {
        yield return new WaitForSeconds(0.41f);
        weapon_short.SetActive(true);
        controller.anim_p.SetBool("atk", false);
        yield return new WaitForSeconds(0.12f);
        weapon_short.SetActive(false);
    }
    protected IEnumerator SetAttack2()
    {
        yield return new WaitForSeconds(0.41f);
        weapon_short.SetActive(true);
        weapon_long.SetActive(true);
        controller.anim_p.SetBool("atk", false);
        yield return new WaitForSeconds(0.12f);
        weapon_long.SetActive(false);
        weapon_short.SetActive(false);
    }


}
