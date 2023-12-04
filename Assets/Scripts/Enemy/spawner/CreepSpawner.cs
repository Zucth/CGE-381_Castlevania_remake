using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CreepSpawner : MonoBehaviour
{
    //other script controller
    [SerializeField] GameManager_controller gameManager;
    [SerializeField] player_controller controller;
    [SerializeField] CreepController creep;

    //creep type
    [SerializeField] bool ghost; //6 spawn 3 layer(x2 cause of left&right)
    [SerializeField] bool bat; //2 spawn, follow player layer
    [SerializeField] bool dog; //position target
    [SerializeField] bool fisherman; //position target, detect if player has enter spawn area

    //pattern - start with 2-4, after first time pattern will be equal to 1
    [SerializeField] int pattern;

    //spawn position - follow cam gameobject? -- odd = left_side, even = right_side
    [SerializeField] private GameObject[] spawnPosition;

    //spawn object
    [SerializeField] private GameObject creep_type; //use to spawn

    //bool
    [SerializeField] bool firstTime;
    [SerializeField] int spawner_cooldown;
    [SerializeField] private bool isActive; // control from inside cooldown
    [SerializeField] bool inrange; // - check 
    private int SpawnAmount;
    private GameObject RandomGameobject;

    [SerializeField] int Round;

    private void Start()
    {
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        creep = (CreepController)FindObjectOfType(typeof(CreepController));

        if (ghost)
        {
            spawner_cooldown = 6;
        }
        else if (bat)
        {
            spawner_cooldown = 11;
        }
        else if (dog)
        {
            spawner_cooldown = 15;
        }
        else // fisherman
        {
            spawner_cooldown = 8;
        }
    }

    void Update()
    {
        if (creep.SpawnAble)
        {
            if (isActive && (gameManager.enemy_gameobject_list.Count <= 3))
            {
                isActive = false; //cancel is active right after when it hit activate
                if (Round == 1)
                {
                    Round = 0;
                    
                    if (gameManager.enemy_gameobject_list.Count == 2)
                    {
                        SpawnAmount = 1;
                    }
                    else if (gameManager.enemy_gameobject_list.Count == 1)
                    {
                        SpawnAmount = 2;
                    }
                    else if (gameManager.enemy_gameobject_list.Count == 0)
                    {
                        SpawnAmount = 3;
                    }
                    else
                    {
                        SpawnAmount = 0;
                    }

                    if (!firstTime)
                    {
                        pattern = 1;
                        firstTime = false;
                    }
                }

                if (ghost)
                {
                    if (pattern == 1)
                    {
                        //Debug.Log("do pattern 1");
                        for (int x = SpawnAmount; x >= 0; x--) //not spawn more than 3
                        {
                            int i;
                            i = Random.Range(0, spawnPosition.Length);
                            RandomGameobject = spawnPosition[i];
                            Instantiate(creep_type, RandomGameobject.transform.position, creep_type.transform.rotation);
                        }
                        StartCoroutine(SpawnCooldown());
                    }
                    else if (pattern == 2 || firstTime)
                    {
                        firstTime = false;
                        Instantiate(creep_type, spawnPosition[0].transform.position, creep_type.transform.rotation);
                        Instantiate(creep_type, spawnPosition[1].transform.position, creep_type.transform.rotation);
                        Instantiate(creep_type, spawnPosition[2].transform.position, creep_type.transform.rotation);
                        StartCoroutine(SpawnCooldown());
                    }
                    else if (pattern == 3)
                    {
                        Instantiate(creep_type, spawnPosition[2].transform.position, creep_type.transform.rotation);
                        Instantiate(creep_type, spawnPosition[5].transform.position, creep_type.transform.rotation);
                        Instantiate(creep_type, spawnPosition[4].transform.position, creep_type.transform.rotation);
                        StartCoroutine(SpawnCooldown());
                    }
                    else if (pattern == 4)
                    {
                        Instantiate(creep_type, spawnPosition[1].transform.position, creep_type.transform.rotation);
                        Instantiate(creep_type, spawnPosition[2].transform.position, creep_type.transform.rotation);
                        StartCoroutine(SpawnCooldown());
                    }
                    else
                    {
                        StartCoroutine(SpawnCooldown());
                    }
                }
                else if (bat)
                {
                    if (firstTime)
                    {
                        firstTime = false;
                        Instantiate(creep_type, spawnPosition[1].transform.position, creep_type.transform.rotation); //spawn right
                    }
                    else if (pattern == 1)

                    {
                        int i;
                        i = Random.Range(0, spawnPosition.Length);
                        RandomGameobject = spawnPosition[i];
                        Instantiate(creep_type, RandomGameobject.transform.position, creep_type.transform.rotation);

                        StartCoroutine(SpawnCooldown());
                    }
                    else
                    {
                        if (controller.FacingDir == false) //turn right
                        {
                            Instantiate(creep_type, spawnPosition[1].transform.position, creep_type.transform.rotation); //spawn right
                        }
                        else if (controller.FacingDir == true) // turn left
                        {
                            Instantiate(creep_type, spawnPosition[0].transform.position, creep_type.transform.rotation); //spawn left
                        }
                        StartCoroutine(SpawnCooldown());
                    }

                }
                else if (dog) //spawn 3 dog at position
                {
                    if (pattern == 2)
                    {
                        Instantiate(creep_type, spawnPosition[0].transform.position, creep_type.transform.rotation);
                        Instantiate(creep_type, spawnPosition[1].transform.position, creep_type.transform.rotation);
                    }
                    else if (pattern == 3)
                    {
                        Instantiate(creep_type, spawnPosition[0].transform.position, creep_type.transform.rotation);
                    }
                    else if (pattern == 1)
                    {
                        return;
                    }

                    StartCoroutine(SpawnCooldown());
                }
                else if (fisherman)
                {
                    if (pattern == 1)
                    {
                        for (int x = SpawnAmount; x >= 0; x--) //not spawn more than 3
                        {
                            int i;
                            i = Random.Range(0, spawnPosition.Length);
                            RandomGameobject = spawnPosition[i];
                            Instantiate(creep_type, RandomGameobject.transform.position, creep_type.transform.rotation);
                        }
                        StartCoroutine(SpawnCooldown());
                    }
                    else
                    {
                        Instantiate(creep_type, spawnPosition[0].transform.position, creep_type.transform.rotation);
                        Instantiate(creep_type, spawnPosition[2].transform.position, creep_type.transform.rotation);
                        StartCoroutine(SpawnCooldown());
                    }
                }
                else
                {
                    return; //other than the limit condition
                }
            }
        }
    }
    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(spawner_cooldown); //cooldown, 

        if (inrange && !dog) //dog condition wont respawn
        {
            isActive = true; //check if player still in the spawn area, if yes active return as true;
        }
        Round = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isActive = true;

            if (firstTime)
            {
                inrange = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isActive = false;
            inrange = false;
            firstTime = false;
        }

    }
}
