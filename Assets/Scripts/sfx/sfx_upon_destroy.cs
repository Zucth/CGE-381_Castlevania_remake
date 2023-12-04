using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx_upon_destroy : MonoBehaviour
{
    [SerializeField] protected GameObject itself_sfx; //get itself(this gameobject) position
    [SerializeField] protected GameObject dead_sfx; //dead sfx
    [SerializeField] protected Transform parent_fx_pool; //fx_pool
    private void Awake()
    {
        parent_fx_pool = GameObject.FindWithTag("fx_pool").transform;
    }

    protected void SummonSFX()
    {
        Instantiate(dead_sfx, itself_sfx.transform.position, itself_sfx.transform.rotation, parent_fx_pool);
    }

}
