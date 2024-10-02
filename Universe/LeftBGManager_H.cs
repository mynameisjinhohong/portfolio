using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeftBGManager_H : MonoBehaviour
{
    public GameObject messageBoxBG;
    public GameObject friendUserBG;
    public GameObject roomInfoBG;
    public Image menuImage;
    public float enableTime = 2f;
    float currentTime = 0;
    public bool startTimer = false;

    public MouseOnChangeImage_H[] buttons;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x < 80)
        {
            startTimer = false;
        }
        else
        {
            startTimer = true;
        }


        if (roomInfoBG != null)
        {
            if (roomInfoBG.activeInHierarchy || messageBoxBG.activeInHierarchy || friendUserBG.activeInHierarchy)
            {
                startTimer = false;
            }
        }
        else if (messageBoxBG.activeInHierarchy || friendUserBG.activeInHierarchy)
        {
            startTimer = false;
        }
        else
        {
            startTimer = true;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].ImOn == true)
            {
                startTimer = false;
            }
        }
        if (startTimer)
        {
            currentTime += Time.deltaTime;
            if (enableTime < currentTime)
            {
                Color color = menuImage.color;
                color.a = 255;
                menuImage.color = color;
                gameObject.SetActive(false);
                currentTime = 0;
                startTimer = false;
            }
        }
        else
        {
            currentTime = 0;
        }


    }
    private void OnEnable()
    {
        startTimer = true;
        Color color = menuImage.color;
        color.a = 0;
        menuImage.color = color;
    }

}
