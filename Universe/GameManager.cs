using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public Userinfo userinfo;
    public MapData mapData;
    public GameObject myCharacter;
    public string url = "43.201.8.119:8080";
    public bool faceChatOn = false;
    public bool micOn = false;
    public bool cherryDone = false;
    public bool capDone = false;
    public bool ShareScreen = false;
    public string whatLayerLoad = "";
    int startCherry;
    int startCap;
    bool userInfoSet = false;
    AudioSource audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        userinfo.URLName = new string[5];
        userinfo.URLs = new string[5];
        SetResolution();
    }

    private void Update()
    {   
        
        if (SceneManager.GetActiveScene().name == "LoginScene_L" || SceneManager.GetActiveScene().name == "LobbyScene_L" || SceneManager.GetActiveScene().name == "MyInfoScene_L")
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        //if(userinfo != null &&userInfoSet == false)
        //{
        //    userInfoSet = true;
        //    if(PlayerPrefs.GetString("Today") == DateTime.Now.ToString("dd"))
        //    {
        //        startCherry = PlayerPrefs.GetInt("startCherry");
        //        startCap = PlayerPrefs.GetInt("startCap");
        //    }
        //    else
        //    {
        //        startCherry = userinfo.memberCherry;
        //        startCap = userinfo.memberCap;
        //        PlayerPrefs.SetInt("startCherry", startCherry);
        //        PlayerPrefs.SetInt("startCap", startCap);
        //        PlayerPrefs.SetString("Today", DateTime.Now.ToString("dd"));
        //    }

        //}
        //if(userinfo.memberCherry - startCherry >4)
        //{
        //    cherryDone = true;
        //}
        //if (userinfo.memberCap - startCap > 4)
        //{
        //    capDone = true;
        //}
        
    }

    public void SetResolution()
    {
        int setWidth = 1920;
        int setHeight = 1080;
        Screen.SetResolution(setWidth, setHeight, true);
    }
}
