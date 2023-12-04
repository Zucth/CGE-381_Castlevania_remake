using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    private ParticleSystem itself;

    private void Start()
    {
        
    }
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
