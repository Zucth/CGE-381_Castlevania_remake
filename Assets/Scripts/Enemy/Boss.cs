using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Debug = UnityEngine.Debug;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class Boss : sfx_upon_destroy, IEnemy
{
    //boss UI

    [SerializeField] private Image[] boss_healthbar_ui;
    [SerializeField] public int health, Maxhealth = 3200;

    //boss 

    public int ReturnScore = 0;
    [SerializeField] private GameObject player;
    [SerializeField] float CapSpeed, speed;
    [SerializeField] private Animator anim; //later put animation on

    [SerializeField] GameObject itself;
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] GameManager_controller gameManager;
    [SerializeField] player_endgame p_eg;
    [SerializeField] ColorBlink_Renderer colorBlink_Renderer_bossHurt;

    private bool isAlive = true;

    //bullet stuff

    [SerializeField] private GameObject _boss_bullet; //bullet gameobject
    [SerializeField] private Transform _SummonPosition; //spawn bullet position

    [SerializeField] private Transform parent_projectile_pool; //use the same as player bullet pool

    //state 
    //[SerializeField] private bool IsStandby = true; //first sleep state
    [SerializeField] private bool IsOpening; //opening acion
    [SerializeField] private bool IsIdle; //idle 1-3
    [SerializeField] private bool IsAttacking1; //deep attack
    [SerializeField] private bool IsAttacking2; // low attack
    [SerializeField] private bool IsDelayIdle; //delay after attack, before enter idle state

    //[SerializeField] private bool IsAlive = true;
    private Collider2D collider2d;

    //boss position
    [SerializeField] private GameObject Main_position;
    [SerializeField] private GameObject top_left_box; //boss
    [SerializeField] private GameObject top_right_box; //boss

    //indicator condition
    [SerializeField] private int IdleRound = 0;
    [SerializeField] private int chance;

    [SerializeField] private Vector3 Offset_deep = new Vector3(0f, -0.5f, 0f);
    [SerializeField] private Vector3 Offset_high = new Vector3(0f, 0.8f, 0f);
    [SerializeField] private Vector3 Offset_y_player = new Vector3(0f, 0f, 0f);

    //boss target position
    [SerializeField] private GameObject TargetPosition; //when random bat final target

    //boss / player position placement
    [SerializeField] private bool FirstTime = true; //-> attack first time doesn't care where player are.

    [SerializeField] public bool SameBox = false;

    //bool check for once run
    bool IsIdleActive_setup = false;
    bool IsRandom = true;
    bool IsIdleDelay = true;

    bool IsIdleRemove = true;
    bool IsLastLocate = false;
    bool IsShooting = false;
    int attackCond = 0;

    bool checkDirection = false;
    bool lastLoop = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        p_eg = (player_endgame)FindObjectOfType(typeof(player_endgame));

        anim = gameObject.GetComponent<Animator>();
        collider2d = gameObject.GetComponent<Collider2D>();

        health = Maxhealth;
        CapSpeed = speed;
        CheckDir();
    }
    void Update()
    {
        HpBarFiller(); //boss hp ui

        if (IsOpening)
        {
            //do opening animation
            transform.position += new Vector3(0.007f * speed, -0.005f, 0.0f);

            //delay as much as animation has play, the is_idle = true
        }
        else if (IsIdle && !(IsAttacking1 || IsAttacking2)) //check if boss is in left_box or right_box ->
                                                            //1st time [> indicator 40% of continue idle, 25% each of high and low swoop. 
                                                            //2nd time [> indicator 33% of continue idle, 33% each of high and low swoop.
                                                            //3rd time [> indicator 0% of continue idle, 50% each of high and low swoop.
                                                            //round check turn back to 0.
        {
            //move toward the random position of the whole top_box stop if idle time run out (2.2 seconds), then do random indicator of pattern

            if (IsIdleActive_setup)
            {
                speed = Random.Range((float)1.8, 4);
                TargetPosition.transform.position = new Vector3(Random.Range(120, (float)130.5), Random.Range((float)19, 22), 0f); //random position in the big top box
                IsIdleActive_setup = false;
                IsLastLocate = true;
                checkDirection = true;
                IsIdleRemove = true;
                lastLoop = true;
            }

            transform.position = Vector2.MoveTowards(transform.position, TargetPosition.transform.position, Time.deltaTime * speed);

            if (IsIdleDelay)
            {
                StartCoroutine(AfterIdle_Movement());
                IsIdleDelay = false;
            }
        }
        else if (IsAttacking1 && !(IsIdle)) //deep swoop (+ [-0.5] offset away from player), || target : feet
        {

            if (IsIdleRemove)
            {
                IsIdleRemove = false;
                IsIdle = false;

                speed = Random.Range((float)3.2, 5);
                attackCond = 1;
            }
            //if [doesn't care of player position] 1>, if not then do 2>, or do if player y level is above boss_y_level 3>
            //1> check if boss is in left_box or right_box -> random number of x,y of the opposite box. save number, after 
            //attack player then 2> move toward(following) that position - work in isDelayIdle
            //3> shoot at the player once. no swoop

            if (player.transform.position.y < transform.position.y) //boss.y position is higher than player
            {
                Locate_Player_LastPosition();
                transform.position = Vector2.MoveTowards(transform.position, TargetPosition.transform.position, Time.deltaTime * speed);
                
                if (TargetPosition.transform.position.y >= transform.position.y) //if hit player
                {
                    IsAttacking1 = false;
                    IsDelayIdle = true;
                } 
            }
            else if (player.transform.position.y > transform.position.y) //boss.y position is lower than player
            {
                StartCoroutine(BossShooting());
            }

            else
            {
                IsAttacking1 = false;
                StartCoroutine(DelayFor_Idle_Movement());
            }

            //Is_delay_idle = true
        }
        else if (IsAttacking2 && !(IsIdle)) //high swoop (+0.5 offset away from player), || target: head
        {
            if (IsIdleRemove)
            {
                IsIdleRemove = false;
                IsIdle = false;

                speed = Random.Range((float)2.6, 5);
                attackCond = 2;
            }

            //if player is in the same box_side as the boss then do 1>, if not then do 2>, or do if player y level is above boss_y_level 3>
            //1> check if boss is in left_box or right_box -> random number of x,y of the opposite box. save number, after 
            //attack player then 2> move toward(following) that position - work in isDelayIdle
            //3> shoot at the player once. no swoop

            if (SameBox)
            {
                if (player.transform.position.y < transform.position.y) //boss.y position is higher than player
                {
                    Locate_Player_LastPosition();
                    transform.position = Vector2.MoveTowards(transform.position, TargetPosition.transform.position, Time.deltaTime * speed);

                    if (TargetPosition.transform.position.y >= transform.position.y) //if hit player
                    {
                        IsAttacking2 = false; //it was IsAttacking1
                        IsDelayIdle = true;
                    }
                }
                else if (player.transform.position.y >= transform.position.y) //boss.y position is lower than player
                {
                    //shoot at 
                    StartCoroutine(BossShooting());
                }
                else
                {
                    Debug.Log("Error Attack_2, No loop");
                }
            }
            else //if not in the same box, return to idle
            {
                IsAttacking2 = false;
                StartCoroutine(DelayFor_Idle_Movement());
            }

            //Is_delay_idle = true
        }
        else if (IsDelayIdle) //Last_state, before return loop
        {
            //boss move up till reach offset y_position away from player last save position. 
            if (checkDirection)
            {
                checkDirection = false;
                CheckDir();
            }

            transform.position = Vector2.MoveTowards(transform.position, TargetPosition.transform.position, Time.deltaTime * speed);

            if (lastLoop)
            {
                lastLoop = false;
                StartCoroutine(LastLoop_func());
            }
            //Is_idle = true
        }
        else if (IsIdle && (IsAttacking1 || IsAttacking2)) //in any bug case!
        {
            IsIdle = false;
            IsAttacking1 = true;
        }
    }

    void HpBarFiller()
    {
        for (int i = 0; i < boss_healthbar_ui.Length; i++)
        {
            boss_healthbar_ui[i].enabled = !DisplayPlayerHp(health, i); //i = 16
        }
    }
    bool DisplayPlayerHp(float _healths, int pointNumber)
    {
        return ((pointNumber * 200)) >= _healths; //this (i * number) >= current_hp
    }

    public int TakeDamage(int DamageTaken)
    {
        health -= DamageTaken;
        if (isAlive)
        {
            if (health <= 0)
            {
                isAlive = false;
                p_eg.CrystalDrop();

                anim.SetBool("idle", false);
                anim.SetBool("dead", true);
                speed = 0; //same as freeze
                collider2d.enabled = false;

                boss_healthbar_ui[0].enabled = false;
                scoreboard.ReturnScoreFromKilling(ReturnScore);
                SummonSFX();
                OnDead();
            }
            else if(health > 0)
            {
                colorBlink_Renderer_bossHurt.ColorChanger();
                rg2d.velocity = new Vector3(0, 0, 0);
            }
        }
        return health;
    }
    private void CheckDir() //check player direction
    {
        LocatePlayer_lastPosition_for_yOffset();

        if (transform.position.x > player.transform.position.x)
        {
            TargetPosition.transform.position = new Vector3(Random.Range(120, (float)125.8), (Random.Range((float)17, 21) + Offset_y_player.y), 0f); //random position in the left top box
            //Move left
        }
        else
        {
            TargetPosition.transform.position = new Vector3(Random.Range(126, (float)130.5), (Random.Range((float)18.3, 22) + Offset_y_player.y), 0f); //random position in the right top box
            //Move right
        }
    }

    private void LocatePlayer_lastPosition_for_yOffset()
    {
        Offset_y_player = new Vector3(0f, (transform.position.y - player.transform.position.y), 0f);
    }

    private void Locate_Player_LastPosition()
    {
        if (IsLastLocate) //locate last player position + y_offset from different attack pattern
        {
            IsLastLocate = false;
            if (attackCond == 1)
            {
                TargetPosition.transform.position = new Vector3(player.transform.position.x, (player.transform.position.y + Offset_deep.y), 0f); 
            }
            else if(attackCond == 2)
            {
                TargetPosition.transform.position = new Vector3(player.transform.position.x, (player.transform.position.y + Offset_high.y), 0f); 
            }
            else
            {
                return;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPlayer playerComponent))
        {
            playerComponent.PlayerTakeDamage(20); //TakeDamage will be write and inheritance on emeny
        }
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

    private void RandomCase() //random pattern chance, after idle
    {
        chance = Random.Range(1, 101);
        IsIdle = false;

        if (IdleRound == 0) //idle 1
        {
            if (chance >= 1 && chance < 21) //suppose to be 51
            {
                StartCoroutine(DelayFor_Idle_Movement());
            }
            else if (chance >= 21 && chance < 56)
            {
                IsAttacking1 = true;
                IdleRound = -1;
            }
            else if (chance >= 56 && chance < 101)
            {
                IsAttacking2 = true;
                IdleRound = -1;
            }
        }
        else if (IdleRound == 1) //idle 2
        {
            if (chance >= 1 && chance < 9) //34
            {
                StartCoroutine(DelayFor_Idle_Movement());
            }
            else if (chance >= 10 && chance < 62)
            {
                IsAttacking1 = true;
                IdleRound = -1;
            }
            else if (chance >= 62 && chance < 101)
            {
                IsAttacking2 = true;
                IdleRound = -1;
            }
        }
        else if (IdleRound == 2) //idle 3
        {

            //fourth idle is impossible

            if (chance >= 1 && chance < 51)
            {
                IsAttacking1 = true;
                IdleRound = -1;
            }
            else if (chance >= 51 && chance < 101)
            {
                IsAttacking2 = true;
                IdleRound = -1;
            }
        }
        else
        {
            Debug.Log("IdleRound is out of limit: " + IdleRound);
            IdleRound = 0;
        }
    }

    private void OnDead() //can only be dead from killing
    {
        gameManager.enemy_gameobject_list.Remove(itself); //remove itself to stop debug null
        gameManager.unit_gameobject_list.Remove(rg2d);
        Destroy(gameObject, 1.5f);  //destroy zombie when move out of the trigger area/playground
    }

    public void TriggerBossFight() //from standby_state to opening_state
    {
        //Debug.Log("TriggerBossFight()");
        StartCoroutine(DelayAction());
    }

    public IEnumerator DelayAction() //from opening_state to idle_state
    {

        yield return new WaitForSeconds(0.4f);

        //animator - opening
        anim.SetBool("opening", true);

        yield return new WaitForSeconds(2.5f);
        //IsStandby = false;
        IsOpening = true;
        //music change

        //animator - idle
        anim.SetBool("opening", false);
        anim.SetBool("idle", true);

        yield return new WaitForSeconds(2);
        IsOpening = false;
        IsIdle = true;

        IsIdleActive_setup = true; //function work

    }

    public IEnumerator AfterIdle_Movement() //next action after idle_movement, last func of idle_state
    {
        yield return new WaitForSeconds(2.2f);

        if (FirstTime)
        {
            IsIdle = false;
            IsAttacking1 = true;

            FirstTime = false; //just to make sure that it will run only once
        }
        else if (!FirstTime && IsRandom)
        {
            RandomCase(); //next pattern
            IsRandom = false;
        }
    }

    public IEnumerator DelayFor_Idle_Movement()
    {
        yield return new WaitForSeconds(0.3f);

        IdleRound++; // if it's new start with 0 -> 1 -> 2, if it's -1 then return will be -1 -> 0 -> 1 -> 2
        IsRandom = true;
        IsIdle = true;
        IsIdleActive_setup = true;
        IsIdleDelay = true;
        IsShooting = true;
    }

    public IEnumerator BossShooting()
    {
        yield return new WaitForSeconds(0.2f);

        //shooting
        if (IsShooting)
        {
            IsShooting = false;
            Instantiate(_boss_bullet, _SummonPosition.position, _SummonPosition.rotation, parent_projectile_pool);
        }


        yield return new WaitForSeconds(0.2f);

        IsAttacking1 = false;
        IsAttacking2 = false;
        IsDelayIdle = true;
    }

    public IEnumerator LastLoop_func()
    {
        yield return new WaitForSeconds(2.2f);
        IsDelayIdle = false;
        TargetPosition.transform.position = new Vector3(0f, Random.Range((float)16.8, 20), 0f); //random position in the big top box

        yield return new WaitForSeconds(0.6f);

        StartCoroutine(DelayFor_Idle_Movement());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Playable_area") //add itself onto the list
        {
            gameManager.unit_gameobject_list.Add(rg2d);
            gameManager.enemy_gameobject_list.Add(itself);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Playable_area") //add itself onto the list
        {
            gameManager.unit_gameobject_list.Remove(rg2d);
            gameManager.enemy_gameobject_list.Remove(itself);
        }
    }
}
