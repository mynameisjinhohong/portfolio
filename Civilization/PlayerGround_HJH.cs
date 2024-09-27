using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround_HJH : MonoBehaviour
{
    
    public int foodTile;
    public int moneyTile;
    public int cultureTile;
    bool peopleIn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(peopleIn == false)
        {
            nowTurn = TurnManager_lyd.instance.turn;
            currentTurn = nowTurn;
        }
        
    }
    int nowTurn =0,currentTurn = 0;
    private void OnTriggerStay(Collider other)
    {
        peopleIn = true;
        if (other.name.Contains("People"))
        {
            
            Building_HJH build = other.GetComponentInParent<Building_HJH>();
            nowTurn = TurnManager_lyd.instance.turn;
            if(nowTurn != currentTurn)
            {
                build.food += foodTile;
                build.culture += cultureTile;
                PlayerStatue_HJH.instance.money += moneyTile;
                currentTurn = nowTurn;
            }
            
        }
    }
}