using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army_HJH : MonoBehaviour
{
    int nowTurn, currentTurn = 0;
    public enum State
    {
        Idle,
        Selected,
    }
    public State state;
    GameObject createArmyButton;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name.Contains("Army"))
        {
                    GameObject UI = GameObject.Find("Canvas_Pioneer");
            createArmyButton = UI.transform.GetChild(4).gameObject;
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
            if (gameObject.name.Contains("Army"))
            {
                GameObject[] army = GameObject.FindGameObjectsWithTag("Army");
                int count = 0;
                for (int i = 0; i < army.Length; i++)
                {
                    Army_HJH arm = army[i].GetComponent<Army_HJH>();
                    if (arm.state == Army_HJH.State.Idle)
                    {
                        ++count;
                    }
                }
                if (count == army.Length)
                {
                    createArmyButton.SetActive(false);
                }
            }
        }
    }
    private void SelectedFunction()
    {
        if (gameObject.name.Contains("Army"))
        {
            createArmyButton.SetActive(true);
        }
    }
}