using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class player_scoreboard : MonoBehaviour
{
    public int player_score = 0; //player main score stored
    public int player_life = 3;
    //private int player_stage = 1;

    //[SerializeField] private Image image;

    [SerializeField] private TMP_Text tmp_player_score; //player when change number to score
    [SerializeField] private TMP_Text tmp_player_life; //player when change number to score
    [SerializeField] private TMP_Text tmp_player_heart; //player when change number to score
    [SerializeField] private TMP_Text tmp_player_stage; //player when change number to score

    //private GameObject catch_the_GameManager;

    [SerializeField] player_heart_transfer heart_Transfer;

    private void Start()
    {

    }

    private void Update()
    {
        AddScore();
        Update_PLife(); 
        Update_PHeart();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IScore_Pickup score_pickup)) //this func is on player body so it should detect when it get touch by player colldier
        {
            player_score += score_pickup.GetScore();

            //+play sound
            Destroy(collision.gameObject);
        }
    }

    public void ReturnScoreFromKilling(int score)
    {
        player_score += score;
    }

    public void AddScore()
    {

        if (player_score <= 9)
        {
            tmp_player_score.text = "Score-00000" + player_score.ToString();
        }
        if (player_score <= 999) //0.99k
        {
            if (player_score >= 100)
            {
                tmp_player_score.text = "Score-000" + player_score.ToString();
            }
        }
        if (player_score <= 9999) //1k
        {
            if (player_score >= 1000)
            {
                tmp_player_score.text = "Score-00" + player_score.ToString();
            }
        }
        if (player_score <= 99999) //10k
        {
            if (player_score >= 10000)
            {
                tmp_player_score.text = "Score-0" + player_score.ToString();
            }
        }
        if (player_score <= 999999) //100k
        {
            if (player_score >= 100000)
            {
                tmp_player_score.text = "Score-" + player_score.ToString();
            }
        }
        return;
    }
    public void ReturnLifeUI(int life)
    {
        player_life -= life;
    }

    public void Update_PLife()
    {
        if (player_life >= 0)
        {
            tmp_player_life.text = "P-0" + player_life.ToString();
        }
        else if (player_life <= -1)
        {
            tmp_player_life.text = "P-00";
        }
    }

    public void Update_PHeart()
    {
        if (heart_Transfer.player_heart_ <= 9)
        {
            tmp_player_heart.text = "-0" + heart_Transfer.player_heart_.ToString();
        }
        if (heart_Transfer.player_heart_ <= 99) 
        {
            if (heart_Transfer.player_heart_ >= 10)
            {
                tmp_player_heart.text = "-" + heart_Transfer.player_heart_.ToString();
            }
        }
        if (heart_Transfer.player_heart_ >= 100) //max at 99
        {
            heart_Transfer.player_heart_ = 99;
            tmp_player_heart.text = "-" + heart_Transfer.player_heart_.ToString();
        }
    }

    public void Stage1_txt()
    {
        tmp_player_stage.text = "STAGE  01";
    }
    public void Stage2_txt()
    {
        tmp_player_stage.text = "STAGE  02";
    }
    public void Stage3_txt()
    {
        tmp_player_stage.text = "STAGE  03";
    }


}

