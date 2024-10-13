using System.Collections;

using System.Collections.Generic;

using UnityEngine;


using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;
using Legacy.LJH;
using CodingSystem_HJH;




public class BE2_Cst_TurnRightLeft : BE2_InstructionBase, I_BE2_Instruction
{
    public bool ExecuteInUpdate => true;
    float timer = 0;
    //Quaternion initialRotation;
    Vector3 direction;
    //string value;
    bool firstPlay = true;
    bool codeStartFirstPlay = true;
    Players player;
    public override void OnStackActive()
    {
        timer = 0;
        firstPlay = true;
        codeStartFirstPlay = true;
        CodeDone = false;
        CodeStart = false;
    }
    public new void Function()
    {
        if(codeStartFirstPlay)
        {
            StartTurn();
            codeStartFirstPlay = false;
        }
        if (!CodeStart) { return; }
        if (firstPlay)
        {
            CodeDone = false;
            player = TargetObject.Transform.gameObject.GetComponent<Players>();
            if (player.canMove)
            {
                switch (Section0Inputs[0].StringValue)
                {
                    case "Left":
                        direction = GetDirection(true);
                        player.TurnLeft();
                        break;
                    case "Right":
                        direction = GetDirection(false);
                        player.TurnRight();
                        break;
                }
            }
            //initialRotation = TargetObject.Transform.rotation;
            firstPlay = false;
        }
        if (timer <= GridManager.Instance.moveSecond)
        {
            timer += Time.deltaTime;
            //TargetObject.Transform.rotation = Quaternion.Lerp(initialRotation,Quaternion.LookRotation(direction), timer / GridManager.Instance.moveSecond);
        }
        else
        {
            CodeDone = true;
        }

    }
    public Vector3 GetDirection(bool left)
    {
        if (left)
        {
            switch (player.direction)
            {
                case Players.Direction.Left:
                    player.direction = Players.Direction.Back;
                    return Vector3.back;
                case Players.Direction.Right:
                    player.direction = Players.Direction.Forward;
                    return Vector3.forward;
                case Players.Direction.Forward:
                    player.direction = Players.Direction.Left;
                    return Vector3.left;
                case Players.Direction.Back:
                    player.direction = Players.Direction.Right;
                    return Vector3.right;
            }
        }
        else
        {
            switch (player.direction)
            {
                case Players.Direction.Left:
                    player.direction = Players.Direction.Forward;
                    return Vector3.forward;
                case Players.Direction.Right:
                    player.direction = Players.Direction.Back;
                    return Vector3.back;
                case Players.Direction.Forward:
                    player.direction = Players.Direction.Right;
                    return Vector3.right;
                case Players.Direction.Back:
                    player.direction = Players.Direction.Left;
                    return Vector3.left;
            }
        }
        return Vector3.zero;
    }
    public override void NextCodeStart()
    {
        timer = 0;
        firstPlay = true;
        codeStartFirstPlay = true;
        CodeDone = false;
        CodeStart = false;
        ExecuteNextInstruction();

    }
}
