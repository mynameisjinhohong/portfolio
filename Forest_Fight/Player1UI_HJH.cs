using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Player1UI_HJH : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GameObject player;
    public Text hpText;
    public Text NickName;
    public GameObject life;
    public int LifeCount = 3;
    [SerializeField]
    List<GameObject> lifes;
    bool lifeSet = true;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if(lifeSet == true)
            {
                LifeSet();
            }
            photonView.RPC("GoHp",RpcTarget.All);
        }
    }
    [PunRPC]
    void GoHp()
    {
            hpText.text = "<size=50><b>" + player.GetComponent<PlayerHp_HJH>().Hp + "</b></size>.0%";
            if (player.GetComponent<Respawn_LHS>())
            {
                while (lifes.Count > player.GetComponent<Respawn_LHS>().RespawnCount)
                {
                    Destroy(lifes[lifes.Count - 1]);
                    lifes.RemoveAt(lifes.Count - 1);
                }
            }
        
    }

    
    void LifeSet()
    {
        lifes.Add(life);
        for (int i = 0; i < LifeCount - 1; i++)
        {
            lifes.Add(Instantiate(life, transform.GetChild(2)));
        }
        lifeSet = false;
    }
}
