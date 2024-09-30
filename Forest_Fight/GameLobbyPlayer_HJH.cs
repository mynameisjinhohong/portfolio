using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameLobbyPlayer_HJH : MonoBehaviourPun
{
    public Text nickText;
    MouseOnCharacterSelect_HJH mouse;

    // Start is called before the first frame update
    void Start()

    {
        // ���� �κ� �Ŵ������� �÷��̾� ������ �� �� �ְ�
        Invoke("LateStart", 0.5f);

    }
    void LateStart()
    {
        GameLobbyManager_HJH.instance.AddPlayer(photonView);
        nickText.text = photonView.Owner.NickName;
        mouse = GameObject.Find("PlayerChoice(Clone)").GetComponent<MouseOnCharacterSelect_HJH>();
        mouse.ui.Add(this.gameObject);
        // �����Ҷ����� ���ӸŴ������� ���� -> �÷��̾� ������ ���� 
        if (photonView.IsMine)
        {
            mouse.whoConnectThis = photonView.ViewID;
        }
    }
    void Update()
    {
        
    }
}
