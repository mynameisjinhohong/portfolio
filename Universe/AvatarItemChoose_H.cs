using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarItemChoose_H : MonoBehaviour
{
    public int whatItem;
    public int itemIdx;

    public void itemClick()
    {
        if(whatItem == 0)
        {
            GameManager.instance.userinfo.avatarSet.head = itemIdx;
        }
        else if(whatItem == 1)
        {
            GameManager.instance.userinfo.avatarSet.face = itemIdx;
        }
        else if (whatItem == 2)
        {
            GameManager.instance.userinfo.avatarSet.body = itemIdx;
        }
    }
}
