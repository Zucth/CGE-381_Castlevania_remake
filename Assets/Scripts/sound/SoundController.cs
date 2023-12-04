using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    #region AudioSource
    //player
    [SerializeField] private AudioSource Sound_output_player;
    [SerializeField] private AudioSource Sound_output_whip;

    //block
    [SerializeField] private AudioSource Sound_output_block;

    //pickup
    [SerializeField] private AudioSource Sound_output_pickup; //other pickup

    //subweapon
    [SerializeField] private AudioSource Sound_output_subweapon; //stopwatch, axe, knife, firebomb

    //systems
    [SerializeField] private AudioSource Sound_output_ui; //timer, score_cal, heart_cal, water - use this as main
    #endregion

    #region sound_clip
    //sound 
    [SerializeField] private AudioClip pauseSound; //01  -- what is pause_game button anyway? 
    [SerializeField] private AudioClip timeTally; //04 X
    [SerializeField] private AudioClip heartTally; //05 X
    [SerializeField] private AudioClip heartTally_last; //06 X
    [SerializeField] private AudioClip walksound_cs; //07 -no use since no last cs
    [SerializeField] private AudioClip whip; //08 X
    [SerializeField] private AudioClip knife_throw; //09  X
    [SerializeField] private AudioClip axe_throw; //10 X
    [SerializeField] private AudioClip fall_fromHighPlace; //13 -no use, since no fall check
    [SerializeField] private AudioClip water_splash_sfx; //14 =
    [SerializeField] private AudioClip whip_hit; //20 X
    [SerializeField] private AudioClip block_breaking; //21 X
    [SerializeField] private AudioClip heart_pickup_sfx; //22 X
    [SerializeField] private AudioClip moneybag_pickup_sfx; //23 X
    [SerializeField] private AudioClip upgrade_pickup_sfx; //24 X
    [SerializeField] private AudioClip preScene_EnterCastle; //25 X
    [SerializeField] private AudioClip stopwatch_ticking; //26 X
    [SerializeField] private AudioClip Invincible_in; //27 X
    [SerializeField] private AudioClip Invincible_out; //28 X
    [SerializeField] private AudioClip door_cs_sfx; //29 X
    [SerializeField] private AudioClip holywater; //30 X
    [SerializeField] private AudioClip run_OutOfTime; //33 =
    [SerializeField] private AudioClip secretTreasure_sfx; //34 X
    [SerializeField] private AudioClip rosemary_Cross; //35 X
    [SerializeField] private AudioClip mc_hurt; //37 X
    #endregion

    //whatever happen just use .PlayOneShot instead of play!

    private void Update()
    {
        //nothing here, just empty space but leave it be for humanity sake!!!
    }

    //player
    public void Playsound_player_whip()
    {
        Sound_output_whip.PlayOneShot(whip, 1f);
    }
    public void Playsound_player_whiphit()
    {
        Sound_output_whip.PlayOneShot(whip_hit, 1f);
    }
    public void Playsound_player_hurt()
    {
        Sound_output_player.PlayOneShot(mc_hurt, 1f);
    }
    //cs
    public void Playsound_cs_prestage_enterCastle()
    {
        Sound_output_ui.PlayOneShot(preScene_EnterCastle, 1f);
    }
    public void Playsound_cs_door()
    {
        Sound_output_ui.clip = door_cs_sfx;
        Sound_output_ui.PlayDelayed(0.45f);
    }
    //tally score cal
    public void Playsound_TimeTally()
    {
        Sound_output_ui.PlayOneShot(timeTally, 1f);
    }
    public void Playsound_HeartTally()
    {
        Sound_output_ui.PlayOneShot(heartTally, 1f);
    }
    public void Playsound_HeartTallylast()
    {
        Sound_output_ui.PlayOneShot(heartTally_last, 1f);
    }
    //pickup
    public void Playsound_heart_pickup()
    {
        Sound_output_pickup.PlayOneShot(heart_pickup_sfx, 1f);
    }
    public void Playsound_money_pickup()
    {
        Sound_output_pickup.PlayOneShot(moneybag_pickup_sfx, 1f);
    }
    public void Playsound_upgrade_pickup()
    {
        Sound_output_pickup.PlayOneShot(upgrade_pickup_sfx, 1f);
    }
    public void Playsound_cross_pickup()
    {
        Sound_output_pickup.PlayOneShot(rosemary_Cross, 1f);
    }
    public void Playsound_invincibility_pickup()
    {
        Sound_output_ui.PlayOneShot(Invincible_in, 1f);
        Sound_output_pickup.clip = Invincible_out;
        Sound_output_pickup.PlayDelayed(2.0f);
    }
    //KaizoBlock
    public void Playsound_KB_break()
    {
        Sound_output_ui.PlayOneShot(block_breaking, 1f);
    }
    public void Playsound_KB_surprise()
    {
        Sound_output_ui.PlayOneShot(secretTreasure_sfx, 1f);
    }
    //subweapon
    public void Playsound_SW_knife()
    {
        Sound_output_subweapon.PlayOneShot(knife_throw, 1f);
    }
    public void Playsound_SW_axe()
    {
        StartCoroutine(SW_axe());
    }

    IEnumerator SW_axe()
    {
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
        yield return new WaitForSeconds(0.25f);
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
        yield return new WaitForSeconds(0.25f);
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
        yield return new WaitForSeconds(0.25f);
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
        yield return new WaitForSeconds(0.25f);
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
        yield return new WaitForSeconds(0.25f);
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
        yield return new WaitForSeconds(0.25f);
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
        yield return new WaitForSeconds(0.25f);
        Sound_output_subweapon.PlayOneShot(axe_throw, 1f);
    }
    public void Playsound_SW_firebomb()
    {
        Sound_output_subweapon.PlayOneShot(holywater, 1f);
    }
    public void Playsound_SW_stopwatch()
    {
        Sound_output_subweapon.PlayOneShot(stopwatch_ticking, 1f);
    }
    public void Playsound_waterSplash_sfx()
    {
        Sound_output_pickup.PlayOneShot(water_splash_sfx, 1f);
    }
    public void Playsound_Runoutoftime()
    {
        Sound_output_pickup.PlayOneShot(run_OutOfTime, 1f);
    }
}
