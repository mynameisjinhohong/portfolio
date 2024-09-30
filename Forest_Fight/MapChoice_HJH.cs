using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MapChoice_HJH : MonoBehaviourPun
{
    ColorBlock nomal;
    // Start is called before the first frame update
    void Start()
    {
        nomal = buttons[1].colors;
        // 방장이 아니라면 맵 선택 금지!

    }

    public Button [] buttons;

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && GameLobbyManager_HJH.instance.playerPhoton.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            //GameObject MapChoice = GameObject.Find("MapChoice(Clone)");
            //Button[] buttonS = MapChoice.GetComponentsInChildren<Button>();

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
            }
        }
    }

    // 방장만 보낼 수 있게!
    public void map1()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("map11", RpcTarget.All);
            photonView.RPC("mapSet", RpcTarget.All, 0);
        }
    }

    public void map2()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("map22", RpcTarget.All);
            photonView.RPC("mapSet", RpcTarget.All, 1);
        }
    }
    [PunRPC]
    void map11()
    {
        GameManager.instance.mapName = "MainScene_Photon";
    }
    [PunRPC]
    void map22()
    {
        GameManager.instance.mapName = "MainScene2_Photon";
    }

    [PunRPC]
    void mapSet(int index)
    {

        ColorBlock colorBlock = new ColorBlock();
        colorBlock.normalColor = new Color(0, 0, 0, 0);
        buttons[0].colors = nomal;
        buttons[1].colors = nomal;
        buttons[index].colors = colorBlock ;
        
    }
}
