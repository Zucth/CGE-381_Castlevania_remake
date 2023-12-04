using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCam_Controlling : MonoBehaviour
{
    [SerializeField] private Transform transform_ref_x; //player x position as reference
    //[SerializeField] private GameObject transform_ref_y; //cam y position as referenece
    [SerializeField] private GameObject MainCam; //switch up to cam stuff

    [SerializeField] private Camera cutscene_0; //cutscene | door 1-2 
    [SerializeField] private Camera cutscene_1; //cutscene | door 2-3 

    public GameObject spawnpoint_stage_1;
    public GameObject spawnpoint_stage_2;
    public GameObject spawnpoint_stage_3;

    //public GameObject bf_4;

    [SerializeField] private GameObject y_position_0; //y position test room //01.66 
    [SerializeField] private GameObject y_position_1; //y position above area //21.35 
    [SerializeField] private GameObject y_position_2; //y position under area //12.86 

    [SerializeField] private float[] camZone;
    [SerializeField] private float[] camZone2_door_cutscene;
    [SerializeField] public float camX;

    //which spawnpoint was it
    public bool sp1;
    public bool sp2;
    //public bool bf1;

    //for mainscene
    public int left_x;
    public int right_x;
    //for cutscene
    public int left_x2;
    public int right_x2;

    //public int Number_of_scene = 0; //start at 0, +1 when enter 1 scene (use this for check cam border in each scene)
    /// <summary>
    ///  prestage -> stage 1 [] Number_of_scene = 1;
    ///  cutscene_0 = [] Number_of_scene = 2;
    ///  stage 1 -> stage 2 [] Number_of_scene = 3;
    ///  cutscene_1 = [] Number_of_scene = 4;
    ///  stage 2 -> stage 3 [] Number_of_scene = 5;
    /// 
    /// </summary>
    /// 
    public bool MainCam_Active = true;


    //spawn location 1y= 17.70215, 2y=22.50754, 3y=22.49556

    public bool y0 = false;
    public bool y1 = false;
    public bool y2 = false;

    //item_family
    [SerializeField] public GameObject Item_1;
    [SerializeField] public GameObject Item_2;
    [SerializeField] public GameObject Item_3;

    public bool isActivate = false; //set as false after activate again when enter cutscene

    private void Start()
    {
        left_x = 0; //0
        right_x = 1; //1
    }
    private void FixedUpdate()
    {
        if (y0 && !isActivate)
        {
            MainCam.transform.position = new Vector3(transform.position.x, y_position_0.transform.position.y, transform.position.z);
            isActivate = true;
            y0 = false;
            StartCoroutine(delayCam());
        }
        else if (y1 && !isActivate)
        {
            MainCam.transform.position = new Vector3(transform.position.x, y_position_1.transform.position.y, transform.position.z);
            isActivate = true;
            y1 = false;
            StartCoroutine(delayCam());
        }
        else if (y2 && !isActivate)
        {
            MainCam.transform.position = new Vector3(transform.position.x, y_position_2.transform.position.y, transform.position.z);
            isActivate = true;
            y2 = false;
            StartCoroutine(delayCam());
        }
        /*
        if (cam clamp position)
        {

        }*/
    }

    void Update()
    {
        if (MainCam_Active)
        {
            camX = transform_ref_x.transform.position.x;
            camX = Mathf.Clamp(camX, camZone[left_x], camZone[right_x]);

            //MainCam.transform.position = new Vector3(transform_ref_x.position.x, MainCam.transform.position.y, -10); //free border cam
            MainCam.transform.position = new Vector3(camX, MainCam.transform.position.y, -10); //real_limit border cam
        }
        else if(!MainCam_Active)
        {
            if (sp1)
            {
                camX = spawnpoint_stage_2.transform.position.x;
            }
            else if (sp2)
            {
                camX = spawnpoint_stage_3.transform.position.x;
            }
            /*
            else if (bf1)
            {
                //camX = camZone2_door_cutscene[];
            } */

            camX = Mathf.Clamp(camX, camZone2_door_cutscene[left_x2], camZone2_door_cutscene[right_x2]);

            if(MainCam.transform.position.x < camX) //if x position of right cam is lower than limit keep moving right
            {
                MainCam.transform.position += new Vector3(3.55f * Time.deltaTime, 0f, 0f);
            }
            else if(MainCam.transform.position.x >= camX)
            {
                MainCam.transform.position = new Vector3(camX, MainCam.transform.position.y, -10);
            }
        }
        
    }

    IEnumerator delayCam()
    {
        yield return new WaitForSeconds(1f);
        isActivate = false;
    }

    public void Scene0() //pre-stage
    {
        left_x = 0;
        right_x = 1;
    }
    public void Scene1() //stage 1
    {
        left_x = 2;
        right_x = 3;
    }
    public void Scene2() //stage 2
    {
        left_x = 4;
        right_x = 5;
    }
    public void Scene3() //satge 3
    {
        left_x = 8;
        right_x = 9;
    }
}
