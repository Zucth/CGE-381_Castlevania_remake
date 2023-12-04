using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class replay_controller : Singleton<replay_controller>
{
    [SerializeField] GameObject player;
    [SerializeField] player_controller controller;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] player_timer timer;
    [SerializeField] MainCam_Controlling cam_control;

    [SerializeField] GameObject BSC;

    public int _Savedcheckpoint_num = 0; //also mean stage number 1 = stage 1, number 2 = stage 2, 3... = 3, if 0 then no tp
    //public GameObject[] _Savedcheckpoint_position;

    public int _Saved_Scoreboard;

    public int _Saved_PlayerLife;

    public bool IsIngame;
    public bool firstTime = true;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsIngame)
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            else if (controller == null)
            {
                controller = (player_controller)FindObjectOfType(typeof(player_controller));
            }
            else if (scoreboard == null)
            {
                scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
            }
            else if (timer == null)
            {
                timer = (player_timer)FindObjectOfType(typeof(player_timer));
            }
            else if (cam_control == null)
            {
                cam_control = (MainCam_Controlling)FindObjectOfType(typeof(MainCam_Controlling));
            }
            else if(BSC == null)
            {
                BSC = GameObject.FindWithTag("bsc");
            }
        }
    }

    public void Called_for_restart() //called from this
    {
        StartCoroutine(DelayRestart());
        StartCoroutine(DelayBlackScreen());
    }

    public void Called_for_Wholerestart() //called from this
    {
        StartCoroutine(DelayRestart_());
        StartCoroutine(DelayBlackScreen());
    }

    IEnumerator DelayRestart_()
    {
        yield return new WaitForSeconds(0.1f);
        if (!firstTime)
        {
            ResetEverything();
        }
        yield return new WaitForSeconds(0.1f);
        CheckSpawn(); // check respawn, time, stage
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator DelayRestart()
    {
        yield return new WaitForSeconds(0.1f);
        CheckSpawn(); // check respawn, time, stage
        if (!firstTime)
        {
            called_PlayerStat_UI_Update();
        }
        yield return new WaitForSeconds(0.2f);
    }

    private void CheckSpawn()
    {
        if (_Savedcheckpoint_num == 1)
        {
            cam_control.Scene1();
            player.transform.position = new Vector2(-6.520249f, 16.04165f);
            timer.Stage1_time();
            scoreboard.Stage1_txt();
        }
        else if (_Savedcheckpoint_num == 2)
        {
            cam_control.Scene2();
            player.transform.position = new Vector2(70.26599f, 20.84754f);
            timer.Stage2_time();
            scoreboard.Stage2_txt();
        }
        else if (_Savedcheckpoint_num == 3)
        {
            cam_control.Scene3();
            player.transform.position = new Vector2(95.9413f, 20.83556f);
            timer.Stage3_time();
            scoreboard.Stage3_txt();
        }
        else
        {
            if (firstTime)
            {
                firstTime = false;
                return;
            }
            else if (!firstTime)
            {
                cam_control.Scene0();
                timer.Stage1_time();
                scoreboard.Stage1_txt();
                return;
            }

        }
    }

    private void called_PlayerStat_UI_Update()
    {
        scoreboard.player_life = _Saved_PlayerLife;
        scoreboard.player_score = _Saved_Scoreboard;
        scoreboard.Update_PLife(); //update player life
        scoreboard.AddScore(); //update player score
    }

    public void Saved_PlayerStat_UI_Update()
    {
        _Saved_PlayerLife = scoreboard.player_life;
        _Saved_Scoreboard = scoreboard.player_score;
    }

    IEnumerator DelayBlackScreen()
    {
        yield return new WaitForSeconds(0.15f);
        if (BSC == null)
        {
            BSC = GameObject.FindWithTag("bsc");
        }
        yield return new WaitForSeconds(0.03f);
        BSC.SetActive(false);
    }

    public void ResetEverything()
    {
        _Savedcheckpoint_num = 0;
        _Saved_PlayerLife = 0;
        _Saved_Scoreboard = 0;
        scoreboard.Update_PLife(); //update player life
        scoreboard.AddScore(); //update player score
    }



}
