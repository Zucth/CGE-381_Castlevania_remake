using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class player_crossOTK : MonoBehaviour
{
    [SerializeField] private GameManager_controller game_controller;
    void Start()
    {
        game_controller = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ICross_OTK cross_otk)) //this func is on player body so it should detect when it get touch by player colldier
        {

            //+play sound
            game_controller.KillAll();
            Destroy(collision.gameObject);
        }
    }
}
