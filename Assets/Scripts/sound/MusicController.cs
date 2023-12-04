using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource Music_output; //MusicSpurce loop
    [SerializeField] private AudioSource Music_output_noLoop;

    [SerializeField] private AudioClip InMainMenu_Music;
    [SerializeField] private AudioClip InEnter_cs_Music;
    [SerializeField] private AudioClip InMainStage_Music;
    [SerializeField] private AudioClip InBossfight_Music;
    [SerializeField] private AudioClip IsDead_Music;
    [SerializeField] private AudioClip InGameOver_Music;
    [SerializeField] private AudioClip InFinishStage;

    [SerializeField] bool isMainMenu;
    [SerializeField] bool isEnter_cs;
    [SerializeField] bool isMainStage;
    [SerializeField] bool isBossfight;
    [SerializeField] bool isDead;
    [SerializeField] bool isGameOver;
    [SerializeField] bool isFinishStage;

    bool IsActive = false;
    private void Awake()
    {
        //Music_output = GetComponent<AudioSource>(); //there are two Audio_Source

        if (isMainMenu)
        {
            Music_output.clip = InMainMenu_Music;
            Music_output.Play();
        }
        else if (isMainStage)
        {
            Music_output.clip = InMainStage_Music;
            Music_output.Play();
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            if (isBossfight) //Main Stage
            {
                IsActive = false;
                isMainStage = false;
                Music_output.Stop();

                Music_output.clip = InBossfight_Music;
                Music_output.Play();
            }
            else if (isDead) //Main Stage
            {
                IsActive = false;
                isMainStage = false;
                Music_output.Stop();

                Music_output_noLoop.clip = IsDead_Music;
                Music_output_noLoop.Play();
            }
            else if (isEnter_cs) //Main Menu
            {
                IsActive = false;
                isMainMenu = false;
                Music_output.Stop();

                Music_output_noLoop.clip = InEnter_cs_Music;
                Music_output_noLoop.Play();
            }
            else if (isFinishStage) //Main Stage
            {
                IsActive = false;
                isMainStage = false;
                Music_output.Stop();

                Music_output_noLoop.clip = InFinishStage;
                Music_output_noLoop.Play();
            }
            else if (isGameOver)
            {
                IsActive = false;
                isMainStage = false;
                Music_output.Stop();

                Music_output.clip = InGameOver_Music;
                Music_output.Play();
            }
        }
    }

    public void Boss_Music()
    {
        isBossfight = true;
        IsActive = true;
    }
    public void Player_Dead_Music()
    {
        isBossfight = false; //to cut off boss music

        isDead = true;
        IsActive=true;
    }
    public void Enter_Cutscene_Music()
    {
        isEnter_cs = true;
        IsActive = true;
    }
    public void Player_Restart()
    {
        Music_output.Stop();

        Music_output.clip = InMainStage_Music;
        Music_output.Play();
    }
    public void StopWatch_on_ms()
    {
        Music_output.Pause();
    }
    public void StopWatch_off_ms()
    {
        Music_output.UnPause();
    }
    public void Finish()
    {
        isBossfight=false; //stop playing bossfight song

        isFinishStage = true;
        IsActive = true;
    }
    public void Gameover_music()
    {
        isDead = false;
        isGameOver = true;
        IsActive = true;
    }

    public void Stopwatch_music()
    {
        Music_output.Pause();
        StartCoroutine(_Stopwatch_music());
    }

    IEnumerator _Stopwatch_music()
    {
        yield return new WaitForSeconds(3);
        Music_output.UnPause();
    }
}
