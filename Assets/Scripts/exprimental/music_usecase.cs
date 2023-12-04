using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_usecase : MonoBehaviour
{
    [SerializeField] MusicController music_con;
    [SerializeField] SoundController sound_con;

    public bool boss_mc;
    public bool finish_game;

    public bool use_whipsound;

    [SerializeField] private AudioSource output_sound_exprimental; //MusicSpurce loop
    [SerializeField] AudioClip clip;
    [SerializeField] bool isActive;

    private void Start()
    {
        music_con = (MusicController)FindObjectOfType(typeof(MusicController));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
    }
    void Update()
    {
        if (boss_mc)
        {
            boss_mc = false;
            music_con.Boss_Music();
        }
        else if (finish_game)
        {
            finish_game = false;
            music_con.Finish();
        }else if (use_whipsound)
        {
            use_whipsound = false;
            sound_con.Playsound_player_whip();
        }


        //test sound
        if (isActive)
        {
            isActive = false;
            output_sound_exprimental.PlayOneShot(clip, 1f);
        }
    }
}
