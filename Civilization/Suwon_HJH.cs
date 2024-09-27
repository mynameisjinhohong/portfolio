using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suwon_HJH : MonoBehaviour
{
    int nowTurn, currentTurn = 0;
    public enum State
    {
        Idle,
        Selected,
    }
    public State state;
    GameObject createMarketButton;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name.Contains("Suwon"))
        {
            GameObject UI = GameObject.Find("Canvas_Pioneer");
            createMarketButton = UI.transform.GetChild(6).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        nowTurn = TurnManager_lyd.instance.turn;
        if (state == State.Selected)
        {
            SelectedFunction();
        }
        else if (state == State.Idle)
        {
            if (gameObject.name.Contains("Suwon"))
            {
                GameObject[] army = GameObject.FindGameObjectsWithTag("Suwon");
                int count = 0;
                for (int i = 0; i < army.Length; i++)
                {
                    Suwon_HJH arm = army[i].GetComponent<Suwon_HJH>();
                    if (arm.state == Suwon_HJH.State.Idle)
                    {
                        ++count;
                    }
                }
                if (count == army.Length)
                {
                    createMarketButton.SetActive(false);
                }
            }
        }
    }
    private void SelectedFunction()
    {
        if (gameObject.name.Contains("Suwon"))
        {
            createMarketButton.SetActive(true);
            createMarketButton.transform.SetAsLastSibling();
            //CurrentUnit¿¡¼­ selectedFunctionÀ» ²¨ÁØ´Ù. 
        }
    }
}