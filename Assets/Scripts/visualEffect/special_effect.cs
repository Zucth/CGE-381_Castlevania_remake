using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class special_effect : MonoBehaviour
{
    private ParticleSystem itself;
    private void Awake()
    {
        itself = GetComponent<ParticleSystem>();
        StartCoroutine(Destroyiteslf());
    }
    private void Update()
    {
        
    }
    IEnumerator Destroyiteslf()
    {
        yield return new WaitForSeconds(itself.main.startLifetime.constantMax);
        Destroy(gameObject);
    }
}
