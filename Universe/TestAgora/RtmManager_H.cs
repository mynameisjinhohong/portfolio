using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_rtm;

public class RtmManager_H : MonoBehaviour
{
    public const string ADD_CHANNEL_COMMAND = "ADD-";
    public const string DELETE_CHANNEL_COMMAND = "DEL-";

    RtmClient rtmClient = null;
    RtmChannel rtmChannel;
    RtmClientEventHandler clientEventHandler;
    RtmChannelEventHandler channelEventHandler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
