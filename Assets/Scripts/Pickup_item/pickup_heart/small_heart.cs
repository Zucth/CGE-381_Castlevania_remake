using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_heart : pickup_mother, IHeart_pickup
{
    public Animator _anim; //only small heart

    private void Awake()
    {
        fall_time = 1;
        _anim = gameObject.GetComponent<Animator>();
        _anim.SetBool("fall", true); //play fall
    }

    private void Update()
    {
         if(fall_time == 0)
        {
            _anim.SetBool("fall", false); //when object touch the ground the animation will stop
        }
    }

    public int GetHeart()
    {
        Playsound();
        return 1; //heart return
    }
}
