using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class pickup_mother : MonoBehaviour
{
    /// ///////////////////////////////////////////  ITEM VISUAL / SOUND EFFECT

    [SerializeField] protected SoundController sound_con;
    [SerializeField] protected bool heart_pickup;
    [SerializeField] protected bool money_pickup;
    [SerializeField] protected bool upgrade_or_subweapon_pickup;
    [SerializeField] protected bool cross_pickup;
    [SerializeField] protected bool invincibility_pickup;

    /// /////////////////////////////////////////// ITEM BEHAVIOR (Falling, destroy itself within 5 sec

    [SerializeField] protected LayerMask Ground_Layer;
    protected const float Ground_check_radius = 0.3f;
    protected float fall_time = 5;
    protected bool Isground = false;
    [SerializeField] protected bool magic_crystal = false;
    [SerializeField] protected bool isActive = true;
    [SerializeField] protected bool Fall_delay;

    private void Start()
    {
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));
    }

    private void Awake()
    {
        if (!magic_crystal && isActive)
        {
            isActive = false;
            StartCoroutine(Destroyiteslf());
        }
        else if (magic_crystal && isActive)
        {
            isActive = false;
            StartCoroutine(MagicCrystal_fall());
        }
    }

    private void FixedUpdate()
    {
        if (!Isground)
        {
            CheckRayCast();

            if (!magic_crystal)
            {
                FallingDown();
            }
            else if (Fall_delay)
            {
                FallingDown();
            }
        }
    }
    
    protected void Playsound()
    {
        // source.Play(); of those 
        if (heart_pickup)
        {
            sound_con.Playsound_heart_pickup();
        }
        else if (money_pickup)
        {
            sound_con.Playsound_money_pickup();
        }
        else if (upgrade_or_subweapon_pickup)
        {
            sound_con.Playsound_upgrade_pickup();
        }
        else if (cross_pickup)
        {
            sound_con.Playsound_cross_pickup();
        }
        else if (invincibility_pickup)
        {
            sound_con.Playsound_invincibility_pickup();
        }
    }

    /////////////////////////////////////////////

    void FallingDown()
    {
        transform.Translate(Vector3.down * fall_time * Time.deltaTime, Space.World);
    }

    void CheckRayCast()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.down, Ground_check_radius, Ground_Layer);

        if (hitinfo.collider != null)
        {
            Isground = true;
            fall_time = 0;
        }
    }
    IEnumerator Destroyiteslf()
    {
        yield return new WaitForSeconds(5); //working just fine
        Destroy(gameObject);
    }

    IEnumerator MagicCrystal_fall()
    {
        yield return new WaitForSeconds(1.5f); //working just fine

    }


}
