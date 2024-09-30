using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainSceneManager_HJH : MonoBehaviourPun
{
    int playerNum;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            playerNum = photonView.ViewID / 1000;
            GameObject[] startPoint = GameObject.FindGameObjectsWithTag("StartPoint");
            GameObject player = PhotonNetwork.Instantiate(GameManager.instance.playerCharcters[playerNum].ToString(), startPoint[playerNum - 1].transform.position, Quaternion.Euler(0, 90, 0));
            GameObject ui1 = PhotonNetwork.Instantiate("CharacterUI", Vector3.zero, Quaternion.identity);
            int id = ui1.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetInfo", RpcTarget.All, GameManager.instance.startLife, player.name, photonView.Owner.NickName, id);
        }
        

    }

    [PunRPC]
    void SetInfo(int life,string playerName, string nickName,int uiViewId)
    {
        GameObject player = GameObject.Find(playerName);
        player.GetComponent<Respawn_LHS>().RespawnCount = life;
        Player1UI_HJH[] uis = FindObjectsOfType<Player1UI_HJH>();
        for(int i = 0; i < uis.Length; i++)
        {
            if(uis[i].gameObject.GetComponent<PhotonView>().ViewID == uiViewId)
            {
                uis[i].player = player;
                uis[i].LifeCount = life;
                uis[i].NickName.text = photonView.Owner.NickName;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
