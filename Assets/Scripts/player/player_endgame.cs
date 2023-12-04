using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_endgame : MonoBehaviour
{
    [SerializeField] player_timer timer;
    //[SerializeField] player_scoreboard scoreboard;
    [SerializeField] player_heart_transfer heart_Transfer;
    [SerializeField] GameManager_controller gameManager;
    [SerializeField] player_controller controller;
    [SerializeField] MusicController music_con;
    [SerializeField] scene_controller scene_con;

    [SerializeField]private GameObject blackscreen;

    [SerializeField] GameObject MagicCrystal;
    [SerializeField] GameObject TargetPosition;

    public bool heartCal = false;
    public bool endCal = false;
    private bool isActive = true;

    private void Start()
    {
        timer = (player_timer)FindObjectOfType(typeof(player_timer));
        //scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        heart_Transfer = (player_heart_transfer)FindObjectOfType(typeof(player_heart_transfer));
        gameManager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        controller = (player_controller)FindObjectOfType(typeof(player_controller));

        music_con = (MusicController)FindObjectOfType(typeof(MusicController));
        scene_con = (scene_controller)FindObjectOfType(typeof(scene_controller));
    }
    private void Update()
    {
        if (heartCal)
        {
            heartCal = false;
            StartCoroutine(HeartScoreDelay());
        }
        if (endCal)
        {
            endCal = false;
            StartCoroutine(EndSceneDelay());
        }
    }

    public void ScoreCalculation()
    {
        timer.timerIsRunning = false;

        if (isActive)
        {
            isActive = false;
            StartCoroutine(TimerScoreDelay());
        }
    }

    IEnumerator TimerScoreDelay()
    {
        music_con.Finish();
        yield return new WaitForSeconds(3.2f);

        controller.playerStatus = false; 
        
        gameManager.FreezeAll();
        yield return new WaitForSeconds(0.2f);

        timer.PlayerTakeScore_fromTime(timer.second);
    }

    IEnumerator HeartScoreDelay()
    {
        heart_Transfer.PlayerTakeScore_fromHeart(heart_Transfer.player_heart_);
        yield return new WaitForSeconds(heart_Transfer.player_heart_);
    }

    IEnumerator EndSceneDelay()
    {
        yield return new WaitForSeconds(1.2f);
        blackscreen.SetActive(true);
        scene_con.LS_Menu();
        //load scene
    }

    public void CrystalDrop()
    {
        StartCoroutine(magic_crystal_drop());
    }

    IEnumerator magic_crystal_drop()
    {
        yield return new WaitForSeconds(0.8f);
        Instantiate(MagicCrystal, TargetPosition.transform.position, Quaternion.identity);
    }
}
