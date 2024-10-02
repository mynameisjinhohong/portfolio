using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAvatarPreset_H : MonoBehaviour
{
    public Sprite[] hairSprite;
    public Sprite[] faceSprite;
    public Sprite[] clothSprite;
    public Image hairImage;
    public Image faceImage;
    public Image clothImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hairImage.sprite != hairSprite[GameManager.instance.userinfo.avatarSet.head])
        {
            hairImage.sprite = hairSprite[GameManager.instance.userinfo.avatarSet.head];
        }
        if (faceImage.sprite != faceSprite[GameManager.instance.userinfo.avatarSet.face])
        {
            faceImage.sprite = faceSprite[GameManager.instance.userinfo.avatarSet.face];
        }
        if (clothImage.sprite != clothSprite[GameManager.instance.userinfo.avatarSet.body])
        {
            clothImage.sprite = clothSprite[GameManager.instance.userinfo.avatarSet.body];
        }
    }
}
