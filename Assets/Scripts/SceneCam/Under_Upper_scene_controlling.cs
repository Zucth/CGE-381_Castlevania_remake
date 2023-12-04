using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Under_Upper_scene_controlling : MonoBehaviour
{
    //main bool for check func // tp

    [SerializeField] private bool y2_cam; //position of that
    [SerializeField] private bool return_to_y1;

    //accept other gameobject

    [SerializeField] GameObject player;
    [SerializeField] private GameObject target_hidden_platform_1; //porkchop
    [SerializeField] private GameObject target_hidden_platform_2; //breakable block
    [SerializeField] private GameObject target_hidden_platformSkin_1; //breakable block skin 1
    [SerializeField] private GameObject target_hidden_platformSkin_2; //breakable block skin 2
    [SerializeField] private GameObject target_hidden_platformSkin_3; //breakable block skin 3

    // other function connection

    [SerializeField] MainCam_Controlling cam_controller;
    [SerializeField] CutScene cutscene;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cam_controller = (MainCam_Controlling)FindObjectOfType(typeof(MainCam_Controlling));
        //cutscene = (CutScene)FindObjectOfType(typeof(CutScene));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!y2_cam && !return_to_y1 && cutscene.round_count > 0)
            {
                cam_controller.y1 = true;
                cam_controller.left_x = 4;
                cam_controller.right_x = 5;
                cutscene.round_count = 0;

                cutscene.TS_under();
            }
            else if (y2_cam && cutscene.round_count >= 0) //under
            {
                if (cutscene.firstTime)
                {
                    cutscene.firstTime = false;
                    cutscene.round_count += 1;
                }

                cam_controller.y2 = true;
                //y2_already = true;
                cam_controller.left_x = 6;
                cam_controller.right_x = 7;
                cutscene.round_count = 1;

                cutscene.TS_under();
            }
            else if (return_to_y1)
            {
                target_hidden_platform_1.SetActive(true);
                target_hidden_platform_2.SetActive(true);
                target_hidden_platformSkin_1.SetActive(true);
                target_hidden_platformSkin_2.SetActive(true);
                target_hidden_platformSkin_3.SetActive(true);
            }
        }
    }
}
