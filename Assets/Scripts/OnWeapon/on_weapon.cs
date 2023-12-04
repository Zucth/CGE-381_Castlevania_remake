using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class on_weapon : MonoBehaviour
{
    //this class was something that was hold by weapon collder
    [SerializeField] sfx_weapon_hit sfx_hit;
    [SerializeField] SoundController sound_con;
    public int update_weapon_damage;

    private void Start()
    {
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.TryGetComponent(out IEnemy enemyComponent))
        {
            sound_con.Playsound_player_whiphit();
            sfx_hit.Summon_WeaponHit_SFX(collision.gameObject);
            enemyComponent.TakeDamage(update_weapon_damage); //TakeDamage will be write and inheritance on emeny
        }
        else if (collision.TryGetComponent(out Obstacle_block ob))
        {
            sfx_hit.Summon_WeaponHit_SFX(collision.gameObject);
            sound_con.Playsound_player_whiphit();
        }
        else if (collision.TryGetComponent(out Kaizo_block kb))
        {
            sfx_hit.Summon_WeaponHit_SFX(collision.gameObject);
            sound_con.Playsound_player_whiphit();
        }
        else
        {
            sound_con.Playsound_player_whip();
        }
    }
}
