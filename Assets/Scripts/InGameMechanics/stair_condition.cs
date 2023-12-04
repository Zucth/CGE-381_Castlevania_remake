using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stair_condition : MonoBehaviour
{
    //stair_entrance จะ detect player เมื่อเข้ามาในระยะและจะสั่งให้ stair condition ตัวที่เป็นเป้าทำงาน (Collider.SetTrigger.true) 
    //stair condition มีหน้าที่ ดูว่าผู้เล่นกดปุ่มอะไรอยู่ ถ้าตรงกับ control condition ของตัวเกมก็จะ (Collider.SetTrigger.false)

    public int myTypeInt; //stair number - just to check which one was it check on

    //[SerializeField] private GameObject player;
    [SerializeField] private GameObject bot_stair;
    [SerializeField] private GameObject top_stair;

    [SerializeField] private GameObject entrance_stair; //cover both of it

    [SerializeField] private GameObject exit_bot_stair;
    [SerializeField] private GameObject exit_top_stair;

    [SerializeField] private GameObject player_feet_position;
    [SerializeField] private player_controller player_contr;

    //public one_way_platform onw;
    //[SerializeField] private Transform stair_checker; // cicle checker for activate onw_way_platform
    //[SerializeField] private LayerMask hidden_layer;

    [SerializeField] public GameObject target_stair; //make the whole stair target become invis -this is real stair use for player walking on. visua;/art is different
    [SerializeField] public Collider2D target_stair_collider;
    public enum StairType
    {
        None,
        tl_bl, //top-left to bot-left
        tr_br, //top-right to bot-right
        // etc...
    }
    public StairType stairType;

    //private bool X = false; //for test stuff

    private void Start()
    {
        player_contr = (player_controller)FindObjectOfType(typeof(player_controller));
        reset_stair_exit();
        reset_stair_collider(); //need to reset when player come close with stair_check == false && 
    }
    IEnumerator Delayby_Half_a_sec()
    {
        yield return new WaitForSeconds(0.5f);
    }
    public void reveal_stair_collider() //this use for the opposite
    { 
        target_stair_collider.isTrigger = false;
    }
    public void reset_stair_collider() //this use for the opposite
    {
        target_stair_collider.isTrigger = true;
    }

    public void reset_stair_exit()
    {
        exit_bot_stair.SetActive(false);
        exit_top_stair.SetActive(false);
        StartCoroutine(Delayby_Half_a_sec());
        entrance_stair.SetActive(true);
    }
    public void reveal_stair_exit()
    {
        exit_bot_stair.SetActive(true);
        exit_top_stair.SetActive(true);
        StartCoroutine(Delayby_Half_a_sec());
        entrance_stair.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player") && !player_contr.on_stair)
        {
            if (player_contr.stair_check == false)
            {
                target_stair_collider.isTrigger = true;
            }
        }
        else
        {
            target_stair_collider.isTrigger = false;
        }
    }

}
