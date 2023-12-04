using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class start_btn_animation : MonoBehaviour
{
    [SerializeField] TMP_Text start_text;
    public bool start_blink;
    private bool isActive = true;

    IEnumerator Blink()
    {
        start_text.text = "push start key"; //1
        yield return new WaitForSeconds(0.25f);

        start_text.text = " ";
        yield return new WaitForSeconds(0.15f);

        start_text.text = "push start key"; //2
        yield return new WaitForSeconds(0.15f);

        start_text.text = " ";
        yield return new WaitForSeconds(0.15f);

        start_text.text = "push start key"; //3
        yield return new WaitForSeconds(0.15f);

        start_text.text = " ";
        yield return new WaitForSeconds(0.15f);

        start_text.text = "push start key"; //4
        yield return new WaitForSeconds(0.15f);

        start_text.text = " ";
        yield return new WaitForSeconds(0.15f);

        start_text.text = "push start key"; //5
        yield return new WaitForSeconds(0.15f);

        start_text.text = " ";
        yield return new WaitForSeconds(0.15f);

        start_text.text = "push start key"; //6
        yield return new WaitForSeconds(0.15f);
    }

    void StartBlinking()
    {
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (start_blink)
            {
                isActive = false;
                StartBlinking();
            }
        }
    }
}
