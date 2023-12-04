using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager_controller : MonoBehaviour
{
    public List<GameObject> enemy_gameobject_list;//kept enemy amount
    public List<Rigidbody2D> unit_gameobject_list; //kept player/enemy/projectile in the scene

    [SerializeField] player_heart_transfer heart_Transfer;
    [SerializeField] player_subWeapon subWeapon;
    [SerializeField] CreepController creep;
    [SerializeField] player_endgame P_end;

    [SerializeField] protected player_timer timer;

    private void Start()
    {
        heart_Transfer = (player_heart_transfer)FindObjectOfType(typeof(player_heart_transfer));
        subWeapon = (player_subWeapon)FindObjectOfType(typeof(player_subWeapon));
        creep = (CreepController)FindObjectOfType(typeof(CreepController));
        P_end = (player_endgame)FindObjectOfType(typeof(player_endgame));

        timer = (player_timer)FindObjectOfType(typeof(player_timer));
    }

    public void Set_Cutscene() 
    {
        for (int x = enemy_gameobject_list.Count - 1; x >= 0; x--) 
        {
            if (enemy_gameobject_list[x] == null)
            {
                enemy_gameobject_list.RemoveAt(x);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out IEnemy enemy)) 
            {
                enemy_gameobject_list[x].gameObject.transform.position = new Vector3(-500f, -500f, -500f);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out on_sub_weapon sub_Weapon))
            {
                enemy_gameobject_list[x].gameObject.transform.position = new Vector3(-500f, -500f, -500f);
            }
        }
    }
    public void KillAll()
    {
        for (int x = enemy_gameobject_list.Count - 1; x >= 0; x--)
        {
            if (enemy_gameobject_list[x] == null)
            {
                enemy_gameobject_list.RemoveAt(x);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Zombie_enemy zombie))
            {
                zombie.ReturnScore = 0;
                zombie.TakeDamage(3000);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Fisherman_enemy fisherman))
            {
                fisherman.ReturnScore = 0;
                fisherman.TakeDamage(3000);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Dog_enemy dog))
            {
                dog.ReturnScore = 0;
                dog.TakeDamage(3000);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out sub_bat bat))
            {
                bat.gameObject.transform.position = new Vector3(-500f, -500f, -500f);
            }
            //doesn't effect boss
            else if (enemy_gameobject_list[x].TryGetComponent(out Fisherman_projectile projectile))
            {
                projectile.ReturnScore = 0;
                projectile.TakeDamage(3000);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Boss_projectile b_projectile))
            {
                b_projectile.ReturnScore = 0;
                b_projectile.TakeDamage(3000);
            }
            
        }
    }
    public void FreezeAll()
    {
        creep.SpawnAble = false;

        for (int x = unit_gameobject_list.Count - 1; x >= 0; x--)
        {
            if (unit_gameobject_list[x] == null)
            {
                unit_gameobject_list.RemoveAt(x);
            }
            else if (unit_gameobject_list[x].TryGetComponent(out player_controller player))
            {
                player.WhenFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Zombie_enemy zombie))
            {
                zombie.WhenFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Fisherman_enemy fisherman))
            {
                fisherman.WhenFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Dog_enemy dog))
            {
                dog.WhenFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Bat_enemy bat))
            {
                bat.WhenFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Boss boss))
            {
                boss.WhenFreeze();
            }

            else if (unit_gameobject_list[x].TryGetComponent(out Fisherman_projectile projectile))
            {
                projectile.WhenFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Boss_projectile b_projectile))
            {
                b_projectile.WhenFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out on_sub_weapon sub_Weapon))
            {
                sub_Weapon.WhenFreeze();
            }
        }
        if (heart_Transfer.GameEnd == false)
        {
            StartCoroutine(Unfreeze_Gameobject());
        }
        else if(heart_Transfer.GameEnd == true)
        {
            //no freeze
            heart_Transfer.GameEnd = false;
        }
    }
    public void PauseEnemy() //only enemy
    {
        creep.SpawnAble = false;

        for (int x = enemy_gameobject_list.Count - 1; x >= 0; x--)
        {
            if (enemy_gameobject_list[x] == null)
            {
                enemy_gameobject_list.RemoveAt(x);
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Zombie_enemy zombie))
            {
                zombie.WhenFreeze();
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Fisherman_enemy fisherman))
            {
                fisherman.WhenFreeze();
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Dog_enemy dog))
            {
                dog.WhenFreeze();
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Bat_enemy bat))
            {
                bat.WhenFreeze();
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Boss boss))
            {
                boss.WhenFreeze();
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Fisherman_projectile projectile))
            {
                projectile.WhenFreeze();
            }
            else if (enemy_gameobject_list[x].TryGetComponent(out Boss_projectile b_projectile))
            {
                b_projectile.WhenFreeze();
            }
        }
        StartCoroutine(Unfreeze_Gameobject_Stopwatch());
    }

    private IEnumerator Unfreeze_Gameobject() //other stuff
    {
        yield return new WaitForSeconds(1.0f);

        for (int x = unit_gameobject_list.Count - 1; x >= 0; x--)
        {
            if (unit_gameobject_list[x] == null)
            {
                unit_gameobject_list.RemoveAt(x);
            }
            else if (unit_gameobject_list[x].TryGetComponent(out player_controller player))
            {
                player.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Zombie_enemy zombie))
            {
                zombie.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Fisherman_enemy fisherman))
            {
                fisherman.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Dog_enemy dog))
            {
                dog.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Bat_enemy bat))
            {
                bat.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Boss boss))
            {
                boss.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Fisherman_projectile projectile))
            {
                projectile.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Boss_projectile b_projectile))
            {
                b_projectile.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out on_sub_weapon sub_Weapon))
            {
                sub_Weapon.StopFreeze();
            }
        }
        creep.SpawnAble = true;
        timer.timerIsRunning = true;
    }

    private IEnumerator Unfreeze_Gameobject_Stopwatch()
    {
        yield return new WaitForSeconds(3.0f);

        for (int x = unit_gameobject_list.Count - 1; x >= 0; x--)
        {
            if (unit_gameobject_list[x] == null)
            {
                unit_gameobject_list.RemoveAt(x);
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Zombie_enemy zombie))
            {
                zombie.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Fisherman_enemy fisherman))
            {
                fisherman.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Dog_enemy dog))
            {
                dog.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Bat_enemy bat))
            {
                bat.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Boss boss))
            {
                boss.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Fisherman_projectile projectile))
            {
                projectile.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out Boss_projectile b_projectile))
            {
                b_projectile.StopFreeze();
            }
            else if (unit_gameobject_list[x].TryGetComponent(out on_sub_weapon sub_Weapon))
            {
                sub_Weapon.StopFreeze();
            }
        }

        subWeapon.UsedTimer = false;
        creep.SpawnAble = true;
        timer.timerIsRunning = true;
    }
}
