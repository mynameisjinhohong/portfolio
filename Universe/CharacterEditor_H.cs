using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditor_H : MonoBehaviour
{
    int nowHead;
    int nowFace;
    int nowCloth;
    private void OnEnable()
    {
        nowHead = GameManager.instance.userinfo.avatarSet.head;
        nowFace = GameManager.instance.userinfo.avatarSet.face;
        nowCloth = GameManager.instance.userinfo.avatarSet.body;
    }
    public void XButton()
    {
        GameManager.instance.userinfo.avatarSet.head = nowHead;
        GameManager.instance.userinfo.avatarSet.face = nowFace;
        GameManager.instance.userinfo.avatarSet.body = nowCloth;
        gameObject.SetActive(false);
    }


}
