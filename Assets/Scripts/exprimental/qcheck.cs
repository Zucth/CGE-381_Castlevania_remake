using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qcheck : MonoBehaviour
{

    [SerializeField] GameManager_controller gameManager;
    //private bool test;
    //private bool Ispause = false;
    private void Start()
    {
        //test = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.tag == "Player")
        {
            Debug.Log(collision.gameObject.name);
            Debug.Log(collision.gameObject.tag);
        } */

        /*
        if(collision != null)
        {
            Debug.Log(collision.gameObject.name);
            //Debug.Log(collision.gameObject.tag);
            if (test)
            {
                Debug.Log("Say hi");
                test = false;
                gameManager.FreezeAll();
                //Destroy(gameObject);
            }
            
        }*/
        /*
        if(collision.gameObject.name == "Player_Character")
        {
            Debug.Log("HelloWorld");
        } */

    }
}
