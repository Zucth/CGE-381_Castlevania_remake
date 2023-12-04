using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_heart_transfer : MonoBehaviour
{
    [SerializeField] player_scoreboard scoreboard;
    [SerializeField] SoundController sound_con;
    [SerializeField] player_endgame p_eg;
    public int player_heart_ = 5;

    public bool GameEnd = false;

    private void Start()
    {
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
        p_eg = (player_endgame)FindObjectOfType(typeof(player_endgame));
    }

    private void Update()
    {
        bool IsActive = true; //func only once
        if (GameEnd && IsActive)
        {
            p_eg.ScoreCalculation();
            IsActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IHeart_pickup heart_Pickup)) //this func is on player body so it should detect when it get touch by player colldier
        {
            player_heart_ += heart_Pickup.GetHeart();

            //+play sound
            Destroy(collision.gameObject);
        }
    }

    public int PlayerTakeScore_fromHeart(int heart_amount) //called active from pickup crystalball
    {
        StartCoroutine(heart_toScore(heart_amount));

        return player_heart_;
    }

    IEnumerator heart_toScore(int heartamount) //was called from PlayerTakeScore_fromHeart
    {
        //Debug.Log("player_heart: " + heartamount);

        for (int ii = heartamount; ii >= 1; ii--)
        {
            yield return new WaitForSeconds(0.2f);

            player_heart_ -= 1;
            sound_con.Playsound_HeartTally();
            scoreboard.ReturnScoreFromKilling(100);
        }

        sound_con.Playsound_HeartTallylast();
        yield return new WaitForSeconds(1.2f);
        p_eg.endCal = true;
    }
}
