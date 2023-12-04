using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class on_sub_weapon : MonoBehaviour 
{
    [SerializeField] GameManager_controller gameManager; //add itself on the list
    [SerializeField] player_controller controller;
    [SerializeField] GameObject itself;
    [SerializeField] Rigidbody2D rg2d;

    private int speed = 10;
    private int Capspeed;
    private float lifetime;
    private float lifetime_max = 5;
    private Animator anim; //only get animator on Axe/Firebomb

    [SerializeField] private LayerMask Ground_Layer; //only for firebomb
    private const float Ground_check_radius = 0.3f;
    private bool Isground = false;

    public int subWeapon_DMG;
    public enum subWeapon_Type
    {
        None,
        dagger, //if other destroy itself in 8 second +dagger mm
        axe, //if other destroy itself in 8 second +axe mm
        firebomb, //if firebomb destroy itself when reach the ground or live longer than 8 second, instantiate a fire ground. 
        // etc...
    }
    public subWeapon_Type subWP_Type;

    bool isActive = false;

    private void Start()
    {
        Capspeed = speed;
    }

    private void Awake()
    {
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));

        if (subWP_Type == subWeapon_Type.axe) //one push fall. hit collider ground only, one time run
        {
            anim = GetComponent<Animator>();
            if (controller.FacingDir == true) //left
            {
                rg2d.velocity = new Vector2(rg2d.velocity.x + -2.3f, rg2d.velocity.y + 5.3f);
            }
            else if (controller.FacingDir == false) //right
            {
                rg2d.velocity = new Vector2(rg2d.velocity.x + 2.3f, rg2d.velocity.y + 5.3f);
            }
        }
        else if (subWP_Type == subWeapon_Type.firebomb) //
        {
            anim = GetComponent<Animator>();
            if (controller.FacingDir == true) //left
            {
                rg2d.velocity = new Vector2(rg2d.velocity.x + -2.9f, rg2d.velocity.y + 1.2f);
            }
            else if (controller.FacingDir == false) //right
            {
                rg2d.velocity = new Vector2(rg2d.velocity.x + 2.9f, rg2d.velocity.y + 1.2f);
            }
            
        }
        lifetime = lifetime_max;
        gameManager.unit_gameobject_list.Add(rg2d);
        gameManager.enemy_gameobject_list.Add(itself);
    }

    private void Update()
    {
        //Debug.Log(lifetime);
        if (subWP_Type == subWeapon_Type.dagger) //no gravity always throw from front size of the player (or spawn position) 
        {
            rg2d.velocity = transform.right * speed; 
        }
        else if (subWP_Type == subWeapon_Type.firebomb && isActive) //if this item is firebomb and !isground
        {
            if (!Isground)
            {
                CheckRayCast(); //check if the ground layer is under in range or not
            }
            else if (Isground)
            {
                anim.SetBool("splash", true);
                lifetime = 2f;
                isActive = false;
            }
        }
        CheckLifeTime();
    }
    void CheckRayCast() //check hit ground, then turn on ground ability
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.down, Ground_check_radius, Ground_Layer);

        if (hitinfo.collider != null)
        {
            Isground = true;
            rg2d.velocity = new Vector3(0, 0, 0); //freeze itself at that position
            rg2d.gravityScale = 0;
        }
    }

    private void CheckLifeTime()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
        }
        else if(lifetime <= 0)
        {
            RemoveItself();
        }
    }

    private void RemoveItself()
    {
        gameManager.enemy_gameobject_list.Remove(itself); //remove itself to stop debug null
        gameManager.unit_gameobject_list.Remove(rg2d);
        Destroy(gameObject);
    }

    public void WhenFreeze()
    {
        speed = 0;
        rg2d.isKinematic = true;
        rg2d.velocity = new Vector3(0, 0, 0);
    }
    public void StopFreeze()
    {
        speed = Capspeed;
        rg2d.isKinematic = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IEnemy enemyComponent)) //if take damage
        {
            enemyComponent.TakeDamage(subWeapon_DMG); //TakeDamage will be write and inheritance on emeny
            if (subWP_Type == subWeapon_Type.dagger)
            {
                Destroy(gameObject);    //destroy itself when hit enemy
            }
            else if (subWP_Type == subWeapon_Type.firebomb)
            {
                if (!Isground) //destroy itself when hit any monster while in the air...
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                return;
            }
        }
        if (collision.gameObject.layer == 6) //ground layer index [when hit ground_layer]
        {
            if (subWP_Type == subWeapon_Type.firebomb) //if this item is firebomb and !isground
            {
                isActive = true;
            }
            else
            {
                RemoveItself();
            }
        }
    }
}
