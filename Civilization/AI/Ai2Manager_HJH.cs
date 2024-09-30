using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai2Manager_HJH : MonoBehaviour
{
    public int enemyMoney;
    public GameObject enemyGround; //1
    public GameObject enemyPalace; //2
    public GameObject enemyArmy; //2
    public GameObject enemyMarket; //2
    public GameObject enemySuwon; //2
    public GameObject enemyArmyUnit;
    public GameObject enemyArcherUnit;
    public GameObject enemyScoutUnit;

    // Start is called before the first frame update
    void Start()
    {
        if (StageMenu_lyd.instance.whatCountry == BTNType.Korea || StageMenu_lyd.instance.whatCountry == BTNType.China)
        {
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(60, 62);
                int y = Random.Range(58, 65);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x-1, y));
            }
            for (int i = 0; i < 2; i++)
            {
                int x = Random.Range(58, 63);
                int y = Random.Range(32, 37);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(45, 51);
                int y = Random.Range(20, 23);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyMarket(x-1 , y));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(53, 57);
                int y = Random.Range(23, 26);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x - 1 , y));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(44, 50);
                int y = Random.Range(20, 22);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x -1, y));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(39, 41);
                int y = Random.Range(4, 9);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x -1, y ));
            }
        }
        else if(StageMenu_lyd.instance.whatCountry == BTNType.Japan)
        {
            for (int i = 0; i < 2; i++)
            {
                int x = Random.Range(24, 28);
                int y = Random.Range(19, 25);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x-1, y));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(25, 27);
                int y = Random.Range(14, 19);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x-1, y));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(19, 27);
                int y = Random.Range(26, 33);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x - 1, y));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(24, 28);
                int y = Random.Range(38, 45);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x-1, y));
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Random.Range(11, 21);
                int y = Random.Range(33, 39);
                StartCoroutine(MakeEnemyPalace(x, y, 6));
                StartCoroutine(MakeEnemySuwon(x + 1, y));
                StartCoroutine(MakeEnemyArmy(x, y + 1));
                StartCoroutine(MakeEnemyMarket(x - 1, y ));
            }
        }

    }

    IEnumerator MakeEnemyPalace(int x, int y, int tileAmount)//�ް��ϸ� 6
    {
        yield return null;
        if (HexGrid.instance.cells[x + (y * 68)].gameObject.name.Contains("Ground") == true && HexGrid.instance.cells[x + (y * 68)].EnemyGround == false)
        {
            HexGrid.instance.cells[x + (y * 68)].EnemyPalace = true;
            HexGrid.instance.cells[x + (y * 68)].EnemyGround = true;
            GameObject eg = Instantiate(enemyGround, HexGrid.instance.cells[x + (y * 68)].gameObject.transform.position - new Vector3(0, 10, 0), Quaternion.identity);
            eg.transform.parent = HexGrid.instance.cells[x + (y * 68)].transform;
            GameObject ep = Instantiate(enemyPalace, HexGrid.instance.cells[x + (y * 68)].gameObject.transform.position - new Vector3(0, 10, 0), Quaternion.identity);
            ep.transform.parent = HexGrid.instance.cells[x + (y * 68)].transform;
            EnemyBuilding_HJH eb = ep.GetComponent<EnemyBuilding_HJH>();
            eb.SetCell(HexGrid.instance.cells[x + (y * 68)]);
            if (tileAmount < 7)
            {
                for (int i = 0; i < tileAmount; i++)
                {
                    GameObject eg1 = Instantiate(enemyGround, HexGrid.instance.cells[x + (y * 68)].GetNeighbor((HexDirection)i).transform.position, Quaternion.identity);
                    eg1.transform.parent = HexGrid.instance.cells[x + (y * 68)].GetNeighbor((HexDirection)i).transform;
                    HexGrid.instance.cells[x + (y * 68)].GetNeighbor((HexDirection)i).EnemyGround = true;
                }
            }
        }

    }
    IEnumerator MakeEnemyArmy(int x, int y)
    {
        yield return null;
        if (HexGrid.instance.cells[x + (y * 68)].gameObject.name.Contains("Ground") && HexGrid.instance.cells[x + ((y - 1) * 68)].gameObject.name.Contains("Ground"))
        {
            HexGrid.instance.cells[x + (y * 68)].EnemyArmy = true;
            GameObject ea = Instantiate(enemyArmy, HexGrid.instance.cells[x + (y * 68)].gameObject.transform.position - new Vector3(0, 10, 0), Quaternion.identity);
            ea.transform.parent = HexGrid.instance.cells[x + (y * 68)].transform;
            EnemyBuilding_HJH eb = ea.GetComponent<EnemyBuilding_HJH>();
            eb.SetCell(HexGrid.instance.cells[x + (y * 68)]);
        }
    }

    IEnumerator MakeEnemySuwon(int x, int y)
    {
        yield return null;
        if (HexGrid.instance.cells[x + (y * 68)].gameObject.name.Contains("Ground") && HexGrid.instance.cells[x - 1 + (y * 68)].gameObject.name.Contains("Ground"))
        {
            HexGrid.instance.cells[x + (y * 68)].EnemySuwon = true;
            GameObject ea = Instantiate(enemySuwon, HexGrid.instance.cells[x + (y * 68)].gameObject.transform.position - new Vector3(0, 10, 0), Quaternion.identity);
            ea.transform.parent = HexGrid.instance.cells[x + (y * 68)].transform;
            EnemyBuilding_HJH eb = ea.GetComponent<EnemyBuilding_HJH>();
            eb.SetCell(HexGrid.instance.cells[x + (y * 68)]);
        }
    }
    IEnumerator MakeEnemyMarket(int x, int y)
    {
        yield return null;
        if (HexGrid.instance.cells[x + (y * 68)].gameObject.name.Contains("Ground") && HexGrid.instance.cells[x + 1 + (y * 68)].gameObject.name.Contains("Ground"))
        {
            HexGrid.instance.cells[x + (y * 68)].EnemyMarket = true;
            GameObject ea = Instantiate(enemyMarket, HexGrid.instance.cells[x + (y * 68)].gameObject.transform.position - new Vector3(0, 10, 0), Quaternion.identity);
            ea.transform.parent = HexGrid.instance.cells[x + (y * 68)].transform;
            EnemyBuilding_HJH eb = ea.GetComponent<EnemyBuilding_HJH>();
            eb.SetCell(HexGrid.instance.cells[x + (y * 68)]);
        }
    }
    int nowTurn = 0, currentTurn = 0;
    bool create = true;
    // Update is called once per frame
    void Update()
    {
        currentTurn = TurnManager_lyd.instance.turn;

        //�׽�Ʈ�� 1������ �����
        //if (currentTurn > nowTurn && create == true)
        //{
        //    GameObject[] ea = GameObject.FindGameObjectsWithTag("EnemyArmy");
        //    Instantiate(enemyArcherUnit, ea[0].transform.position - new Vector3(0, 10, 0), Quaternion.identity);
        //    ea[0].GetComponent<EnemyBuilding_HJH>().GetCell().EnemyUnit = true;
        //    create = false;
        //}

        if (currentTurn > nowTurn)
        {
            enemyMoney += 10;
            int count = 0;
            while (enemyMoney > 1 && count < 10)
            {

                GameObject[] ea = GameObject.FindGameObjectsWithTag("EnemyArmy2");
                int whereSummon = Random.Range(0, ea.Length);
                int whatSummon = Random.Range(0, 3);
                if (enemyMoney >= 3 && whatSummon == 0 && ea[whereSummon].GetComponent<EnemyBuilding_HJH>().GetCell().EnemyUnit == false)
                {
                    GameObject a = Instantiate(enemyArcherUnit, ea[whereSummon].transform.position - new Vector3(0, 10, 0), Quaternion.identity);
                    ea[whatSummon].GetComponent<EnemyBuilding_HJH>().GetCell().EnemyUnit = a.GetComponent<EnemyUnit_HJH>();
                    enemyMoney -= 3;
                }
                else if (enemyMoney >= 2 && whatSummon == 1 && ea[whereSummon].GetComponent<EnemyBuilding_HJH>().GetCell().EnemyUnit == false)
                {
                    GameObject a = Instantiate(enemyArmyUnit, ea[whereSummon].transform.position - new Vector3(0, 10, 0), Quaternion.identity);
                    ea[whatSummon].GetComponent<EnemyBuilding_HJH>().GetCell().EnemyUnit = a.GetComponent<EnemyUnit_HJH>();
                    enemyMoney -= 2;
                }
                else if (enemyMoney >= 1 && whatSummon == 3 && ea[whereSummon].GetComponent<EnemyBuilding_HJH>().GetCell().EnemyUnit == false)
                {
                    GameObject a = Instantiate(enemyScoutUnit, ea[whereSummon].transform.position - new Vector3(0, 10, 0), Quaternion.identity);
                    ea[whatSummon].GetComponent<EnemyBuilding_HJH>().GetCell().EnemyUnit = a.GetComponent<EnemyUnit_HJH>();
                    enemyMoney -= 1;
                }
                count++;
            }

            nowTurn = currentTurn;
        }

    }
}