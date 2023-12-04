using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx_text : MonoBehaviour
{
    [SerializeField] private GameObject target_position; //get itself(this gameobject) position
    [SerializeField] private GameObject text_sfx; //text sfx
    [SerializeField] private Transform parent_text_pool; //fx_pool

    public bool IsTextDrop;
    private void Awake()
    {
        parent_text_pool = GameObject.FindWithTag("text_pool").transform;
        if (IsTextDrop)
        {
            StartCoroutine(Destroyiteslf());
        }
    }
    public void Summon_text_SFX()
    {
        Instantiate(text_sfx, target_position.transform.position, target_position.transform.rotation, parent_text_pool);
    }

    IEnumerator Destroyiteslf()
    {
       yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
