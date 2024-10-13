using System.Collections;

using System.Collections.Generic;

using UnityEngine;



// --- most used BE2 namespaces for instruction scripts 

using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;
using CodingSystem_HJH;

public class BE2_Cst_DefenceKey1 : BE2_InstructionBase, I_BE2_Instruction
{
    public bool ExecuteInUpdate => true;
    bool firstPlay = true;
    bool codeStartFirstPlay = true;
    public override void OnStackActive()
    {
        firstPlay = true;
        codeStartFirstPlay = true;
        CodeStart = false;
        CodeDone = false;
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
            firstPlay = false;
            Defence_Cell defence = TargetObject.Transform.gameObject.GetComponent<Defence_Cell>();
            defence.KeyTypeCheck = KeyType.Blue;
            CodeDone = true;
        }
    }

    public override void NextCodeStart()
    {
        firstPlay = true;
        codeStartFirstPlay = true;
        CodeStart = false;
        CodeDone = false;
        ExecuteNextInstruction();
    }
}
