using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using System;
using Photon.Voice.PUN;
using UnityEngine.UI;

[System.Serializable]
public struct AvatarSet
{
    public int head;
    public int face;
    public int body;
}

public class CharacterMove_H : MonoBehaviourPun, IPunObservable
{
    public string getItemURL = "/members/item";
    public bool capDone;
    public bool cherryDone;
    public AvatarSet avatarSet;
    public bool myCharacter = true;
    public string myToken;
    public float speed;
    public float runSpeed;
    float applyRunSpeed;
    bool applyRunFlag = false;
    private Vector3 vector;
    public int walkCount;
    int currentWalkCount;
    bool canMove = true;
    private Animator animator;
    public Sprite[] imos1;
    public Sprite[] imos2;
    public Sprite[] imos3;
    public Sprite[] imos4;
    //public Sprite[] imoticon;
    public GameObject imoticonPrefab;
    public GameObject pressFKey;
    private KeyCode[] keyCodes = {
KeyCode.Alpha1,
KeyCode.Alpha2,
KeyCode.Alpha3,
KeyCode.Alpha4,
KeyCode.Alpha5,
KeyCode.Alpha6,
KeyCode.Alpha7,
KeyCode.Alpha8,
KeyCode.Alpha9,
};
    public bool inObj = false;
    public ObjectInfo_H objInfo;
    public Userinfo user;
    public int idx;
    public int whatAnimNow = 0;
    public int dir = 2;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer[] anims;
    [SerializeField]
    public object[] values = new object[4];
    public GameObject chatBox;
    int Speed = 100;
    PhotonVoiceView photonVoiceView;

    public Text textNicknameBox;
    string myNickname;
    UICommon_L uiCommon;
    void Start()
    {
        photonVoiceView = GetComponent<PhotonVoiceView>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myNickname = GameManager.instance.userinfo.nickname;
        if (photonView.IsMine)
        {
            user = new Userinfo();

            user.name = PhotonNetwork.NickName;
        }
        animator = GetComponent<Animator>();
        if (photonView.IsMine)
        {
            avatarSet.head = GameManager.instance.userinfo.avatarSet.head;
            avatarSet.face = GameManager.instance.userinfo.avatarSet.face;
            avatarSet.body = GameManager.instance.userinfo.avatarSet.body;
        }
        GameObject spriteBox = GameObject.Find("SpriteBox");
        imos1 = spriteBox.GetComponent<ImoticonSpriteBox_H>().sprite1;
        imos2 = spriteBox.GetComponent<ImoticonSpriteBox_H>().sprite2;
        imos3 = spriteBox.GetComponent<ImoticonSpriteBox_H>().sprite3;
        imos4 = spriteBox.GetComponent<ImoticonSpriteBox_H>().sprite4;
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetNickname", RpcTarget.AllBuffered, myNickname);
            uiCommon = GameObject.FindGameObjectWithTag("UICommon").GetComponent<UICommon_L>();
        }
    }

    public IEnumerator WebRequest_ItemCheck()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(GameManager.instance.url + getItemURL))
        {
            request.SetRequestHeader("Authorization", myToken);
            request.SetRequestHeader("Content-Type", "application/json");
            //Debug.Log(request.GetRequestHeader("Authorization"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string result = request.downloadHandler.text;
                string[] data = result.Split("\"data\":");
                data[1] = data[1].Substring(0, data[1].Length - 1);
                ItemRequest item = JsonUtility.FromJson<ItemRequest>(data[1]);
                avatarSet.head = item.itemHairCode;
                avatarSet.face = item.itemFaceCode;
                avatarSet.body = item.itemClothCode;
            }
            request.Dispose();
            itemSet = true;
        }
    }

    [PunRPC]
    void RPCSetNickname(string nickname)
    {
        textNicknameBox.text = nickname;
    }

    bool chatOn = false;
    public void ChatOn(string chat)
    {
        photonView.RPC("RPCChatOn", RpcTarget.All, chat);
    }
    [PunRPC]
    void RPCChatOn(string chat)
    {
        chatOn = true;
        chatBox.SetActive(true);
        string realChat = "";
        for(int i =0; i<chat.Length; i++)
        {
            realChat += chat[i];
            if(i %12 == 0 && i != 0)
            {
                realChat += "\n";
            }
        }
        chatBox.transform.GetChild(0).GetComponent<Text>().text = realChat;
        Invoke("ChatOff", 3f);
    }

    void ChatOff()
    {
        chatOn = false;
        chatBox.SetActive(false);
        chatBox.transform.GetChild(0).GetComponent<Text>().text = "";
    }

    public bool itemSet = false;
    void Update()
    {
        if ((photonVoiceView.IsSpeaking) && chatOn == false)
        {
            //Debug.Log("isrecording = " + photonVoiceView.IsRecording + "\nisspeaking = " + photonVoiceView.IsSpeaking);
            chatBox.SetActive(true);
            chatBox.transform.GetChild(0).GetComponent<Text>().text = "...";
        }
        else if (chatOn == false)
        {
            chatBox.SetActive(false);
            chatBox.transform.GetChild(0).GetComponent<Text>().text = "";
        }

        //Debug.Log("Base : " + GetComponent<SpriteRenderer>().sprite.name + "\nFace : " + anims[0].sprite.name +"\nHead : " + anims[1].sprite.name + "\nBody : " + anims[2].sprite.name);
        if (myToken.Length > 0 && itemSet == false)
        {
            StartCoroutine(WebRequest_ItemCheck());
        }
        if (photonView.IsMine == false)
        {
            //animator.SetFloat("DirX", vector.x);
            //animator.SetFloat("DirY", vector.y);
            //transform.position = Vector3.Lerp(transform.position, receivePos, speed*Time.deltaTime);
            return;
        }
        if (uiCommon.checkInputFieldOn() == true)
        {
            return;
        }
        cherryDone = GameManager.instance.cherryDone;
        capDone = GameManager.instance.capDone;
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                //StopAllCoroutines();
                StartCoroutine(MoveCoroutine());
            }
        }
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                photonView.RPC("RPCEmoji", RpcTarget.All, i, idx);

