using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBasicInfo_H : MonoBehaviour
{
    public Text capText;
    public Text cherryText;
    public Text nickNameText;
    bool setDone = false;
    void Start()
    {
        
    }
    void Update()
    {
        if(setDone == false)
        {
            SetInfo();
            if (nickNameText.text.Length > 0)
            {
                setDone = true;
            }
        }
        
    }
    void SetInfo()
    {
        capText.text = GameManager.instance.userinfo.memberCap.ToString();
        cherryText.text = GameManager.instance.userinfo.memberCherry.ToString();
        nickNameText.text = GameManager.instance.userinfo.nickname;
    }
}
