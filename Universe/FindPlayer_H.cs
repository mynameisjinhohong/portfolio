using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer_H : MonoBehaviour
{
    CinemachineVirtualCamera cinemachine;
    // Start is called before the first frame update
    void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();

    }

    // Update is called once per frame
    void Update()
    {
        if(cinemachine.Follow == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for(int i =0; i<players.Length; i++)
            {
                if (players[i].GetComponent<PhotonView>().IsMine == true)
                {
                    cinemachine.Follow = players[i].transform;
                }
            }
        }
        
    }
}