/*                GameObject imo = gameObject.transform.GetChild(0).gameObject;
                EmoDestory_H emo = imo.GetComponent<EmoDestory_H>();
                emo.emoOn = true;
                emo.checkTime = 0;
                SpriteRenderer spriteRenderer = imo.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = imoticon[i];
                imo.transform.parent = gameObject.transform;*/
            }

            //키를 눌렀을 때
            //나의 아이디를 찾은 후, <==여기까진 로컬
            //아이디를 통해 해당 키의 이모티콘을 켜주는 것 <==이건 RPC로 해야함

            //  photonView.RPC("RPCEmoji", RpcTarget.All, i);
        }
        if(inObj == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                objInfo.OnPlayerCall();
            }
        }
        if (spriteRenderer.sprite.name.Contains("1"))
        {
            whatAnimNow = 1;
        }
        else if (spriteRenderer.sprite.name.Contains("2"))
        {
            whatAnimNow = 2;
        }
        else if (spriteRenderer.sprite.name.Contains("3"))
        {
            whatAnimNow = 3;
        }
        else if (spriteRenderer.sprite.name.Contains("4"))
        {
            whatAnimNow = 4;
        }
        if (spriteRenderer.sprite.name.Contains("F"))
        {
            dir = 2;
        }
        else if (spriteRenderer.sprite.name.Contains("B"))
        {
            dir = 0;
        }
        else if (spriteRenderer.sprite.name.Contains("L"))
        {
            dir = 3;
        }
        else if (spriteRenderer.sprite.name.Contains("R"))
        {
            dir = 1;
        }
    }



    [PunRPC]
    void RPCEmoji(int num, int myIdx)
    {
//        if (GetComponent<CharacterMove_H>().enabled)
//        {
            GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < playerArray.Length; i++)
            {
                if (playerArray[i].GetComponent<CharacterMove_H>().idx == myIdx)
                {
                    GameObject imo = playerArray[i].transform.GetChild(0).gameObject;
                    EmoDestory_H emo = imo.GetComponent<EmoDestory_H>();
                    emo.emoOn = true;
                    StartCoroutine(ImoticonAnimation(imo, num));
                    emo.checkTime = 0;
                    imo.transform.parent = playerArray[i].transform;
                }
            }
            
//        }
    }
    bool iswalking;
    IEnumerator ImoticonAnimation(GameObject imo,int imoNum)
    {
        for(int i =0; i < 8; i++)
        {
            switch (imoNum)
            {
                case 0:
                    imo.GetComponent<SpriteRenderer>().sprite = imos1[i];
                    break;
                case 1:
                    imo.GetComponent<SpriteRenderer>().sprite = imos2[i];
                    break;
                case 2:
                    imo.GetComponent<SpriteRenderer>().sprite = imos3[i];
                    break;
                case 3:
                    imo.GetComponent<SpriteRenderer>().sprite = imos4[i];
                    break;

            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator MoveCoroutine()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            applyRunSpeed = runSpeed;
            applyRunFlag = true;
        }
        else
        {
            applyRunSpeed = 0;
            applyRunFlag = false;
        }
        vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        int count = 0;
        animator.SetFloat("DirX", vector.x);
        animator.SetFloat("DirY", vector.y);
        photonView.RPC("DirSet", RpcTarget.All, dir);
        animator.SetBool("Walking", true);
        iswalking = true;
        while (currentWalkCount < walkCount)
        {
            count++;
            transform.position += vector.normalized * (speed + applyRunSpeed) * 0.1f;
            

            if (applyRunFlag)
            {
                currentWalkCount++;
            }
            currentWalkCount++;
            yield return new WaitForSeconds(0.01f);
        }
        animator.SetBool("Walking", false);
        iswalking = false;
        currentWalkCount = 0;
        canMove = true;
    }

    [PunRPC]
    void DirSet(int a)
    {
        dir = a;
    }
    Vector3 receivePos;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(whatAnimNow);
            stream.SendNext(capDone);
            stream.SendNext(cherryDone);
            //stream.SendNext(transform.position);
            //stream.SendNext(vector);
            //stream.SendNext(iswalking);
        }
        else if (stream.IsReading)
        {
           

            for(int i = 0; i < 3; i++)
            {
                try
                {
                    values[i] = stream.ReceiveNext();
                    //Debug.Log($"{i} : {values[i]}");
                }
                catch(Exception e)
                {
                    //Debug.Log(e);
                }
            }
            whatAnimNow = (int)values[0];
            capDone = (bool)values[1];
            cherryDone = (bool)values[2];
            //receivePos = (Vector3)values[1];
            //vector = (Vector3)values[2];
            //iswalking = (bool)values[3];
        }
    }
}
