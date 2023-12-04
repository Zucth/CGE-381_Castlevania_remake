using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_player_y : MonoBehaviour
{
    [SerializeField] private GameObject following_object; //
    [SerializeField] GameObject cam_object;
    [SerializeField] private GameObject transform_ref_y; //player y position as referenece
    void Update()
    {
        following_object.transform.position = new Vector3(cam_object.transform.position.x, transform_ref_y.transform.position.y, transform.position.z);
    }
}
