using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score_Pickup_Type", menuName ="PickUp")]
public class PickUp : ScriptableObject, IVisitor
{
    public string ScorePickup_name;
    public GameObject ScorePickup_Prefabs;
    public string ScorePickup_description;

    [Tooltip("RedBag: 100, PurpleBag: 400, WhiteBag: 700, Crowns: 1000, Treasure: 2000")]
    public int scoreFreeAmount; //drop from non-lives entity

    [Tooltip("Zombie/Projectile: 100, Dog/Bat: 200, Fisherman: 300")] //drop and delay by one second, before got add
    public int scoreDropAmount; //drop from kill whatever, 

    public void Visit(Score_PickUp score_pickup)
    {
        //later use, maybe sent the score to scoreboard class function.
        scoreFreeAmount += score_pickup.score_stored;
    }

    /*
    public int SentScoreValues(UIScore ui_score)
    {
        scoreFreeAmount + scoreDropAmount += ui_score.currentscore;
        return ui_score.currerntscore;
    } */
}
