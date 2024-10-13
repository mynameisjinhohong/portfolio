using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;
using CodingSystem_HJH;


public class BE2_Cst_PickUp : BE2_InstructionBase, I_BE2_Instruction
{
    public bool ExecuteInUpdate => true;
    bool firstPlay = true;
    bool codeStartFirstPlay = true;
    public override void OnStackActive()
    {
        firstPlay = true;
        CodeStart = false;
        CodeDone = false;
        codeStartFirstPlay = true;
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
            CodeDone = true;
            Players player = TargetObject.Transform.gameObject.GetComponent<Players>();
            player.PickKeyItem();
            firstPlay = false;
        }

    }
    public override void NextCodeStart()
    {
        firstPlay = true;
        CodeStart = false;
        CodeDone = false;
        codeStartFirstPlay = true;
        ExecuteNextInstruction();

    }
}
