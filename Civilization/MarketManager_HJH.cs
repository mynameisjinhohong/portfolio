using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager_HJH : MonoBehaviour
{
    int nowTurn, currentTurn = 0;
    public int foodMoney = 5;
    public int foodPlus = 5;
    public int upgradeSeaMoney = 10; //물이동을 하기 위해서는 돈이 10원이 필요하다.

    public GameObject ui;
    // Start is called before the first frame update
    void Start()
    {
        currentTurn = TurnManager_lyd.instance.turn;
    }

    // Update is called once per frame
    void Update()
    {
        nowTurn = TurnManager_lyd.instance.turn;
        GameObject ui1 = ui.transform.parent.gameObject;
        if (ui1.activeInHierarchy == false)
        {
            ui.SetActive(false);
        }

    }
    public void BuyFood()
    {
        if(PlayerStatue_HJH.instance.money >= foodMoney)
        {
            nowTurn = TurnManager_lyd.instance.turn;
            if (nowTurn != currentTurn)
            {
                PlayerStatue_HJH.instance.money -= foodMoney;
                GameObject[] market = GameObject.FindGameObjectsWithTag("Market");
                Building_HJH palce;
                for (int i = 0; i < market.Length; ++i)
                {
                    if (market[i].GetComponent<Building_HJH>().state == Building_HJH.State.selected)
                    {
                        palce = market[i].GetComponent<Building_HJH>().GetCell().GetPalace();
                        palce.food += foodPlus;
                    }
                }
            }
        }
        
    }

    //유닛업그레이드 함수를 만든다.
    //무엇을 하냐면, 
    //유닛강화ui를 클릭했을 때 물 이동 ui가 나와야한다. 
    //물이동 ui를 눌렀을 때 물유닛이 생성된다.
    //물유닛은 물타일 위를 건널 수 있다. 
    public void KindUnitUpgradeUI()
    {
        ui.SetActive(true);

    }

    //물이동 ui를 눌렀을 때 물유닛이 생성된다.
    //물유닛은 물타일 위를 건널 수 있다.
    public void SeaMove()
    {
        nowTurn = TurnManager_lyd.instance.turn;
        if(PlayerStatue_HJH.instance.money >= upgradeSeaMoney)
        {
            if (nowTurn != currentTurn)
            {
                PlayerStatue_HJH.instance.money -= upgradeSeaMoney;
                //마켓을 찾는다. 
                GameObject[] market = GameObject.FindGameObjectsWithTag("Market");
                for(int i = 0; i < market.Length; i++)
                {
                    //마켓건물은 빌딩스크립트를 가지고 있으니 빌딩스크립트를 가져온다.
                    Building_HJH bui = market[i].GetComponent<Building_HJH>();
                    HexCell cell = bui.GetCell();
                    //만약 빌딩이 selected 상태이면, cell의 getunit에 유닛이 있으면, currentUnit의 스크립트를 가져와서 bool을 ture로 해준다.
                    if(bui.state == Building_HJH.State.selected)
                    {
                        if(cell.getUnit() != null)
                        {
                            
                            CurrentUnit_HJH unit = cell.getUnit();
                            unit.upgradeWater = true;
                        }
                    }
                }
            }

        }
           
    }
}