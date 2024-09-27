using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuwonManager_HJH : MonoBehaviour
{
    public Text makingText;
    GameObject createSuwonButton;
    public int sukGulAmMoney = 10;
    public int sukGulAmTurn = 5;
    int startTurn;
    bool sukGulAmMaking = false,sukGulAmMaked = false;
    int nowTurn, currentTurn = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name.Contains("Suwon"))
        {
            GameObject UI = GameObject.Find("Canvas_Pioneer");
            createSuwonButton = UI.transform.GetChild(6).gameObject;
        }
    }
    public void CloseUI()
    {
        createSuwonButton.SetActive(false);
        GameObject[] ui = GameObject.FindGameObjectsWithTag("Suwon");
        for(int i=0; i<ui.Length; i++)
        {
            ui[i].GetComponent<Suwon_HJH>().state = Suwon_HJH.State.Idle;
        }
    }
    public void CreateSukGulAm()
    {
        if(sukGulAmMaked == false && PlayerStatue_HJH.instance.money >= sukGulAmMoney)
        {
            startTurn = sukGulAmTurn;
            sukGulAmMaking = true;
            PlayerStatue_HJH.instance.money -= sukGulAmMoney;
            CloseUI();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        nowTurn = TurnManager_lyd.instance.turn;
        if(nowTurn != currentTurn && sukGulAmMaking == true)
        {
            startTurn--;
            currentTurn = nowTurn;
            MakingCheck(ref sukGulAmMaking, startTurn,ref sukGulAmMaked);
        }
        if(sukGulAmMaking == true)
        {
            makingText.text = "석굴암" + "을 제작중입니다\n" +"남은 턴 : " +startTurn;
        }
        else if(sukGulAmMaking == false && sukGulAmMaked == true)
        {
            Debug.Log("석굴암 제작완료");
        }

    }
    void MakingCheck(ref bool what, int howMuch, ref bool done)
    {
        if(howMuch == 0)
        {
            what = false;
            done = true;
            makingText.text = "";
        }
    }
}