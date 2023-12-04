using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_trigger : MonoBehaviour
{
    [SerializeField] private Boss boss_script;
    bool Isactive = true;

    void Start()
    {
        boss_script = (Boss)FindObjectOfType(typeof(Boss));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && Isactive)
        {
            boss_script.TriggerBossFight(); //run only once

            Isactive = false;
        }
    }
}
