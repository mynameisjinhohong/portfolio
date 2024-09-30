using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest_HJH : MonoBehaviour
{
    public GameObject enemy;
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("MakeEnemy", 0.1f);
    }

    void MakeEnemy()
    {
        GameObject en = Instantiate(enemy, HexGrid.instance.cells[x + (68 * y)].transform.position - new Vector3(0,20,0) , Quaternion.identity);
        HexGrid.instance.cells[x + (68 * y)].EnemyUnit = en.GetComponent<EnemyUnit_HJH>();
    }
    // Update is called once per frame
    void Update()
    {
    }
}