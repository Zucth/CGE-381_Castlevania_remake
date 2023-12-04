using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_enemy : sfx_upon_destroy
{
    [SerializeField] public int health, Maxhealth = 200;
    public int ReturnScore = 200;
    [SerializeField] private GameObject player;
    [SerializeField] float CapSpeed, speed;
    private bool Isleft;
    public bool isAlive = true;

    [SerializeField] private Animator anim; //later put animation on
    [SerializeField] private Collider2D collider2d;

    [SerializeField] GameObject itself;
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] GameManager_controller gameManager;

    private bool FacingDirection;

    //public AudioSource enemy_dead;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));

        health = Maxhealth;
        CapSpeed = speed;
        CheckDir();
    }

    // Update is called once per frame
    void Update()
    {
        if (Isleft)
        {
            MoveLeft();
        }
        else //is left = false, then turn right
        {
            MoveRight();
        }
    }
    public void deadCalled()
    {
        if (isAlive)
        {
            gameManager.enemy_gameobject_list.Remove(itself);
            gameManager.unit_gameobject_list.Remove(rg2d);
            SummonSFX();

            isAlive = false;

            speed = 0;
            collider2d.enabled = false;
            anim.SetBool("dead", true);

            scoreboard.ReturnScoreFromKilling(ReturnScore);
            SummonSFX();
            Destroy(gameObject, 0.3f);
        }
    }
    private void CheckDir()
    {
        if (transform.position.x > player.transform.position.x)
        {
            Isleft = true;
            //Move left
        }
        else
        {
            Isleft = false;
            //Move right
        }
    }

    private void MoveLeft()
    {
        FacingDirection = true;
        Walking_FlipCharacter();
        transform.position += new Vector3(-0.85f * speed * Time.deltaTime, 0.0f, 0.0f);
    }
    private void MoveRight()
    {
        FacingDirection = false;
        Walking_FlipCharacter();
        transform.position += new Vector3(0.85f * speed * Time.deltaTime, 0.0f, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            if (collision.gameObject.TryGetComponent(out IPlayer playerComponent))
            {
                playerComponent.PlayerTakeDamage(20); //TakeDamage will be write and inheritance on emeny
            }
        }
    }
    void Walking_FlipCharacter()
    {
        if (transform.localEulerAngles.y != 0 && !FacingDirection)
            transform.Rotate(0f, -180f, 0f);
        else if (transform.localEulerAngles.y != 180 && FacingDirection)
            transform.Rotate(0f, 180f, 0f);
    }

    public void WhenFreeze()
    {
        speed = 0;
        rg2d.velocity = new Vector3(0, 0, 0);
        anim.speed = 0;
    }
    public void StopFreeze()
    {
        speed = CapSpeed;
        anim.speed = 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Playable_area")
        {
            isAlive = false;
            gameManager.enemy_gameobject_list.Remove(itself); //remove itself to stop debug null
            gameManager.unit_gameobject_list.Remove(rg2d);
            Destroy(gameObject, 0.9f); //destroy zombie when move out of the trigger area/playground
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Playable_area" && collision.gameObject.layer == 15 && isAlive) //add itself onto the list
        {
            gameManager.unit_gameobject_list.Add(rg2d);
            gameManager.enemy_gameobject_list.Add(itself);
        }
    }
}
