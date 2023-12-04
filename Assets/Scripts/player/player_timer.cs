using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel;

public class player_timer : MonoBehaviour
{
    //

    [SerializeField] private TMP_Text tmp_game_time;
    [SerializeField] player_controller controller;
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] player_endgame p_eg;
    [SerializeField] SoundController sound_con;

    //

    private bool InStage1 = false;
    private bool InStage2 = false;
    private bool InStage3 = false;
    public int second;

    private bool Is_outOftime = false;
    bool _isActive = true;

    //

    public float timeRemaining = 300;
    public bool timerIsRunning = false;

    void Start()
    {
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        p_eg = (player_endgame)FindObjectOfType(typeof(player_endgame));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));

        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                Update_Timer_UI();

                if (timeRemaining <= 30 && timeRemaining > 0)
                {
                    if (_isActive)
                    {
                        Is_outOftime = true;
                        _isActive = false;
                        StartCoroutine(SoundDelay());
                    }
                }
                else
                {
                    if (!_isActive)
                    {
                        Is_outOftime = false;
                        _isActive = true;
                    }
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                controller.PlayerTakeDamage(200);
            }
        }
        else if (!timerIsRunning)
        {
            if (timeRemaining <= 30 && timeRemaining > 0)
            {
                if (_isActive)
                {
                    Is_outOftime = false;
                }
            }
        }
    }

    IEnumerator SoundDelay()
    {
        while (Is_outOftime)
        {
            sound_con.Playsound_Runoutoftime();
            yield return new WaitForSeconds(1f);
        }
    }

    public void CheckStage_Respawn()
    {
        if (InStage1)
        {
            Stage1_time();
        }
        else if (InStage2)
        {
            Stage2_time();
        }
        else if (InStage3)
        {
            Stage3_time();   
        }
    }

    public void Stage1_time()
    {
        timeRemaining = 301;
        StartCoroutine(delay());
    }
    public void Stage2_time()
    {
        timeRemaining = 201;
        StartCoroutine(delay());
    }
    public void Stage3_time()
    {
        timeRemaining = 101;
        StartCoroutine(delay());
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1.2f);
        timerIsRunning = true;
    }

    public void Update_Timer_UI()
    {
        second = (int)timeRemaining;


        if (second <= 9) //0.009k
        {
            tmp_game_time.text = "Time 000" + second.ToString();
        }
        if (second <= 99) //0.09k
        {
            if (second >= 10)
            {
                tmp_game_time.text = "Time 00" + second.ToString();
            }
        }
        if (second <= 999) //0.9k
        {
            if (second >= 100)
            {
                tmp_game_time.text = "Time 0" + second.ToString();
            }
        }
    }
    public int PlayerTakeScore_fromTime(int time_amount) //called active from pickup crystalball
    {
        StartCoroutine(heart_toScore(time_amount));

        return time_amount;
    }

    IEnumerator heart_toScore(int timeamount) //was called from PlayerTakeScore_fromHeart
    {
        //Debug.Log("Second left: " + timeamount);

        for (int ii = timeamount; ii >= 1; ii--)
        {
            yield return new WaitForSeconds(0.08f);

            second -= 1;

            if (second <= 9) //0.009k
            {
                tmp_game_time.text = "Time 000" + second.ToString();
            }
            if (second <= 99) //0.09k
            {
                if (second >= 10)
                {
                    tmp_game_time.text = "Time 00" + second.ToString();
                }
            }
            if (second <= 999) //0.9k
            {
                if (second >= 100)
                {
                    tmp_game_time.text = "Time 0" + second.ToString();
                }
            }
            sound_con.Playsound_TimeTally();
            scoreboard.ReturnScoreFromKilling(10);
        }
        yield return new WaitForSeconds(0.5f);
        p_eg.heartCal = true;
    }
}
