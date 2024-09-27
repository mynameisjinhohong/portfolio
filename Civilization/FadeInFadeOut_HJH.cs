using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOut_HJH : MonoBehaviour
{
    public float animTime = 2f;
    private Text fadeText;
    Color textcolor;
    // Start is called before the first frame update
    void Start()
    {
        fadeText = GetComponent<Text>();
        textcolor = fadeText.color;
        textcolor.a = 0f;
        fadeText.color = textcolor;
        StartCoroutine(fadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator fadeIn()
    {
        float i = 0;
        while (i < 1f)
        {
            textcolor.a = i;
            fadeText.color = textcolor;
            i = i + 0.01f;
            yield return null;
        }
        StartCoroutine(fadeOut());
    }
    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(animTime);
        float i = 1;
        while (i > 0f)
        {
            textcolor.a = i;
            fadeText.color = textcolor;
            i = i - 0.01f;
            yield return null;
        }
    }
}