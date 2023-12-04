using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class raycast_check : MonoBehaviour
{
    public LayerMask Ground_Layer;
    public Rigidbody2D rb2d;
    public float line_range;

    private void Start()
    {
        rb2d.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        /*
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 30f) && Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.tag == "stair_ent")
                {
                    Debug.Log("Grabbed The Key");
                }
            }
        }
        */

        drawLine_();
        CheckRayCast();
    }
    void CheckRayCast()
    {
        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.down, line_range, Ground_Layer);

        if (hitinfo.collider != null)
        {
            Debug.Log(hitinfo.transform.name);
        }
    }
    void drawLine_()
    {
        Debug.DrawRay(transform.position, Vector3.down * line_range, Color.red);
    }
}
