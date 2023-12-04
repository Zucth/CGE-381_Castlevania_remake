using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Kaizo_block : MonoBehaviour, IEnemy
{
    [SerializeField] int health, Maxhealth = 1;
    [SerializeField] GameObject _dropItem1;
    [SerializeField] GameObject _dropItem2;
    [SerializeField] GameObject _dropItem3;
    [SerializeField] Collider2D _dropContainer; //maybe kept for later used.
    [SerializeField] GameObject _dropPosition;

    [SerializeField] player_controller controller; //doesn't need since it will be load later
    [SerializeField] player_subWeapon subWeapon;
    [SerializeField] SoundController sound_con;

    [SerializeField] GameObject Target_visual;

    [SerializeField] GameObject block_break; //water splash particle
    [SerializeField] Transform parent_projectile_pool; //fx_pool

    public bool emptyItem;
    public bool porkchop;
    public bool scoreDrop;
    public bool _doubleTriple_condition;
    private bool _trigger = false;
    public enum Kaizob_Type
    {
        None,
        breakable, //can be break
        trapped, //step on while attack
        plate //step on
        // etc...
    }
    public Kaizob_Type kaizoBlock_type;
    private void Start()
    {
        controller = (player_controller)FindObjectOfType(typeof(player_controller));
        subWeapon = (player_subWeapon)FindObjectOfType(typeof(player_subWeapon));
        sound_con = (SoundController)FindObjectOfType(typeof(SoundController));

        parent_projectile_pool = GameObject.FindWithTag("projectile_pool").transform;

        health = Maxhealth;
    }
    public int TakeDamage(int DamageTaken)
    {
        health -= DamageTaken;
        if (health <= 0 && kaizoBlock_type == Kaizob_Type.breakable)
        {
            sound_con.Playsound_KB_break();
            //play FX block broke
            spawn_item_whenBroke();
            Destroy(gameObject);
            //Destroy(Target_visual.gameObject);
            Target_visual.SetActive(false);
        }
        return health;
    }
    private void spawn_item_whenBroke() //can't skip up level 1 -> 3 
    {
        if (!_trigger) 
        {
            Instantiate(block_break, _dropPosition.transform.position, _dropPosition.transform.rotation, parent_projectile_pool); //play fx

            //Debug.Log("Check of this is trigger or not");
            if (subWeapon.sp_atk_delay == 3 && _doubleTriple_condition) //has none equip
            {
                GameObject item = Instantiate(_dropItem1, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                _trigger = true; //summon success
            }
            else if (subWeapon.sp_atk_delay == 2 && _doubleTriple_condition) //has double equip
            {
                GameObject item = Instantiate(_dropItem2, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                _trigger = true; //summon success
            }
            else if (subWeapon.sp_atk_delay == 1 && _doubleTriple_condition) //already has triple
            {
                GameObject item = Instantiate(_dropItem3, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                _trigger = true; //summon success
            }
            else if (scoreDrop) 
            {
                GameObject item = Instantiate(_dropItem1, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                _trigger = true; //summon success
            }
            else if (porkchop)
            {
                GameObject item = Instantiate(_dropItem1, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                _trigger = true; //summon success
            }
            else if (emptyItem)
            {
                _trigger = true; //summon success
            }
        }
    }
    private void spawn_item_Animation()
    {
        sound_con.Playsound_KB_surprise();
        GameObject item = Instantiate(_dropItem1, new Vector2(_dropPosition.transform.position.x, _dropPosition.transform.position.y), Quaternion.identity);
        _dropContainer = item.GetComponent<Collider2D>(); //get collider2d of that spawn item
        _dropContainer.isTrigger = true;
        //playsound 
        _trigger = true; //summon success
    }

    private void destroyItself()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !_trigger) //detect for player enter in that area
        {
            if((kaizoBlock_type == Kaizob_Type.plate))
            {
                spawn_item_Animation();
                destroyItself();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision) //detect player when attacking and stay in the area
    {
        if (collision.tag == "Player" && !_trigger)
        {
            if ((kaizoBlock_type == Kaizob_Type.trapped && controller.IsCrounching))
            {
                spawn_item_Animation();
                destroyItself();
            }
        }
    }
}
