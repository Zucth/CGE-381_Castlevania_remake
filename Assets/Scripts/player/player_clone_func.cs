using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_clone_func : MonoBehaviour
{
    [SerializeField] private Transform transform_ref;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform_ref.position + offset;
    }
}
