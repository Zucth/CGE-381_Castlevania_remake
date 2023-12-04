using System.Collections;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class player_controller : MonoBehaviour, IPlayer
{

    //float current_speed = 0;
    [SerializeField] float CapSpeed, mm_speed; //fix later
    [SerializeField] public float on_stair_mm_speed = 3; //fix later
    [SerializeField] float jump_height = 16; //fix later

    float gravityScale = 3;

    public LayerMask Stair_Layer;
    public LayerMask Ground_Layer;
    [SerializeField] Transform Ground_checker;
    [SerializeField] Transform stair_checker;
    const float Ground_check_radius = 0.5f;

    public bool FacingDir; //true when turn left, false when turn right
    public bool IsCrounching;
    public bool IsMoving = true;
    public bool IsAttack = false;
    public bool IsJumping = false;
    public bool Isground = false; //check for jump possibility when on ground only
    public bool playerStatus = true; //check if the player should be control able at the time/situation
    public bool playerstate = false; //check if player in on stair or not and which type was it ->player state of crounch, jump, walkdown, walkup = true, else false
    public bool isHurt = false;
    public bool isDead = false;
    public bool isInvincible = false;
    public bool DiedfromFalling = false;

    public bool tlbl = false; //player stair state
    public bool trbr = false; //player stair state

    public bool tl = false; //player animation state
    public bool bl = false; //player animation state
    public bool tr = false; //player animation state
    public bool br = false; //player animation state

    public bool stair_check = false; //check if player enter stair entrance
    public bool on_stair = false; //check if player on stair or not, if yes avoid player to do any other action that are on this list [jump, crounch, ground walking]

    //for extend physic and animation
    [SerializeField] private Rigidbody2D rgb2d; //player rigid body
    [SerializeField] public Animator anim_p; //later put animation on

    [SerializeField] private GameObject feet_level;
 
    [SerializeField] Transform parent_projectile_pool; //fx_pool

    //func
    public stair_condition str_cond;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] player_mainWeapon mainWeapon;
    [SerializeField] player_subWeapon subWeapon;
    [SerializeField] GameManager_controller gameManager;
    [SerializeField] SoundController sound_con;
    [SerializeField] MusicController music_con;
    [SerializeField] button_control btn_control;
    [SerializeField] player_timer timer;
    [SerializeField] replay_controller replay;
    [SerializeField] scene_controller _scene_controller;
    [SerializeField] ColorBlink_Renderer colorBlink_Renderer_hurt;

    //player Intiger
    [SerializeField] public Image[] player_healthbar_ui;
    public int player_current_hp; //set to equal to max_player_hp everytime player died
    private int max_player_hp = 160;

    public Vector3 Checkpont_Pos;
    [SerializeField] private GameObject Player_itself;

    public one_way_platform onw;
    [SerializeField] private LayerMask hidden_layer;

    //blackscreen UI
    [SerializeField] private GameObject blackscreen;
    void Start()
    {
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
        music_con = (MusicController)FindObjectOfType(typeof(MusicController));
        btn_control = (button_control)FindObjectOfType(typeof(button_control));
        timer = (player_timer)FindObjectOfType(typeof(player_timer));
        _scene_controller = (scene_controller)FindObjectOfType(typeof(scene_controller));

        gameManager.unit_gameobject_list.Add(rgb2d);
        player_current_hp = max_player_hp;
        CapSpeed = mm_speed;
        //Btn_Delay = 0;
        rgb2d = GetComponent<Rigidbody2D>();
        anim_p = GetComponent<Animator>();
    }

    void Update()
    {
        //test link
        //Debug.DrawRay(transform.position, Vector3.down *1.5f, Color.red);
        /*
        foreach (var x in enemy_collider)
        {
            Debug.Log(x.ToString());
        }
        foreach (var x in enemy_rgb2d)
        {
            Debug.Log(x.ToString());
        }*/
        //Debug.Log(tlbl + " = tlbl");
        // Debug.Log(trbr + " = trbr");

        //detect while stair_check = true 
        //in sphere range, is player closer to top_stair or bot_stair. if top_stair (walk_down = true), else if bot_stair (walk_up = true)

        //this character will move only when they are touch the ground! walk jump won't work
        //but you can walk then jump. use current speed * max speed toward the dir they facing.

        if (rgb2d.velocity == Vector2.zero && !on_stair && playerStatus) //while not on_stair or got freeze
        {
            //Debug.Log("Player isn't moving");
            anim_p.SetBool("walking", false);
            anim_p.SetBool("jump", false);
            anim_p.SetBool("walkup", false);
            anim_p.SetBool("walkdown", false);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && Isground && IsCrounching)
        {
            IsCrounching = !IsCrounching;
            anim_p.SetBool("couch", false);
        }

        if (on_stair)
        {
            rgb2d.gravityScale = 0;
        }
        else if(!on_stair)
        {
            rgb2d.gravityScale = 2;
        } 
        CheckStairID(); //stair selected
        HpBarFiller(); //player hp ui
    }
    
    private void FixedUpdate()
    {
        if (playerStatus)
        {
            if (Input.GetKey(KeyCode.U)) //just use for instant dead called.
            {
                //Debug.Log("Instant Dead");
                PlayerTakeDamage(80);
            }
            //////////////////////////////////////////////////////////////////
            else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.Z) && !stair_check)
            {
                //Debug.Log("Special Attack");
                subWeapon.SpecialWeapon_Attack(); //throw subweapon
                StartCoroutine(sp_AttackDelay()); //not really a delay, but for animation check case
            }
            else if (Input.GetKey(KeyCode.Z) && !IsAttack)
            {
                IsAttack = true;
                mainWeapon.MainWeapon_Attack(); //do attack (animation were checking in another
                StartCoroutine(AttackDelay());
            }

            ////////////////////////////////////////////////////////// LEFT

            else if (Input.GetKey(KeyCode.LeftArrow) && !IsAttack)
            {
                FacingDir = true;
                Walking_FlipCharacter();

                if (!on_stair && IsMoving)
                {
                    //rgb2d.transform.position += new Vector3(-0.85f * mm_speed * Time.deltaTime, 0.0f, 0.0f); //walk left
                    anim_p.SetBool("walking", true);
                    anim_p.SetBool("walkup", false);
                    anim_p.SetBool("walkdown", false);
                    rgb2d.velocity = new Vector3(-mm_speed, rgb2d.velocity.y, 0.0f); //walk left
                    //anim.SetBool(isWalkingHash, false);

                    if (Input.GetKey(KeyCode.X) && Isground)
                    {
                        IsMoving = false;
                        anim_p.SetBool("jump", true);
                        StartCoroutine(WalkJumping_Left_Delay());

                        //do animation
                    }
                }

                else if (on_stair)
                {
                    anim_p.SetBool("walking", false);
                    anim_p.SetBool("walkdown_idle", false);
                    anim_p.SetBool("walkup_idle", false);

                    if (tlbl)
                    {
                        bl = false;
                        tl = true;
                        anim_p.SetBool("walkup", true);
                        anim_p.SetBool("walkdown", false);
                        rgb2d.transform.position += new Vector3(-0.15f * on_stair_mm_speed * Time.deltaTime, -0.0035f * Time.deltaTime, 0.0f); //walk left up
                        //rgb2d.velocity = new Vector3(-on_stair_mm_speed, -0.0035f, 0.0f); //walk left
                    }
                    else if (trbr)
                    {
                        tl = false;
                        bl = true;
                        anim_p.SetBool("walkup", false);
                        anim_p.SetBool("walkdown", true);
                        rgb2d.transform.position += new Vector3(-0.15f * mm_speed * Time.deltaTime, -0.0165f, 0.0f); //walk left down
                        //rgb2d.velocity = new Vector3(-mm_speed, -0.0165f, 0.0f); //walk left
                    }
                }
                else
                {
                    return;
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) && on_stair)
            {
                if (playerStatus && ((tlbl && tl) || (trbr && tr)))
                {
                    anim_p.SetBool("walkup_idle", true);
                    anim_p.SetBool("walkup", false);
                    anim_p.SetBool("walkdown", false);
                }
                else if (playerStatus && (tlbl && br) || (trbr && bl))
                {
                    anim_p.SetBool("walkdown_idle", true);
                    anim_p.SetBool("walkup", false);
                    anim_p.SetBool("walkdown", false);
                }
            }
            ////////////////////////////////////////////////////////// RIGHT

            else if (Input.GetKey(KeyCode.RightArrow) && !IsAttack)
            {
                FacingDir = false;
                Walking_FlipCharacter();

                if (!on_stair && IsMoving)
                {
                    //rgb2d.transform.position += new Vector3(0.85f * mm_speed * Time.deltaTime, 0.0f, 0.0f); //walk right
                    anim_p.SetBool("walking", true);
                    anim_p.SetBool("walkup", false);
                    anim_p.SetBool("walkdown", false);
                    rgb2d.velocity = new Vector3(mm_speed, rgb2d.velocity.y, 0.0f); //walk left

                    if (Input.GetKey(KeyCode.X) && Isground)
                    {
                        IsMoving = false;
                        anim_p.SetBool("jump", true);
                        StartCoroutine(WalkJumping_Right_Delay());
                    }
                }

                else if (on_stair)
                {
                    anim_p.SetBool("walking", false);
                    anim_p.SetBool("walkdown_idle", false);
                    anim_p.SetBool("walkup_idle", false);

                    if (tlbl)
                    {
                        tr = false;
                        br = true;
                        anim_p.SetBool("walkup", false);
                        anim_p.SetBool("walkdown", true);
                        rgb2d.transform.position += new Vector3(0.15f * mm_speed * Time.deltaTime, -0.0165f, 0.0f); //walk right down
                        //rgb2d.velocity = new Vector3(mm_speed, -0.0165f, 0.0f); //walk left
                    }
                    else if (trbr)
                    {
                        br = false;
                        tr = true;
                        anim_p.SetBool("walkup", true);
                        anim_p.SetBool("walkdown", false);
                        rgb2d.transform.position += new Vector3(0.15f * on_stair_mm_speed * Time.deltaTime, -0.0035f, 0.0f); //walk up right
                        //rgb2d.velocity = new Vector3(on_stair_mm_speed, -0.0035f, 0.0f); //walk left
                    }
                }
                else
                {
                    return;
                }
            }

            else if (Input.GetKeyUp(KeyCode.RightArrow) && on_stair)
            {
                if (playerStatus && ((tlbl && tl) || (trbr && tr)))
                {
                    anim_p.SetBool("walkup_idle", true);
                    anim_p.SetBool("walkup", false);
                    anim_p.SetBool("walkdown", false);
                }
                else if (playerStatus &&  (tlbl && br) || (trbr && bl))
                {
                    anim_p.SetBool("walkdown_idle", true);
                    anim_p.SetBool("walkup", false);
                    anim_p.SetBool("walkdown", false);
                }
            }

            ////////////////////////////////////////////////////////// JUMP
            else if (Input.GetKey(KeyCode.X) && !on_stair && Isground && IsMoving)
            {
                rgb2d.gravityScale = gravityScale;
                anim_p.SetBool("jump", true);
                float jumpForce = Mathf.Sqrt(jump_height * (Physics2D.gravity.y * gravityScale) * -2f) * rgb2d.mass;
                rgb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                IsMoving = false;
                StartCoroutine(JumpingDelay());
            }
            ////////////////////////////////////////////////////////// CROUNCH
            else if (Input.GetKey(KeyCode.DownArrow) && Isground && !on_stair && !IsCrounching)
            {
                IsCrounching = true;
                anim_p.SetBool("couch", true);
            }

            ////////////////////////////////////////////////////////// STAIR
            else if (Input.GetKey(KeyCode.DownArrow) && stair_check && Isground || Input.GetKey(KeyCode.UpArrow) && stair_check && Isground)
            {
                //Function();
                on_stair = true; //on stair never set to true --> something is continue to disrupt this to true
                stair_check = false;
                //Debug.Log("Stair_available");
                str_cond.reveal_stair_collider();
                str_cond.reveal_stair_exit();

                rgb2d.velocity = new Vector2(0.0f, rgb2d.velocity.y); //stop player from slippely

                //do function player walk till reach the start stair position
                //check where player has start walking from [top/bot] remove the other trigger of re-checking to avoid infinite stair loop
                //the function will stop when player reach the end of the stair then get player out of on_stair state
                //re-open the other stair check trigger to check for start position
            }
        }

        GroundCheck(); //working just fine
        //checkforstopMoving(); //cancel 
    }

