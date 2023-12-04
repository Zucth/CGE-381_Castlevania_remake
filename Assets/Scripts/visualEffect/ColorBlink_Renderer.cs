using System.Collections;
using UnityEngine;

public class ColorBlink_Renderer : MonoBehaviour
{
    [SerializeField] private Color _Start_color; //= Color(0.3f, 0.4f, 0.6f, 1f);
    [SerializeField] private Color _End_color;

    [Range(0, 15)] public float blink_speed = 6;

    [SerializeField] private Renderer target_object;
    [SerializeField] bool MoneyBag;
    [SerializeField] bool weapon_player;
    [SerializeField] bool _hurt;
    [SerializeField] bool boss_hurt;

    private bool _isActive = false;
    public bool _ColorChange = false;

    [SerializeField] player_controller controller;
    [SerializeField] Boss boss;

    private void Awake()
    {
        _Start_color = Color.white;

        if (MoneyBag)
        {
            target_object = GetComponent<Renderer>();
        }
        else if (weapon_player || _hurt)
        {
            controller = (player_controller)FindObjectOfType(typeof(player_controller));
        }
        else if (boss_hurt)
        {
            boss = (Boss)FindObjectOfType(typeof(Boss));
        }
    }
    void Update()
    {
        if (MoneyBag)
        {
            target_object.material.color = Color.Lerp(_Start_color, _End_color, Mathf.PingPong(Time.time * blink_speed, 1));
            //target_object.material.color = Color.blue;
        }
        else if (_isActive)
        {
            _isActive = false;
            _ColorChange = true;

            if (weapon_player)
            {
                StartCoroutine(WeaponBlink());
                blink_speed = 8;
            }
            else if (_hurt)
            {
                StartCoroutine(TargetHurtBlink());
                //blink_speed = 15;
            }
            else if (boss_hurt)
            {
                StartCoroutine(BossHurtBlink());
                //blink_speed = 15;
            }
        }

        if (_ColorChange)
        {
            target_object.material.color = Color.Lerp(_Start_color, _End_color, Mathf.PingPong(Time.time * blink_speed, 1));
        }
        else
        {
            return;
        }
    }
    public void ColorChanger()
    {
        _isActive = true;
    }
    private IEnumerator TargetHurtBlink()
    {
        yield return new WaitForSeconds(2.3f);
        _ColorChange = false;
        target_object.material.color = _Start_color;

    }
    private IEnumerator BossHurtBlink()
    {
        yield return new WaitForSeconds(0.3f);
        _ColorChange = false;
        target_object.material.color = _Start_color;

    }
    private IEnumerator WeaponBlink()
    {
        yield return new WaitForSeconds(0.9f);
        _ColorChange = false;
        target_object.material.color = _Start_color;

    }

}
