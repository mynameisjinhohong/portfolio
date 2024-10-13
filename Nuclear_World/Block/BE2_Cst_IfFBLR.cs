using System.Collections;

using System.Collections.Generic;

using UnityEngine;



// --- most used BE2 namespaces for instruction scripts 

using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;
using CodingSystem_HJH;



// --- additional BE2 namespaces used for specific cases as accessing BE2 variables or the event manager

// using MG_BlocksEngine2.Core;

// using MG_BlocksEngine2.Environment;



public class BE2_Cst_IfFBLR : BE2_InstructionBase, I_BE2_Instruction
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
                    canMove = CheckDirection(value);
                    break;
                case "Right":
                    canMove = CheckDirection(value);
                    break;
                case "Front":
                    canMove = CheckDirection(value);
                    break;
                case "Back":
                    canMove = CheckDirection(value);
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

    public bool CheckDirection(string value)
    {
        bool dap = false;
        Players player = TargetObject.Transform.gameObject.GetComponent<Players>();
        switch (value)
        {
            case "Left":
                switch (player.direction)
                {
                    case Players.Direction.Left:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y + 1, player);
                        break;
                    case Players.Direction.Right:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y - 1, player);
                        break;
                    case Players.Direction.Forward:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x - 1, player.PlayerPos.y, player);
                        break;
                    case Players.Direction.Back:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x + 1, player.PlayerPos.y, player);
                        break;
                }
                break;
            case "Right":
                switch (player.direction)
                {
                    case Players.Direction.Left:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y - 1, player);
                        break;
                    case Players.Direction.Right:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y + 1, player);
                        break;
                    case Players.Direction.Forward:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x + 1, player.PlayerPos.y, player);
                        break;
                    case Players.Direction.Back:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x - 1, player.PlayerPos.y, player);
                        break;
                }
                break;
            case "Front":
                switch (player.direction)
                {
                    case Players.Direction.Left:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x - 1, player.PlayerPos.y, player);
                        break;
                    case Players.Direction.Right:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x + 1, player.PlayerPos.y, player);
                        break;
                    case Players.Direction.Forward:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y - 1, player);
                        break;
                    case Players.Direction.Back:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y + 1, player);
                        break;
                }
                break;
            case "Back":
                switch (player.direction)
                {
                    case Players.Direction.Left:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x + 1, player.PlayerPos.y, player);
                        break;
                    case Players.Direction.Right:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x - 1, player.PlayerPos.y, player);
                        break;
                    case Players.Direction.Forward:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y + 1, player);
                        break;
                    case Players.Direction.Back:
                        dap = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y - 1, player);
                        break;
                }
                break;
        }

        return dap;
    }

    // v2.12 - added Reset method to the instructions to enable reuse by Function Blocks
    public new void Reset()
    {
        firstPlay = true;
    }
}
