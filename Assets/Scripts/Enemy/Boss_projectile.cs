using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss_projectile : MonoBehaviour, IEnemy
{
    //its component

    [SerializeField] int health, Maxhealth = 100;
    public int ReturnScore = 100;

    //get itself add on the list

    [SerializeField] GameObject itself;
    [SerializeField] Rigidbody2D rg2d;

    //get other component relative

    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] GameManager_controller gameManager;
    [SerializeField] GameObject player;

    [SerializeField] float CapSpeed, speed;
    private float lifetime;
    private float lifetime_max = 3.8f;

    [SerializeField] private GameObject TargetPosition;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        TargetPosition.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0f); //only once

        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));

        lifetime = lifetime_max;
        health = Maxhealth;
        CapSpeed = speed;

        gameManager.unit_gameobject_list.Add(rg2d);
        gameManager.enemy_gameobject_list.Add(itself);
    }

    private void Update()
    {
        CheckLifeTime();

        if (transform.position != TargetPosition.transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, TargetPosition.transform.position, Time.deltaTime * speed);
        }
        else
        {
            lifetime = 0;
        }
        

        
    }

    public int TakeDamage(int DamageTaken)
    {
        health -= DamageTaken;
        if (health <= 0)
        {
            gameManager.enemy_gameobject_list.Remove(itself);
            gameManager.unit_gameobject_list.Remove(rg2d);

            scoreboard.ReturnScoreFromKilling(ReturnScore); //destroy bullet return 100 score
            Destroy(gameObject);
        }
        return health;
    }
    private void CheckLifeTime()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
        }
        else if (lifetime <= 0)
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
        rg2d.velocity = new Vector3(0, 0, 0);
        Debug.Log(itself.name + " = Freeze!");
        speed = 0;
    }
    public void StopFreeze()
    {
        Debug.Log(itself.name + " = Freeze!");
        speed = CapSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IPlayer playerComponent))
        {
            playerComponent.PlayerTakeDamage(20); //TakeDamage will be write and inheritance on emeny
            Destroy(this.gameObject);
        }
        if (collision.gameObject.layer == 6) //ground layer index
        {
            RemoveItself();
        }
    }
}
