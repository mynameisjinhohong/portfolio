using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;
using CodingSystem_HJH;




public class BE2_Cst_ChangeKey : BE2_InstructionBase, I_BE2_Instruction
{
    public bool ExecuteInUpdate => true;
    bool firstPlay = true;
    bool codeStartFirstPlay = true;
    public override void OnStackActive()
    {
        firstPlay = true;
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
            ChangeControl_Cell changeControl = TargetObject.Transform.gameObject.GetComponent<ChangeControl_Cell>();
            if (Section0Inputs[0] != null)
            {
                changeControl.KeyType = (KeyType)(int)Section0Inputs[0].FloatValue;
            }
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