#region IEnumerator
    IEnumerator WalkJumping_Left_Delay() //use for jump attack animation
    {
        for (int x = 0; x < 8; x += 1)
        {
            rgb2d.velocity = new Vector2(-rgb2d.velocity.x, rgb2d.velocity.y + jump_height);
        }
        yield return new WaitForSeconds(1f);
        anim_p.SetBool("jump", false);
        yield return new WaitForSeconds(0.05f);
        IsMoving = true;
    }
    IEnumerator WalkJumping_Right_Delay() //use for jump attack animation
    {
        for (int x = 0; x < 8; x += 1)
        {
            rgb2d.velocity = new Vector2(rgb2d.velocity.x, rgb2d.velocity.y + jump_height);
        }
        yield return new WaitForSeconds(1f);
        anim_p.SetBool("jump", false);
        yield return new WaitForSeconds(0.05f);
        IsMoving = true;
    }

    IEnumerator JumpingDelay() //use for jump attack animation
    {
        yield return new WaitForSeconds(0.7f);
        anim_p.SetBool("jump", false);
        yield return new WaitForSeconds(0.1f);
        IsMoving = true;
    }

    IEnumerator AttackDelay() //use for attack cooldown
    {
        if (IsMoving)
        {
            rgb2d.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }

        yield return new WaitForSeconds(0.85f);
        IsAttack = false;
    }

    IEnumerator sp_AttackDelay() //use for attack cooldown
    {
        if (IsMoving)
        {
            rgb2d.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
        yield return new WaitForSeconds(0.71f);
        anim_p.SetBool("sp_atk", false);
    }
    #endregion

    public void Walking_FlipCharacter()
    {
        if (transform.localEulerAngles.y != 0 && !FacingDir)
            transform.Rotate(0f, -180f, 0f);
        else if (transform.localEulerAngles.y != 180 && FacingDir)
            transform.Rotate(0f, 180f, 0f);
    }

    void HpBarFiller()
    {
        for (int i = 0; i< player_healthbar_ui.Length; i++)
        {
            player_healthbar_ui[i].enabled = (!DisplayPlayerHp(player_current_hp, i));
        }
    }

    bool DisplayPlayerHp(float _healths, int pointNumber) 
    {
        return ((pointNumber * 10)) >= _healths; //this 10 is stand for 100%
    }  

    public int PlayerTakeDamage(int DamageTaken)
    {
        if (!isHurt && playerStatus && !isInvincible)
        {
            player_current_hp -= DamageTaken;
            PlayerHurt();
            playerStatus = false;
            if (player_current_hp <= 0)
            {

                if(replay == null)
                {
                    replay = (replay_controller)FindObjectOfType(typeof(replay_controller));
                }

                if (!DiedfromFalling)
                {
                    scoreboard.ReturnLifeUI(1);
                    anim_p.SetBool("dead", true);
                    isDead = true;
                    call_LastCheckpoint(); //set player to unplayable state, play dead animation then restart the map
                }
                else if (DiedfromFalling)
                {
                    scoreboard.ReturnLifeUI(1);
                    isDead = true;
                    StartCoroutine(DiedFromFallingDelay());
                }
                
            }
            return player_current_hp;
        }
        else
        {
            return player_current_hp;
        }
    }
    public int PlayerTakeHeal(int HealAmount) //60 & 160
    {
        StartCoroutine(healDelay(HealAmount));

        return player_current_hp;
    }

    IEnumerator DiedFromFallingDelay()
    {
        yield return new WaitForSeconds(0.05f);
        DiedfromFalling = false;
        call_LastCheckpoint();
        
    }

    IEnumerator healDelay(int healamount)
    {
        for(int i = 0; i < healamount; i++)
        {
            //Debug.Log(i + 1); //round it got played, start with 0 so round gonna be +1
            yield return new WaitForSeconds(0.5f);
            player_current_hp += 10;
            if (player_current_hp > max_player_hp)
            {
                player_current_hp = max_player_hp;
            }
        }
    }

    IEnumerator checkplayerstatus()
    {
        Player_itself.layer = LayerMask.NameToLayer("hide_player");
        yield return new WaitForSeconds(1.0f);
        anim_p.SetBool("hurt", false);
        if (!on_stair)
        {
            playerStatus = true; //regain player controller
        }
        yield return new WaitForSeconds(1.5f);
        Player_itself.layer = LayerMask.NameToLayer("Player");
        if (on_stair)
        {
            playerStatus = true; //regain player controller
        }
        isHurt = false;
    }

    void PlayerHurt()
    {
        if(player_current_hp > 0)
        {
            isHurt = true;
            colorBlink_Renderer_hurt.ColorChanger();

            if (!on_stair && !isDead) //if noton stair or already dead
            {
                anim_p.SetBool("hurt", true);
            }

            if (FacingDir && !on_stair) //face Left, knock Right
            {
                rgb2d.velocity = new Vector3(0, 0, 0);
                if (!Isground)
                {
                    rgb2d.AddForce(new Vector2(4.2f, 3.6f), ForceMode2D.Impulse);
                }
                else
                {
                    rgb2d.AddForce(new Vector2(4.2f, 8), ForceMode2D.Impulse);
                }
            }
            else if (!FacingDir && !on_stair) //face Right, knock Left
            {
                rgb2d.velocity = new Vector3(0, 0, 0);
                if (!Isground)
                {
                    rgb2d.AddForce(new Vector2(-4.2f, 3.6f), ForceMode2D.Impulse);
                }
                else
                {
                    rgb2d.AddForce(new Vector2(-4.2f, 8), ForceMode2D.Impulse);
                }
            }

            // knock was install when player collisionEnter with enemy layer (BELOW)
            sound_con.Playsound_player_hurt();
            StartCoroutine(checkplayerstatus());
        }

    }

    void CheckStairID()
    {
        Collider2D _colliders = Physics2D.OverlapCircle(stair_checker.position, 1.4f, Stair_Layer);

        
        if((_colliders != null) && _colliders.tag == "Stair_platform")
        {
            //Debug.Log("Hello World");
           //Debug.Log("raycast is hitting: " + _colliders.transform.name);
            if (_colliders.TryGetComponent<stair_condition>(out str_cond))
            {
                if (str_cond.myTypeInt == 1) //just to check the stair number [if it's match or not]
                {
                    //Debug.Log("stair type = " + str_cond.myTypeInt);
                    {
                        Collider2D __colliders = Physics2D.OverlapCircle(stair_checker.position, 6f, hidden_layer);
                        //Debug.Log("raycast is hitting: " + __colliders.transform.name);

                        if ((__colliders != null) && __colliders.tag == "One_way_platform")
                        {
                            if (__colliders.TryGetComponent<one_way_platform>(out onw))
                            {
                                onw.Onstair_One_check = true;
                                StartCoroutine(DelayOfOneWayPF());
                            }
                        }
                    }
                }

                /////////// check for side it was on

                if (str_cond.stairType == stair_condition.StairType.tl_bl && on_stair)
                {
                    tlbl = true;
                    trbr = false;
                }
                if (str_cond.stairType == stair_condition.StairType.tr_br && on_stair)
                {
                    trbr = true;
                    tlbl = false;
                }
            }
        } 
    }

    IEnumerator DelayOfOneWayPF() //
    {
        yield return new WaitForSeconds(0.5f);
        onw.Onstair_One_check = false;
    }

    private void GroundCheck() //check to stop player from double jump
    {
        IsJumping = true;
        Isground = false;
        Collider2D[] _colliders = Physics2D.OverlapCircleAll(Ground_checker.position, Ground_check_radius, Ground_Layer);
        if (_colliders.Length > 0)
        {
            Isground = true;
            IsJumping = false;
        }
    }

    public void setCheckpoint() //better in a checkpoint class and call this
    {
        Checkpont_Pos = transform.position;
    }

    IEnumerator CompletelyDead()
    {
        yield return new WaitForSeconds(1f);
        blackscreen.SetActive(true);
        gameManager.Set_Cutscene();
        btn_control.DeadUI(); //active button
    }

    IEnumerator restart_newlife()
    {
        yield return new WaitForSeconds(1f);
        blackscreen.SetActive(true);
        gameManager.Set_Cutscene();
        transform.position = Checkpont_Pos;
        //call load all gameobject.position_json file
        yield return new WaitForSeconds(0.5f);
        player_current_hp = max_player_hp;
        anim_p.SetBool("dead", false);
        isDead = false;
        playerStatus = true;
    }

    IEnumerator restart_newlife2()
    {
        yield return new WaitForSeconds(2f);
        Player_itself.layer = LayerMask.NameToLayer("Player");
        replay.Saved_PlayerStat_UI_Update(); // saved stats
        yield return new WaitForSeconds(0.15f);
        _scene_controller.LS_same();
        //blackscreen.SetActive(false);
        yield return new WaitForSeconds(0.10f);
        //timer.timerIsRunning = true;
        
    }

    private void call_LastCheckpoint()
    {
        music_con.Player_Dead_Music();
        Player_itself.layer = LayerMask.NameToLayer("hide_player");
        timer.timerIsRunning = false;

        if (scoreboard.player_life <= -1)
        {
            StartCoroutine(CompletelyDead());
        }
        else if (scoreboard.player_life >= 0)
        {
            StartCoroutine(restart_newlife()); //other status

            StartCoroutine(restart_newlife2());  //fade black -> gameplay
        }
    }

    public void WhenFreeze()
    {
        playerStatus = false;
        mm_speed = 0;
        rgb2d.isKinematic = true;
        rgb2d.velocity = new Vector3(0, 0, 0);
        anim_p.speed = 0;
    }
    public void StopFreeze()
    {
        playerStatus = true;
        mm_speed = CapSpeed;
        rgb2d.isKinematic = false;
        anim_p.speed = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player is in the stair entrance trigger area 
        if (collision.tag == "stair_ent" && !on_stair)
        {
            stair_check = true;
        }

        if ((collision.tag == "stair_exit" && on_stair))
        {
            str_cond.reset_stair_exit();
            str_cond.reset_stair_collider();
            on_stair = false;
            tlbl = false;
            trbr = false;
            tl = false;
            tr = false;
            bl = false;
            br = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if player is in the stair exit trigger area 
        if (collision.tag == "stair_ent" && !on_stair)
        {
            stair_check = false;
        }
    }
}
