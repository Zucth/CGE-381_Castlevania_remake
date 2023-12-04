using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    //which cam condition was it
    [SerializeField] private bool pre_cam;
    [SerializeField] private bool cs_1;
    [SerializeField] private bool cs_2;
    [SerializeField] private bool Boss_room_bool;

    //other gameobject
    [SerializeField] GameObject player;
    [SerializeField] MainCam_Controlling cam_controller;
    [SerializeField] GameManager_controller game_manager;
    [SerializeField] player_controller controller;
    [SerializeField] player_timer timer;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] CreepController creep;
    //other gameobject, music
    [SerializeField] MusicController music_con;
    [SerializeField] SoundController sound_con;

    [SerializeField] GameObject bossfight_collider;
    [SerializeField] public GameObject blackScreen_;
    [SerializeField] GameObject prestage_Stoppoint;
    [SerializeField] Animator door1;
    [SerializeField] Animator door2;
    [SerializeField] GameObject blocking_door;

    [SerializeField] Renderer castle_front;
    //activation check
    bool isActive = true;
    bool isPlayerWalk = false;
    public int round_count = 0;
    public bool firstTime = true;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cam_controller = (MainCam_Controlling)FindObjectOfType(typeof(MainCam_Controlling));
        game_manager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        timer = (player_timer)FindObjectOfType(typeof(player_timer));
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        creep = (CreepController)FindObjectOfType(typeof(CreepController));

        music_con = (MusicController)FindObjectOfType(typeof(MusicController));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
    }

    void Update()
    {
        if (isPlayerWalk)
        {
            if (pre_cam)
            {
                if (player.transform.position.x < (prestage_Stoppoint.transform.position.x + 3.8f))
                {
                    player.transform.position += new Vector3(0.55f * 2.6f * Time.deltaTime, 0.0f, 0.0f); //3.4f = player mm_speed
                }
                else if (player.transform.position.x >= (prestage_Stoppoint.transform.position.x + 3.8f))
                {
                    player.transform.position = new Vector3((prestage_Stoppoint.transform.position.x + 3.8f), player.transform.position.y, 0);
                    //stop playing player walking animation
                }
            }
            else if (cam_controller.sp1)
            {
                if (player.transform.position.x < cam_controller.spawnpoint_stage_2.transform.position.x) //-0.3 cause of door position
                {
                    player.transform.position += new Vector3(0.85f * 3.4f * Time.deltaTime, 0.0f, 0.0f); //3.4f = player mm_speed
                }
                else if (player.transform.position.x >= cam_controller.spawnpoint_stage_2.transform.position.x)
                {
                    player.transform.position = new Vector3(cam_controller.spawnpoint_stage_2.transform.position.x, player.transform.position.y, 0);
                    //stop playing player walking animation
                }
            }
            else if (cam_controller.sp2)
            {
                if (player.transform.position.x < cam_controller.spawnpoint_stage_3.transform.position.x) //-0.3 cause of door position
                {
                    player.transform.position += new Vector3(0.85f * 3.4f * Time.deltaTime, 0.0f, 0.0f); //3.4f = player mm_speed
                }
                else if (player.transform.position.x >= cam_controller.spawnpoint_stage_3.transform.position.x)
                {
                    player.transform.position = new Vector3(cam_controller.spawnpoint_stage_3.transform.position.x, player.transform.position.y, 0);
                    //stop playing player walking animation
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (pre_cam)
            {
                //cam_controller.Number_of_scene++; //should be list as 1 after pre-stage cutscene were call
                if (isActive)
                {
                    castle_front.sortingOrder = 15;
                    isActive = false;
                    isPlayerWalk = true;
                    StartCoroutine(DelayCutscene0());
                }

            }
            else if (cs_1)
            {
                if (isActive)
                {
                    cam_controller.sp1 = true;
                    cam_controller.MainCam_Active = false; //do cam_cutscene things
                    DisableOtherSystems();
                    isActive = false;

                    cam_controller.left_x2 = 0;
                    cam_controller.right_x2 = 1;
                    StartCoroutine(DelayCutscene1());
                }
            }
            else if (cs_2)
            {
                if (isActive)
                {
                    cam_controller.sp2 = true;
                    cam_controller.MainCam_Active = false; //do cam_cutscene things
                    DisableOtherSystems();
                    isActive = false;

                    cam_controller.left_x2 = 3;
                    cam_controller.right_x2 = 4;
                    StartCoroutine(DelayCutscene2());
                }
            }
            else if (Boss_room_bool)
            {
                if (isActive)
                {
                    isActive = false;

                    StartCoroutine(EnableBossFight());

                }
            }
            else
            {
                return; //do nothing
            }
        }
    }

    public void TS_under()
    {
        creep.SpawnAble = false;
        StartCoroutine(TranslateScene_under());
    }

    private IEnumerator TranslateScene_under()
    {
        blackScreen_.SetActive(true);
        DisableOtherSystems();

        if (controller.trbr) //player stair skip movement, left up/bot right
        {
            if (controller.tr) //left up
            {
                player.transform.position = new Vector3(71.01068f, 15.49372f, player.transform.position.z);
            }
            else if (controller.bl) //bot right
            {
                player.transform.position = new Vector3(88.99933f, 13.69019f, player.transform.position.z);
            }
        }
        else if (controller.tlbl) //player stair skip movement, 
        {
            if (controller.br) //bot left
            {
                player.transform.position = new Vector3(72.65817f, 13.76111f, player.transform.position.z);
            }
            else if (controller.tl) //right up
            {
                player.transform.position = new Vector3(86.90636f, 15.71805f, player.transform.position.z);
            }
        }

        yield return new WaitForSeconds(0.3f);
        EnableOtherSystems();
        blackScreen_.SetActive(false);

        yield return new WaitForSeconds(2.2f);
        creep.SpawnAble = true;
    }
    IEnumerator DelayCutscene0() //prescene
    {
        DisableOtherSystems();

        creep.SpawnAble = false;
        yield return new WaitForSeconds(0.9f);
        sound_con.Playsound_cs_prestage_enterCastle();
        yield return new WaitForSeconds(0.8f);

        yield return new WaitForSeconds(1.1f);
        controller.anim_p.SetBool("walking", false);
        isPlayerWalk = false;
        blackScreen_.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        player.transform.position = new Vector3(cam_controller.spawnpoint_stage_1.transform.position.x, cam_controller.spawnpoint_stage_1.transform.position.y, cam_controller.spawnpoint_stage_1.transform.position.z); //atm use this for now
        cam_controller.left_x = 2;
        cam_controller.right_x = 3;

        yield return new WaitForSeconds(0.1f);
        EnableOtherSystems();
        blackScreen_.SetActive(false);
        isActive = true;
        creep.SpawnAble = true;
    }
    IEnumerator DelayCutscene1() //door1
    {
        controller.anim_p.SetBool("walking", false);
        creep.SpawnAble = false;
        yield return new WaitForSeconds(1.0f);
        sound_con.Playsound_cs_door();
        door1.SetBool("door_open", true);

        yield return new WaitForSeconds(0.9f);
        controller.anim_p.SetBool("walking", true);
        isPlayerWalk = true; //move player position till reach the spawnpoint position

        yield return new WaitForSeconds(1.2f);
        sound_con.Playsound_cs_door();
        door1.SetBool("door_open", false);

        yield return new WaitForSeconds(0.2f);
        controller.anim_p.SetBool("walking", false);
        yield return new WaitForSeconds(0.5f);
        isPlayerWalk = false;
        //stop playing player walking animation
        cam_controller.left_x2 = 1;
        cam_controller.left_x2 = 2;

        yield return new WaitForSeconds(1.9f);
        //0.1 sec delay to avoid bug
        cs_1 = false;
        blocking_door.SetActive(true);

        //main cam update
        cam_controller.left_x = 4;
        cam_controller.right_x = 5;

        yield return new WaitForSeconds(0.1f);

        EnableOtherSystems();
        scoreboard.Stage2_txt();
        cam_controller.MainCam_Active = true; //end cutscene
        cam_controller.sp1 = false;
        isActive = true;
        creep.SpawnAble = true;
    }

    IEnumerator DelayCutscene2() //door2
    {
        controller.anim_p.SetBool("walking", false);
        creep.SpawnAble = false;
        yield return new WaitForSeconds(1.0f);
        sound_con.Playsound_cs_door();
        door2.SetBool("door_open", true);

        yield return new WaitForSeconds(0.9f);
        controller.anim_p.SetBool("walking", true);
        isPlayerWalk = true; //move player position till reach the spawnpoint position
        //play player walking animation

        yield return new WaitForSeconds(1.2f);
        sound_con.Playsound_cs_door();
        door2.SetBool("door_open", false);

        yield return new WaitForSeconds(0.2f);
        controller.anim_p.SetBool("walking", false);
        yield return new WaitForSeconds(0.5f);
        isPlayerWalk = false;
        //stop playing player walking animation
        cam_controller.left_x2 = 4;
        cam_controller.left_x2 = 5;

        yield return new WaitForSeconds(1.9f);
        //0.1 sec delay to avoid bug
        cs_2 = false;
        blocking_door.SetActive(true);

        //main cam update
        cam_controller.left_x = 8;
        cam_controller.right_x = 9;

        yield return new WaitForSeconds(0.1f);

        EnableOtherSystems();
        scoreboard.Stage3_txt();
        cam_controller.MainCam_Active = true; //end cutscene
        cam_controller.sp2 = false;
        isActive = true;
        creep.SpawnAble = true;
    }

    private void DisableOtherSystems()
    {
        timer.timerIsRunning = false;
        game_manager.Set_Cutscene(); //kill everything other than player in the map
        controller.playerStatus = false;
        controller.FacingDir = false;
        controller.Walking_FlipCharacter();

        //mobs spawn things = false
        cam_controller.Item_1.SetActive(false);
        cam_controller.Item_2.SetActive(false);
        cam_controller.Item_3.SetActive(false);
    }

    private void EnableOtherSystems()
    {
        timer.timerIsRunning = true;
        //-
        controller.playerStatus = true;

        //mobs spawn things = true
        cam_controller.Item_1.SetActive(true);
        cam_controller.Item_2.SetActive(true);
        cam_controller.Item_3.SetActive(true);
    }
    IEnumerator EnableBossFight()
    {
        yield return new WaitForSeconds(0.5f);

        music_con.Boss_Music();
        bossfight_collider.SetActive(true);
        cam_controller.left_x = 9;
        cam_controller.right_x = 9;
    }
    private void ResetBossFight()
    {
        cam_controller.left_x = 8;
        cam_controller.right_x = 9;
        bossfight_collider.SetActive(false);
        isActive = true;
    }
}
