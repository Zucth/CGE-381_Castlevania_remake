using System.Collections;
using UnityEngine;

public class sfx_weapon_hit : MonoBehaviour
{
    private ParticleSystem itself;

    [SerializeField] private GameObject taregt_fx; //hit fx sfx
    [SerializeField] private Transform parent_fx_pool; //fx_pool

    public bool IsFxDrop;
    private void Awake()
    {
        parent_fx_pool = GameObject.FindWithTag("fx_pool").transform;

        if (IsFxDrop)
        {
            itself = GetComponent<ParticleSystem>();
            StartCoroutine(Destroyiteslf());
        }
    }
    public void Summon_WeaponHit_SFX(GameObject target_position)
    {
       Instantiate(taregt_fx, target_position.transform.position, target_position.transform.rotation, parent_fx_pool);
    }

    IEnumerator Destroyiteslf()
    {
        yield return new WaitForSeconds(itself.main.startLifetime.constantMax);
        Destroy(gameObject);
    }
}
