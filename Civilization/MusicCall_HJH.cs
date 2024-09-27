using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCall_HJH : MonoBehaviour
{
    AudioSource BGM;
    public AudioClip[] bgmList;
    // Start is called before the first frame update
    void Start()
    {
        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        if(StageMenu_lyd.instance.whatCountry == BTNType.Korea)
        {
            BGM.clip = bgmList[0];
            BGM.Play();
        }
        else if(StageMenu_lyd.instance.whatCountry == BTNType.China)
        {
            BGM.clip = bgmList[1];
            BGM.Play();
        }
        else if(StageMenu_lyd.instance.whatCountry == BTNType.Japan)
        {
            BGM.clip = bgmList[2];
            BGM.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}