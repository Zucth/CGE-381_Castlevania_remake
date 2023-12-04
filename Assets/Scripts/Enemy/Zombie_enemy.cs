using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie_enemy : sfx_upon_destroy, IEnemy
{

    [SerializeField] int health, Maxhealth =100;
    public int ReturnScore = 100;
    [SerializeField] private GameObject player;
    [SerializeField] float CapSpeed, speed;
    private bool Isleft;
    [SerializeField] private Animator anim; //later put animation on
    [SerializeField] private Collider2D collider2d;

    [SerializeField] private float wall_range; //raycast line range
    [SerializeField] private LayerMask Ground_layer; //ground layer

    [SerializeField] GameObject itself;
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] GameManager_controller gameManager;

    [SerializeField] bool IsAlive = true; //consider if this enemy should still be able to do anything

    private bool FacingDirection;

    //public AudioSource enemy_dead;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));

        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();    

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
            CheckRayCastLeft();
        }
        else //is left = false, then turn right
        {
            MoveRight();
            CheckRayCastRight();
        }
    }
    public int TakeDamage(int DamageTaken)
    {
        if (IsAlive)
        {
            health -= DamageTaken;
            rg2d.velocity = new Vector3(0, 0, 0); //when hurt they will stand still for 0.2 sec 

            if (health <= 0)
            {
                gameManager.enemy_gameobject_list.Remove(itself);
                gameManager.unit_gameobject_list.Remove(rg2d);

                anim.SetBool("hurt", true);
                speed = 0;
                //collider check
                collider2d.enabled = false;
                rg2d.gravityScale = 0;

                scoreboard.ReturnScoreFromKilling(ReturnScore);
                SummonSFX();
                IsAlive = false; //check if this is working properly

                Destroy(gameObject, 0.3f);
            }
        }
        return health;
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
        //rg2d.AddForce(new Vector2(-speed, 0f)); //doesn't work


    }
    private void MoveRight()
    {
        FacingDirection = false;
        Walking_FlipCharacter();
        transform.position += new Vector3(0.85f * speed * Time.deltaTime, 0.0f, 0.0f);
        //rg2d.AddForce(new Vector2(mm_speed, 0f));
    }
    void CheckRayCastLeft()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.left, wall_range, Ground_layer);
        //Debug.DrawRay(transform.position, Vector2.left * wall_range, Color.red);

        if (hitinfo.collider != null) //hit anything in ground_layer
        {
            Isleft = false;
        }
    }
    void CheckRayCastRight()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.right, wall_range, Ground_layer);
        //Debug.DrawRay(transform.position, Vector2.right * wall_range, Color.red);

        if (hitinfo.collider != null) //hit anything in ground_layer
        {
            Isleft = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsAlive)
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
        rg2d.isKinematic = true;
        rg2d.velocity = new Vector3(0, 0, 0);
        anim.speed = 0;
    }
    public void StopFreeze()
    {
        speed = CapSpeed;
        rg2d.isKinematic = false;
        anim.speed = 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Playable_area")
        {
            IsAlive = false;
            gameManager.enemy_gameobject_list.Remove(itself); 
            gameManager.unit_gameobject_list.Remove(rg2d);
            Destroy(gameObject, 0.6f); //destroy zombie when move out of the trigger area/playground
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Playable_area" && IsAlive) //add itself onto the list
        {
            gameManager.unit_gameobject_list.Add(rg2d);
            gameManager.enemy_gameobject_list.Add(itself);
        }
    }
}
