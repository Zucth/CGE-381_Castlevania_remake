using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dog_enemy_activate : MonoBehaviour
{
    [SerializeField] Dog_enemy dogEnemy;

    private void Start()
    {
        //dogEnemy = (Dog_enemy)FindObjectOfType(typeof(Dog_enemy));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dogEnemy.IsIdle_Phase && (collision.tag == "Player"))
        {
            //Debug.Log("Found Player");
            dogEnemy.ChangeFromIdletoRunning();
        }
    }
}
