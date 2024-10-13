// --- most used BE2 namespaces for instruction scripts 
using CodingSystem_HJH;
using Legacy.LJH;
using MG_BlocksEngine2.Block.Instruction;
using UnityEngine;

// --- additional BE2 namespaces used for specific cases as accessing BE2 variables or the event manager
// using MG_BlocksEngine2.Core;
// using MG_BlocksEngine2.Environment;

public class BE2_Cst_Move : BE2_InstructionBase, I_BE2_Instruction
{
    public bool ExecuteInUpdate => true;
    bool firstPlay = true;
    float timer = 0;
    bool GoMove = false;
    bool codeStartFirstPlay = true;
    Vector3 startPos;
    Vector3 endPos;
    public override void OnStackActive()
    {
        timer = 0;
        firstPlay = true;
        codeStartFirstPlay = true;
        CodeDone = false;
        CodeStart = false;
        GoMove = false;
    }
    public new void Function()
    {
        if (codeStartFirstPlay)
        {
            StartTurn();
            codeStartFirstPlay = false;
        }
        if (!CodeStart) { return; }
        if (firstPlay)
        {
            CodeDone = false;
            Players player = TargetObject.Transform.gameObject.GetComponent<Players>();
            startPos = TargetObject.Transform.position;
            firstPlay = false;
            switch (player.direction)
            {
                case Players.Direction.Forward:
                    GoMove = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y - 1, player);
                    endPos = startPos + Vector3.forward;
                    break;
                case Players.Direction.Left:
                    GoMove = GridManager.Instance.CanMoveCheck(player.PlayerPos.x -1, player.PlayerPos.y, player);
                    endPos = startPos - Vector3.right;
                    break;
                case Players.Direction.Back:
                    GoMove = GridManager.Instance.CanMoveCheck(player.PlayerPos.x, player.PlayerPos.y  +1, player);
                    endPos = startPos - Vector3.forward;
                    break;
                case Players.Direction.Right:
                    GoMove = GridManager.Instance.CanMoveCheck(player.PlayerPos.x + 1, player.PlayerPos.y, player);
                    endPos = startPos + Vector3.right;
                    break;
            }
            if (GoMove)
            {
                player.WalkStart();
            }
        }
        if (GoMove)
        {
            if (timer <= GridManager.Instance.moveSecond)
            {
                timer += Time.deltaTime;
                TargetObject.Transform.position = Vector3.Lerp(startPos, endPos, timer/GridManager.Instance.moveSecond);
            }
            else
            {
                Players player = TargetObject.Transform.gameObject.GetComponent<Players>();
                switch (player.direction)
                {
                    case Players.Direction.Forward:
                        if (GoMove) player.PlayerPos = new Vector2Int(player.PlayerPos.x,player.PlayerPos.y-1);
                        break;
                    case Players.Direction.Left:
                        if (GoMove) player.PlayerPos = new Vector2Int(player.PlayerPos.x-1, player.PlayerPos.y);
                        break;
                    case Players.Direction.Back:
                        if (GoMove) player.PlayerPos = new Vector2Int(player.PlayerPos.x, player.PlayerPos.y + 1);
                        break;
                    case Players.Direction.Right:
                        if (GoMove) player.PlayerPos = new Vector2Int(player.PlayerPos.x + 1, player.PlayerPos.y);
                        break;
                }
                player.WalkEnd();
                GoMove = false;
            }
        }
        else
        {
            CodeDone = true;
        }
    }

    public override void NextCodeStart()
    {
        timer = 0;
        firstPlay = true;
        codeStartFirstPlay = true;
        CodeDone = false;
        CodeStart = false;
        GoMove = false;
        ExecuteNextInstruction();
        
    }

}
