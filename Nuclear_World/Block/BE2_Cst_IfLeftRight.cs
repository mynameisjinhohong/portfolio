// --- most used BE2 namespaces for instruction scripts 
using CodingSystem_HJH;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Block.Instruction;
using System.Diagnostics;
using UnityEngine;
// --- additional BE2 namespaces used for specific cases as accessing BE2 variables or the event manager
// using MG_BlocksEngine2.Core;
// using MG_BlocksEngine2.Environment;

public class BE2_Cst_IfLeftRight : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    string value;
    bool firstPlay = true;
    bool canMove = false;
    protected override void OnButtonStop()
    {
        firstPlay = true;
    }

    public override void OnStackActive()
    {
        firstPlay = true;
    }

    public new void Function()
    {
        if (firstPlay)
        {
            _input0 = Section0Inputs[0];
            value = _input0.StringValue;

            switch (value)
            {
                case "Left":
                    canMove = CheckDirection(true);
                    break;
                case "Right":
                    canMove = CheckDirection(false);
                    break;
            }
            if (canMove)
            {
                firstPlay = false;
                ExecuteSection(0);
            }
            else
            {
                firstPlay = true;
                ExecuteNextInstruction();
            }
        }
        else
        {
            firstPlay = true;
            ExecuteNextInstruction();
        }
    }

    public bool CheckDirection(bool left)
    {
        bool dap = false;
        Players player = TargetObject.Transform.gameObject.GetComponent<Players>();
        if (left)
        {
            switch (player.direction)
            {
                case Players.Direction.Left:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y -1, player);
                    break;
                case Players.Direction.Right:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y + 1, player);
                    break;
                case Players.Direction.Forward:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x -1 , player.PlayerPos.y, player);
                    break;
                case Players.Direction.Back:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x + 1, player.PlayerPos.y, player);
                    break;
            }
        }
        else
        {
            switch (player.direction)
            {
                case Players.Direction.Left:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y + 1, player);
                    break;
                case Players.Direction.Right:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y - 1, player);
                    break;
                case Players.Direction.Forward:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x + 1, player.PlayerPos.y, player);
                    break;
                case Players.Direction.Back:
                    dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x - 1, player.PlayerPos.y, player);
                    break;
            }
        }
        return dap;
    }

    // v2.12 - added Reset method to the instructions to enable reuse by Function Blocks
    public new void Reset()
    {
        firstPlay = true;
    }
}
