using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MouseOnCharacterSelect_HJH : MonoBehaviourPun
{
    // 누가 바꾸고 있는가를 알려주는 변수
    public int whoConnectThis;

    bool select = false;
    public List<GameObject> ui = new List<GameObject>();
    public Button[] buttons;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AlondSelect()
    {
        if(select == false)
        {
            photonView.RPC("ChangeImage", RpcTarget.All, 0, whoConnectThis);
            select = true;
            photonView.RPC("SelectedButton", RpcTarget.All, 0);
            photonView.RPC("SetDic",RpcTarget.All, whoConnectThis,(int)GameManager.PlayerCharcter.Aland);
        }
        
    }
    public void AliceSelect()
    {
        if(select == false)
        {

            photonView.RPC("SetDic",RpcTarget.All , whoConnectThis, (int)GameManager.PlayerCharcter.Alice);
            photonView.RPC("ChangeImage", RpcTarget.All, 1, whoConnectThis);
            photonView.RPC("SelectedButton", RpcTarget.All, 1);
            select = true;
        }

    }
    public void WarriorSelect()
    {
        if(select == false)
        {

            photonView.RPC("SetDic", RpcTarget.All, whoConnectThis, (int)GameManager.PlayerCharcter.Warrior);
            photonView.RPC("ChangeImage", RpcTarget.All, 2, whoConnectThis);
            photonView.RPC("SelectedButton", RpcTarget.All, 2);
            select = true;
        }

    }
    public void ArcherSelect()
    {
        if (select == false)
        {

            photonView.RPC("SetDic", RpcTarget.All, whoConnectThis, (int)GameManager.PlayerCharcter.Archer);
            photonView.RPC("ChangeImage", RpcTarget.All, 3, whoConnectThis);
            photonView.RPC("SelectedButton", RpcTarget.All, 3);
            select = true;
        }
        
    }
    public void AlandOn()
    {
        if (select == false && buttons[0].interactable)
        {
            photonView.RPC("ChangeImage", RpcTarget.All, 0, whoConnectThis);
        }
        
    }
    public void AliceOn()
    {

        if (select == false && buttons[1].interactable)
        {

            photonView.RPC("ChangeImage", RpcTarget.All, 1, whoConnectThis);

        }
    }
    public void WarriorOn()
    {
        if (select == false && buttons[2].interactable)
        {
            photonView.RPC("ChangeImage", RpcTarget.All, 2, whoConnectThis);
        }
    }
    public void ArcherOn()
    {
        if (select == false && buttons[3].interactable)
        {
            photonView.RPC("ChangeImage", RpcTarget.All, 3,whoConnectThis);
            //ChangeImage(3);
        }
    }

    // 알아서 생성되게 -> UI안에 들어가게
    [PunRPC]
    void ChangeImage(int what,int who)
    {
        int idx = 0;
        for(int i = 0; i < ui.Count; i++)
        {
            if(ui[i].GetComponent<PhotonView>().ViewID == who)

            {
                idx = i;
                break;
            }
        }


        for (int i = 0; i < 5; i++)
        {
            ui[idx].transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        ui[idx].transform.GetChild(0).GetChild(what).gameObject.SetActive(true);

        
    }

    [PunRPC]
    void SelectedButton(int what)
    {
        buttons[what].interactable = false;
    }
    [PunRPC]
    void SetDic(int player,int charcter)
    {
        if (GameManager.instance.playerCharcters.ContainsKey(player / 1000))
        {
            GameManager.instance.playerCharcters[player / 1000] = (GameManager.PlayerCharcter)charcter;
        }
        else
        {
            GameManager.instance.playerCharcters.Add(player / 1000, (GameManager.PlayerCharcter)charcter);
        }
    }
}
