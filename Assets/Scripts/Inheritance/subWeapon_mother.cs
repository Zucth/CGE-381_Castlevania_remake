using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class subWeapon_mother : MonoBehaviour
{
    [SerializeField] protected int subweapon_Index; 
    public float sp_atk_delay = 3; //attack delay - will be -1 depend on the level of double and triple 
    protected bool sp_atk_ready = true;

    [SerializeField] protected Transform _SummonPosition;
    [SerializeField] protected Transform _SummonPosition_axe;

    [SerializeField] protected GameObject _Dagger; 
    [SerializeField] protected GameObject _Axe; 
    [SerializeField] protected GameObject _Firebomb; 
    [SerializeField] protected Transform parent_projectile_pool; 

    //timer boolean

    [SerializeField] public bool UsedTimer = false;

    //image

    [SerializeField] protected Image image; //subweapon image
    [SerializeField] protected Sprite blank;
    [SerializeField] protected Sprite dagger;
    [SerializeField] protected Sprite axe;
    [SerializeField] protected Sprite stopwatch;
    [SerializeField] protected Sprite firebomb;

    [SerializeField] protected Image image_2;
    [SerializeField] protected Sprite _double;
    [SerializeField] protected Sprite _triple;

    //other class

    [SerializeField] protected player_controller controller;
    [SerializeField] protected player_scoreboard scoreboard;
    [SerializeField] protected player_heart_transfer heart_Transfer;
    [SerializeField] protected player_timer timer;
    [SerializeField] protected GameManager_controller game_manager;
    [SerializeField] protected SoundController sound_con;
    [SerializeField] protected MusicController music_con;

    private void Start()
    {
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        scoreboard = (player_scoreboard)FindObjectOfType(typeof(player_scoreboard));
        heart_Transfer = (player_heart_transfer)FindObjectOfType(typeof(player_heart_transfer));

        timer = (player_timer)FindObjectOfType(typeof(player_timer));
        game_manager = (GameManager_controller)FindObjectOfType(typeof(GameManager_controller));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
        music_con = (MusicController)FindObjectOfType(typeof(MusicController));
    }

    protected void Empty() //0
    {
        //image.ui = none
        image.sprite = blank; //load black image instead
        image_2.sprite = blank; 
    }

    protected IEnumerator Dagger() //1
    {
        yield return new WaitForSeconds(0.51f);
        sound_con.Playsound_SW_knife();
        Instantiate(_Dagger, _SummonPosition.position, _SummonPosition.rotation, parent_projectile_pool);
        
    }
    protected IEnumerator Axe() //2
    {
        yield return new WaitForSeconds(0.51f);
        sound_con.Playsound_SW_axe();
        Instantiate(_Axe, _SummonPosition_axe.position, _SummonPosition.rotation, parent_projectile_pool);
    }
    protected void StopWatch() //3
    {
        music_con.Stopwatch_music();
        sound_con.Playsound_SW_stopwatch();
        game_manager.PauseEnemy(); //use call freeze delay 5 sec, timeRunning = true;
        timer.timerIsRunning = false; //call timer timeRunning = false;
    }
    protected IEnumerator Firebomb() //4
    {
        yield return new WaitForSeconds(0.51f);
        sound_con.Playsound_SW_firebomb();
        Instantiate(_Firebomb, _SummonPosition.position, _SummonPosition.rotation, parent_projectile_pool);
    }
    
    protected IEnumerator ResetAttack(float delay) //delay stand for last update sp_atk_delay
    {
        yield return new WaitForSeconds(delay);
        sp_atk_ready = true;
    }
}
