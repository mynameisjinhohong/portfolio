using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market_HJH : MonoBehaviour
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
        if (gameObject.name.Contains("Market"))
        {
            GameObject UI = GameObject.Find("Canvas_Pioneer");
            createMarketButton = UI.transform.GetChild(5).gameObject;
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
            if (gameObject.name.Contains("Market"))
            {
                GameObject[] army = GameObject.FindGameObjectsWithTag("Market");
                int count = 0;
                for (int i = 0; i < army.Length; i++)
                {
                    Market_HJH arm = army[i].GetComponent<Market_HJH>();
                    if (arm.state == Market_HJH.State.Idle)
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
        if (gameObject.name.Contains("Market"))
        {
            createMarketButton.SetActive(true);
            //CurrentUnit¿¡¼­ selectedFunctionÀ» ²¨ÁØ´Ù. 
        }
    }
}