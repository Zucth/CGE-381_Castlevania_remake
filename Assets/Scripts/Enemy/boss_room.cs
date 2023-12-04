using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class boss_room : MonoBehaviour
{
    public List<GameObject> bossroom_left_list;//kept boss and player
    public List<GameObject> bossroom_right_list;//kept boss and player

    [SerializeField] private int RoomNumber; // left == 0, right == 1, 
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    [SerializeField] private Boss boss_script;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        boss = GameObject.FindWithTag("Boss");
        boss_script = (Boss)FindObjectOfType(typeof(Boss));
    }
    private void FixedUpdate()
    {
        if ((bossroom_left_list.Count == 2) || (bossroom_right_list.Count == 2))
        {
            boss_script.SameBox = true;
        }
        else
        {
            boss_script.SameBox = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(RoomNumber == 0)
        {
            if (collision.tag == "Player")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_left_list.Remove(player);
            }
            if (collision.tag == "Boss")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_left_list.Remove(boss);
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_right_list.Remove(player);
            }
            if (collision.tag == "Boss")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_right_list.Remove(boss);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(RoomNumber == 0)
        {
            if (collision.tag == "Player")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_left_list.Add(player);
            }
            if (collision.tag == "Boss")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_left_list.Add(boss);
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_right_list.Add(player);
            }
            if (collision.tag == "Boss")
            {
                //bossroom_list.Add(collision.gameObject);
                bossroom_right_list.Add(boss);
            }
        }
        
    }
}
