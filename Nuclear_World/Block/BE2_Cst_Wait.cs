using System.Collections;

using System.Collections.Generic;

using UnityEngine;



// --- most used BE2 namespaces for instruction scripts 

using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;
using Unity.VisualScripting;
using CodingSystem_HJH;




public class BE2_Cst_Wait : BE2_InstructionBase, I_BE2_Instruction
{
    public bool ExecuteInUpdate => true;
    bool startNext = false;
    bool firstPlay = true;
    int term = 0;
    bool codeStartFirstPlay = true;
    bool wait = false;
    float waitTime = 0.5f;
    float timer = 0;
    public override void OnStackActive()
    {
        timer = 0;
        codeStartFirstPlay = true;
        firstPlay = true;
        CodeDone = false;
        CodeStart = false;
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
            term = (int)Section0Inputs[0].FloatValue;
            firstPlay = false;
            Debug.Log("Wait Start");
        }
        else
        {
            timer += Time.deltaTime;
            if(timer > waitTime)
            {
                CodeDone = true;
            }

        }
        if (startNext)
        {
            if (TurnManager.Instance.turnStart)
            {
                startNext = false;
                StartTurn();
            }
        }

    }
    public override void NextCodeStart()
    {
        if(term > 1)
        {
            term--;
            startNext = true;
            timer = 0;
            return;
        }
        else
        {
            timer = 0;
            codeStartFirstPlay = true;
            firstPlay = true;
            CodeDone = false;
            CodeStart = false;
            ExecuteNextInstruction();

        }

    }
}
