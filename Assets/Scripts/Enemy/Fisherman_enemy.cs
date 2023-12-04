using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisherman_enemy : sfx_upon_destroy, IEnemy
{
    [SerializeField] int health, Maxhealth = 300;
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
    private bool FacingDirection;

    //raycast check ground

    [SerializeField] private float line_range; //raycast line range
    [SerializeField] private float wall_range; //raycast line range
    [SerializeField] private LayerMask Ground_layer; //ground layer

    [SerializeField] private float y_check; //when reach this spot the raycast will turn on
    [SerializeField] private float jump_force = 25;

    //bullet stuff

    [SerializeField] private GameObject _Enemy_bullet; //bullet gameobject
    [SerializeField] private Transform _SummonPosition; //spawn bullet position

    [SerializeField] private Transform parent_projectile_pool; //use the same as player bullet pool

    //enemy state control

    [SerializeField] private bool IsJumping_Phase = true; //jumping phase
    [SerializeField] private bool IsFalling_Phase = false; //falling phase
    [SerializeField] private bool IsWalking_Phase = false; //walking phase
    [SerializeField] private bool IsShooting_Phase = false; //IsShooting phase

    [SerializeField] private bool OnlyShootOnce = false; //test bullet shoot or not

    //fx play

    //[SerializeField] GameObject water_splash; //water splash particle
    //[SerializeField] Transform parent_fx_pool; //fx_pool

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        parent_projectile_pool = GameObject.FindWithTag("projectile_pool").transform;
        parent_fx_pool = GameObject.FindWithTag("fx_pool").transform;

        anim = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();

        health = Maxhealth;
        CapSpeed = speed;

        SpawnPhase();
    }

    private void Update()
    {
        if (IsJumping_Phase)
        {
            if (player.transform.position.y < y_check)
            {
                IsFalling_Phase = true;
                IsJumping_Phase = false;
            }
        }
        else if (IsFalling_Phase && !IsJumping_Phase)
        {
            CheckRayCast();
        }
        else if (IsWalking_Phase) //doesn't need to check for falling_phase cause we already write a delay
        {
            anim.SetBool("idle", true);
            anim.SetBool("shoot", false);
            WalkPhase();
        }
        else if (IsShooting_Phase && !IsWalking_Phase)
        {
            
            StartCoroutine(ShootingPhase());
        }
    }
    private void SpawnPhase()
    {
        //spawn + moveup till reach the y high then "rg2d.isKinematic = false;", jumping phase = false;
        //enter falling phase till hit ground, once hit ground, falling phase = false, and do checkdir() + enter walking phase;

        rg2d.velocity = Vector2.up * jump_force;
    }
    void CheckRayCast()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.down, line_range, Ground_layer);
        Debug.DrawRay(transform.position, Vector3.down * line_range, Color.red);

        if (hitinfo.collider != null) //hit anything in ground_layer
        {
            //Debug.Log(hitinfo.transform.name);
            CheckDir();
            StartCoroutine(ReturnWalkingPhaseDelay()); //delay for falling/check dir spare time to work
            IsFalling_Phase = false;

        }
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
        StartCoroutine(GetWalkingPhaseTime()); //delay for walking time
    }

    private IEnumerator ReturnWalkingPhaseDelay()
    {
        yield return new WaitForSeconds(0.08f); //falling time 
        IsWalking_Phase = true;
    }
    private void WalkPhase()
    {
        //after do checkdir(); keep walking in that direction till, IEnumurator of 3 (+0.5 cause of the fall delay) 
        //seconds return walking phase = false, then do shoothing_phase = true
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
    private IEnumerator GetWalkingPhaseTime()
    {
        yield return new WaitForSeconds(3.0f); //walking time
        IsWalking_Phase = false;
        OnlyShootOnce = false;
        yield return new WaitForSeconds(0.5f); //entering duck before shoot
        //start duck animation
        anim.SetBool("idle", false);
        anim.SetBool("shoot", true);
        IsShooting_Phase = true;
    }

    private void Shoot()
    {
        if (!OnlyShootOnce)
        {
            OnlyShootOnce = true;
            Instantiate(_Enemy_bullet, _SummonPosition.position, _SummonPosition.rotation, parent_projectile_pool);
        }
    }

    private IEnumerator ShootingPhase()
    {
        //do IEnumurator duck, wait for 1 sec then shoot, stop ducking and go shooting_phase = false; ,back to checkdir, walking phase = true

        yield return new WaitForSeconds(0.3f);
        Shoot(); //enemy shoot projectile
        yield return new WaitForSeconds(0.5f); //delay after shoot
        IsShooting_Phase = false;
        CheckDir(); //look for player position
        IsWalking_Phase = true;
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
                speed = 0;
                collider2d.enabled = false;
                rg2d.gravityScale = 0;

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
