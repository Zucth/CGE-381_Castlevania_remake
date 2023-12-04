using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_block : sfx_upon_destroy
{
    //[SerializeField] GameObject Prefabs_render_as;
    [SerializeField] private Animator anim; //later put animation on
    protected SpriteRenderer rend;

    [SerializeField] protected Sprite Prefabs_render_as; //what item look like?

    //[SerializeField] int health, Maxhealth = 1;
    [Tooltip("1 = First item drop priority, or weapon lv.2")]
    [SerializeField] GameObject _dropItem_1;

    [Tooltip("2 = Second item drop priority, or weapon lv.3")]
    [SerializeField] GameObject _dropItem_2;

    [Tooltip("3 = Third item drop priority(other item), or small_heart")]
    [SerializeField] GameObject _dropItem_3;

    [SerializeField] protected AudioSource source; //sound

    [SerializeField] player_mainWeapon mainWeapon;
    [SerializeField] player_heart_transfer heart_Transfer; //bring heart score

    private bool _trigger = false;
    public enum obstacle_type
    {
        None,
        small_heart, //contain small heart
        // etc...
    }
    public obstacle_type block_type;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        mainWeapon = (player_mainWeapon)FindObjectOfType(typeof(player_mainWeapon));
        heart_Transfer = (player_heart_transfer)FindObjectOfType(typeof(player_heart_transfer));

        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //rend.sprite = Prefabs_render_as; //remove "//" later
    }
    private void spawn_item_whenBroke()
    {
        if (!_trigger)
        {
            if(block_type == obstacle_type.small_heart)
            {
                //Debug.Log(mainWeapon.weapon_level_speaker);
                if (heart_Transfer.player_heart_ >= 5 && mainWeapon.weapon_level_speaker == 1)
                {
                    //Debug.Log("Condition 1 if");
                    GameObject item = Instantiate(_dropItem_1, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                    _trigger = true; //summon success
                } 
                if (heart_Transfer.player_heart_ >= 8 && mainWeapon.weapon_level_speaker == 2)
                {
                    //Debug.Log("Condition 2 if");
                    GameObject item = Instantiate(_dropItem_2, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                    _trigger = true; //summon success
                }
                if (heart_Transfer.player_heart_ < 8 && mainWeapon.weapon_level_speaker == 2)
                {
                    //Debug.Log("Condition 2 if");
                    GameObject item = Instantiate(_dropItem_3, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                    _trigger = true; //summon success
                }
                else if (mainWeapon.weapon_level_speaker == 3)
                {
                    //Debug.Log("Condition 3 if");
                    GameObject item = Instantiate(_dropItem_3, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                    _trigger = true; //summon success
                }
            }
            else //other that's not a small_heart will use this drop
            {
                //Debug.Log("Condition 3 else");
                GameObject item = Instantiate(_dropItem_3, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                _trigger = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //detect when anything that has on_weapon script enter the area will work
    {
        if (collision.TryGetComponent(out on_weapon on_main_wp) && !_trigger) 
        {
            //Debug.Log("debug fround" + on_main_wp.name);
            spawn_item_whenBroke();
            SummonSFX();
            Destroy(gameObject);
        }
    }
}
