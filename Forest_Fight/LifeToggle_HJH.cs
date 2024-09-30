using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class LifeToggle_HJH : MonoBehaviourPun
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            for(int i =0; i < toggles.Length; i++)
            {
                toggles[i].interactable = false;
            }
        }
    }
    public Toggle[] toggles;
  public void Life2()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LifeSet", RpcTarget.Others, 2);
            photonView.RPC("ToggleSet", RpcTarget.Others, 0);

        }
        GameManager.instance.startLife = 2;
    }
    public void Life3()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LifeSet", RpcTarget.Others, 3);
            photonView.RPC("ToggleSet", RpcTarget.Others, 1);
        }
        GameManager.instance.startLife = 3;
    }
    public void Life4()
    {if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LifeSet", RpcTarget.Others, 4);
            photonView.RPC("ToggleSet", RpcTarget.Others, 2);
        }
        GameManager.instance.startLife = 4;
    }
    public void Life5()
    {if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LifeSet", RpcTarget.Others, 5);
            photonView.RPC("ToggleSet", RpcTarget.Others, 3);
        }
        GameManager.instance.startLife = 5;
    }

    [PunRPC]
    void LifeSet(int life)
    {
        GameManager.instance.startLife = life;
    }
    [PunRPC]
    void ToggleSet(int index)
    {
        toggles[index].isOn = true;
    }
}
