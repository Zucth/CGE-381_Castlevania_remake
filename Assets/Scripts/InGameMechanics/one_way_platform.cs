using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class one_way_platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public bool Ondelay = false;

    public bool Onstair_One_check = false;

    [SerializeField] GameObject itself; // detect if player is transform.position.y is higher or lower than this platform
    [SerializeField] private GameObject player;

    [SerializeField] player_controller controller; //check if On_stair is true? to make this func activate
    [SerializeField] MainCam_Controlling cam_controller;

    //check if on_stair == true && player position is higher than the one_way_platform_target, then effector = 0 (so they can walk down)
    //another way is to check if on_stair == true && player position is lower than the one_way_platform_target, then effector = 180 (so they can walk up)

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();

        player = GameObject.FindWithTag("Player");
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        //cam_controller = (MainCam_Controlling)FindObjectOfType(typeof(MainCam_Controlling));
    }

    private void FixedUpdate()
    {
        if (Onstair_One_check)
        {
            //Debug.Log("Onstair_One_check");

            if (controller.on_stair == true && !Ondelay)
            {
                if (transform.position.y < player.transform.position.y)
                {
                    Ondelay = true;
                    StartCoroutine(active1()); //0 
                    StartCoroutine(delay());
                }
                else if (transform.position.y > player.transform.position.y)
                {
                    Ondelay = true;
                    StartCoroutine(active2()); //180 
                    StartCoroutine(delay());
                }
            }
        }
    }

    IEnumerator active1()
    {
        yield return new WaitForSeconds(0.12f);
        effector.rotationalOffset = 0; // - fall down
    }
    IEnumerator active2()
    {
        yield return new WaitForSeconds(0.12f);
        effector.rotationalOffset = 180; // - jump up
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(4f);
        Ondelay = false;
    }
}
