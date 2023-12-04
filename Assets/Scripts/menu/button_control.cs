using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class button_control : MonoBehaviour
{
    //videoplayer
    [SerializeField] VideoPlayer Opening_cutscene;
    //bool
    [SerializeField] bool MainManu;
    [SerializeField] bool RestartMenu;
    bool isActive;
    //gameobject mainmenu
    [SerializeField] GameObject video_cs;
    [SerializeField] GameObject start_txt;
    [SerializeField] GameObject credit_txt;
    //gameobject  restartScene
    [SerializeField] GameObject dead_ui;
    //other scripts
    [SerializeField] scene_controller sc_controller;
    [SerializeField] start_btn_animation start_btn;
    [SerializeField] player_controller controller;

    [SerializeField] MusicController music_con;
    [SerializeField] SoundController sound_con;

    // Start is called before the first frame update
    void Start()
    {
        if (MainManu)
        {
            start_btn = (start_btn_animation)FindObjectOfType(typeof(start_btn_animation));
        }
        else if (RestartMenu)
        {
            controller = (player_controller)FindObjectOfType(typeof(player_controller));
        }

        sc_controller = (scene_controller)FindObjectOfType(typeof(scene_controller));

        music_con = (MusicController)FindObjectOfType(typeof(MusicController));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManu)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                StartCoroutine(_Cutscene());
            }
        }
        else if (RestartMenu && isActive)
        {
           
            if (Input.GetKey(KeyCode.Keypad1)) //continue
            {
                //sc_controller.LS_InGame(); //reload the same menu
                sc_controller.LS_InGame();
                //singleton set as original
            }
            else if (Input.GetKey(KeyCode.Keypad2)) //end
            {
                sc_controller.LS_Menu();
                //add destroy singleton
            }
        }
    }

    IEnumerator _Cutscene()
    {
        start_btn.start_blink = true; //start button animation
        yield return new WaitForSeconds(1.55f);

        CloseAll();
        video_cs.SetActive(true);
        Opening_cutscene.Play(); //do cutscene

        yield return new WaitForSeconds(0.35f);
        music_con.Enter_Cutscene_Music(); //do music

        yield return new WaitForSeconds(8.85f);
        //Debug.Log("Loadsecene");
        sc_controller.LS_InGame(); // load scene
    }

    private void CloseAll()
    {
        start_txt.SetActive(false);   
        credit_txt.SetActive(false);
    }

    public void DeadUI()
    {
        isActive = true;
        music_con.Gameover_music();
        dead_ui.SetActive(true);
    }
}
