using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HJH_FadeInOut : MonoBehaviour
{
    public bool prologueScene = true;
    public AudioClip[] ac;
    public Image[] stroyImages;
    public Canvas canvas;
    AudioSource au;
    float curTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        au = Camera.main.GetComponent<AudioSource>();
        if (ac.Length > 1)
        {
            au.clip = ac[0];

        }
        au.Play();
    }
    int count = 0;
    public float fadeSpeed = 1;
    public float nextTime = 3;
    public float secondToThird = 3;
    float time = 0;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > nextTime)
        {
            if (prologueScene == true)
            {
                nextTime -= secondToThird;
                prologueScene = false;
            }
            StartCoroutine(Fade());
            if (ac.Length > 0)
            {
                au.clip = ac[count + 1];
                au.Play();
            }
            time = 0;
        }
        if (count == 1)
        {
            curTime += Time.deltaTime;
            if (curTime > 6)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                curTime = 0;
            }
        }
    }
    IEnumerator Fade()
    {
        while (true)
        {
            time = 0;
            Color co = stroyImages[count].color;
            co.a -= Time.deltaTime * fadeSpeed;
            stroyImages[count].color = co;
            stroyImages[count + 1].gameObject.SetActive(true);
            co = stroyImages[count + 1].color;
            co.a += Time.deltaTime * fadeSpeed;
            stroyImages[count + 1].color = co;
            if (co.a > 0.95f)
            {
                stroyImages[count].gameObject.SetActive(false);
                if (count < stroyImages.Length - 2)
                {
                    count++;
                    time = 0;
                    yield break;
                }
            }
            yield return null;
        }

    }
}