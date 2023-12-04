using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Dog_enemy : sfx_upon_destroy, IEnemy
{
    [SerializeField] int health, Maxhealth = 200;
    public int ReturnScore = 200;
    [SerializeField] private GameObject player;
    [SerializeField] float CapSpeed, speed;
    private bool Isleft;
    private bool isAlive = true;

    [SerializeField] private Animator anim; //later put animation on
    [SerializeField] private Collider2D collider2d;

    [SerializeField] GameObject itself;
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] GameManager_controller gameManager;
    //[SerializeField] Dog_enemy_activate activate;
    private bool FacingDirection;

    //raycast check ground

    [SerializeField] private float line_range; //raycast line range
    [SerializeField] private LayerMask Ground_layer; //ground layer


    //box = null then jump state true

    [SerializeField] Transform box_detect_cliff_right; //jump right 
    [SerializeField] Transform box_detect_cliff_left; //jump left 

    //enemy state control

    [SerializeField] public bool IsIdle_Phase = true; //Idle phase
    [SerializeField] private bool IsRunning_Phase = false; //running phase

    //choice state - check for 

    [SerializeField] private bool IsJumping_Phase = false; //jumping phase
    [SerializeField] private bool IsFalling_Phase = false; //falling phase

    [SerializeField] private bool isjump = false; //ability to jump - cooldown

    /// <summary> //Panther state function blueprint 
    /// 
    /// //spawn dog in idle state looking for player direction, speed = 0. till pllayer enter the player detect area ,idle_phase = false && running_phase = true
    /// running till box_check_left or right detect ground_layer = null, then do jump ^, running_phase = false && jumping_phase = true, else run forever
    /// delay 1 sec then jump_phase = false, falling phase = true, detect for ground + delay then, check player dir -> loop running
    /// 
    /// </summary> 

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        //activate = (Dog_enemy_activate)FindObjectOfType(typeof(Dog_enemy_activate));

        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();

        health = Maxhealth;
        CapSpeed = speed;

        CheckDir();
        CheckDirFacing();
        //SpawnPhase();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsIdle_Phase) // do in dog enemy active
        {
            //CheckPlayerInRange();
        }
        else if (IsRunning_Phase)
        {
            anim.SetBool("running", true);
            anim.SetBool("jumping", false);
            RunningPhase_action();
            //do something with jump check
        }
        else if (IsJumping_Phase) 
        {
            if (!isjump) //if hasn't jump yet then jump
            {
                anim.SetBool("running", false);
                anim.SetBool("jumping", true);
                Jumping_action();
                isjump = true;
            }
            StartCoroutine(ReturnJumpingPhaseDelay());
        } 
        else if (IsFalling_Phase && !IsJumping_Phase)
        {
            CheckRayCast();
            drawLine_(); //check draw line for where does it really check
        }
        /*
        else if (IsWalking_Phase) //doesn't need to check for falling_phase cause we already write a delay
        {
            WalkPhase();
        }
        else if (IsShooting_Phase && !IsWalking_Phase)
        {
            StartCoroutine(ShootingPhase());
        } */
    }

    public void ChangeFromIdletoRunning()
    {
        anim.SetBool("running", true);
        anim.SetBool("jumping", false);
        speed = CapSpeed;
        IsRunning_Phase = true;
        IsIdle_Phase = false;
    }
    void CheckGroundInRange()
    {
        Collider2D _colliders = Physics2D.OverlapCircle(box_detect_cliff_left.position,/*size*/ 0.5f, Ground_layer);
        Collider2D __colliders = Physics2D.OverlapCircle(box_detect_cliff_right.position,/*size*/ 0.5f, Ground_layer);

        if (_colliders == null || __colliders == null) //detect != ground
        {
            IsJumping_Phase = true;
            IsRunning_Phase = false;
        }
        else
        {
            return;
        }
    }

    private IEnumerator ReturnJumpingPhaseDelay()
    {
        yield return new WaitForSeconds(0.12f); //falling time 
        IsFalling_Phase = true;
        IsJumping_Phase = false;
    }

    void Jumping_action()
    {
        if (Isleft) //jump left
        {
            rg2d.velocity = new Vector2(rg2d.velocity.x + -6.3f, rg2d.velocity.y + 6.3f);
        }
        else //jump right
        {
            rg2d.velocity = new Vector2(rg2d.velocity.x + 6.3f, rg2d.velocity.y + 6.3f);
        }

    }
    void CheckRayCast()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.down, line_range, Ground_layer);

        if (hitinfo.collider != null) //hit anything in ground_layer
        {
            CheckDir();
            StartCoroutine(ReturnWalkingPhaseDelay()); //delay for falling/check dir spare time to work
            IsFalling_Phase = false;
        }
    }
    void drawLine_() //check_where line state
    {
        Debug.DrawRay(transform.position, Vector3.down * line_range, Color.red);
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
    private void CheckDirFacing()
    {
        if (Isleft)
        {
            FacingDirection = true;
            Walking_FlipCharacter();
        }
        else
        {
            FacingDirection = false;
            Walking_FlipCharacter();
        }
    }
    private IEnumerator ReturnWalkingPhaseDelay()
    {
        isjump = false; //reset jump
        yield return new WaitForSeconds(0.08f); //falling time 
        IsRunning_Phase = true;
    }
    private void RunningPhase_action()
    {
        CheckGroundInRange();
        if (Isleft)
        {
            MoveLeft();
        }
        else //is left = false, then turn right
        {
            MoveRight();
        }
    }
    public int TakeDamage(int DamageTaken)
    {
        health -= DamageTaken;
        if (isAlive)
        {
            if (health <= 0)
            {
                gameManager.enemy_gameobject_list.Remove(itself);
                gameManager.unit_gameobject_list.Remove(rg2d);
                isAlive = false;

                anim.SetBool("dead", true);
                rg2d.gravityScale = 0;
                collider2d.enabled = false;
                speed = 0;

                scoreboard.ReturnScoreFromKilling(ReturnScore); //return 200 score to player
                SummonSFX();

                Destroy(gameObject, 0.3f);
            }
        }
        
        return health;
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
    void Walking_FlipCharacter()
    {
        if (transform.localEulerAngles.y != 0 && !FacingDirection)
            transform.Rotate(0f, -180f, 0f);
        else if (transform.localEulerAngles.y != 180 && FacingDirection)
            transform.Rotate(0f, 180f, 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            if (collision.gameObject.TryGetComponent(out IPlayer playerComponent))
            {
                playerComponent.PlayerTakeDamage(40); //TakeDamage will be write and inheritance on emeny
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Playable_area")
        {
            isAlive = false;
            gameManager.enemy_gameobject_list.Remove(itself); //remove itself to stop debug null
            gameManager.unit_gameobject_list.Remove(rg2d);
            Destroy(gameObject, 0.6f); //destroy zombie when move out of the trigger area/playground
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Playable_area" && isAlive) //add itself onto the list
        {
            gameManager.unit_gameobject_list.Add(rg2d);
            gameManager.enemy_gameobject_list.Add(itself);
        }
    }

}
