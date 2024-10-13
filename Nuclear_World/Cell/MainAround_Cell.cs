using CodingSystem_HJH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAround_Cell : Cell
{
    public Main_Cell mainCell;
    public override bool CanMove(int x, int y, Players player)
    {
        if (onPlayer == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void OnPlayer(Players player, int beforeX, int beforeY)
    {
        onPlayer = player;
        if (mainCell.isGoal == false)
        {
            player.canMove = false;
            mainCell.isGoal = true;
        }
    }


}

