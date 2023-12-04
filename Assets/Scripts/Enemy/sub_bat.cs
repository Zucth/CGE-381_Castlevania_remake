using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sub_bat : MonoBehaviour, IEnemy
{
    [SerializeField] Bat_enemy bat_script;
    //private int damage;

    private void Start()
    {
        //bat_script = (Bat_enemy)FindObjectOfType(typeof(Bat_enemy));
    }

    public int TakeDamage(int DamageTaken)
    {
        bat_script.health -= DamageTaken;
        //anim.SetBool("isDead", true);
        //Debug.Log("Bruh");

        if (bat_script.health <= 0)
        {
            bat_script.deadCalled();
        }
        return bat_script.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bat_script.isAlive)
        {
            if (collision.TryGetComponent(out IPlayer playerComponent))
            {
                playerComponent.PlayerTakeDamage(20); //TakeDamage will be write and inheritance on emeny
            }
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.TryGetComponent(out on_weapon weapon))
        {
            weapon.update_weapon_damage = damage;
            bat_script.TakeDamage(damage);
            TakeDamage(damage);
        }
    }*/
}
