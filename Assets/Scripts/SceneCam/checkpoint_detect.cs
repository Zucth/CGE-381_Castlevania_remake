using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint_detect : MonoBehaviour
{
    [SerializeField] player_controller p_controller;
    [SerializeField] replay_controller replay;
    public int checkpoint_num;

    private void Start()
    {
        p_controller = (player_controller)FindObjectOfType(typeof(player_controller));
    }

    private void Update()
    {
        if(replay == null)
        {
            replay = (replay_controller)FindObjectOfType(typeof(replay_controller));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            p_controller.setCheckpoint();
            replay._Savedcheckpoint_num = checkpoint_num; //save spawnpoint
            //Debug.Log("checkpoint saved");
        }
    }
}
