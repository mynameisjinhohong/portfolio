using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;

public class UpperUI_H : MonoBehaviourPun
{
    public Button chatButton;
    public Button micButton;
    public Button mediaButton;
    public Button screenShareButton;
    public Button faceChatButton;

    public Sprite[] blackImage;
    public Sprite[] purpleImage;

    public GameObject chatCanvas;

    public GameObject myCharacter;
    bool setPurple = false;
    private void Start()
    {
        myCharacter = GameManager.instance.myCharacter;
    }
    private void Update()
    {
        if (!GameManager.instance.faceChatOn)
        {
            screenShareButton.image.sprite = blackImage[3];
            faceChatButton.image.sprite = blackImage[4];
            setPurple = false;
        }
        else if(GameManager.instance.faceChatOn && !setPurple)
        {
            setPurple = true;
            faceChatButton.image.sprite = purpleImage[4];
        }
    }

    public void ChatButton()
    {
        if(chatButton.image.sprite == blackImage[0])
        {
            chatButton.image.sprite = purpleImage[0];
            chatCanvas.SetActive(true);
        }
        else
        {
            chatCanvas.SetActive(false);
            chatButton.image.sprite = blackImage[0];
        }
    }
    public void MicButton()
    {
        if (micButton.image.sprite == blackImage[1])
        {
            myCharacter.AddComponent<PhotonVoiceView>();
            micButton.image.sprite = purpleImage[1];
        }
        else
        {
            Destroy(myCharacter.GetComponent<PhotonVoiceView>());
            micButton.image.sprite = blackImage[1];
        }
    }
    public void MediaButton()
    {
        if (mediaButton.image.sprite == blackImage[2])
        {
            mediaButton.image.sprite = purpleImage[2];
        }
        else
        {
            mediaButton.image.sprite = blackImage[2];
        }
    }
    public void ScreenButton()
    {
        if (screenShareButton.image.sprite == blackImage[3] && GameManager.instance.faceChatOn == true)
        {
            if(faceChatButton.image.sprite == purpleImage[4])
            {
                screenShareButton.image.sprite = purpleImage[3];
                faceChatButton.image.sprite = blackImage[4];
                GameManager.instance.ShareScreen = true;
            }
        }
    }
    public void FaceButton()
    {
        if (faceChatButton.image.sprite == blackImage[4] && GameManager.instance.faceChatOn == true)
        {
            if (screenShareButton.image.sprite == purpleImage[3])
            {
                faceChatButton.image.sprite = purpleImage[4];
                screenShareButton.image.sprite = blackImage[3];
                GameManager.instance.ShareScreen = true;
            }
        }
    }
}
