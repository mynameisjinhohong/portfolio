using System.Collections;

using System.Collections.Generic;

using UnityEngine;


using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;




public class BE2_Cst_Bridge2 : BE2_InstructionBase, I_BE2_Instruction
{

    public new string Operation()

    {

        string result = "1";


        return result;

    }



    // ### Execution Setting ###



    // --- Use ExecuteInUpdate for functions that plays repeatedly in update, holding the blocks stack execution flow until completed (ex.: wait, lerp).

    // --- Default value is false. Loop Blocks are always executed in update (true).

    //public new bool ExecuteInUpdate => true; 



    // ### Additional Methods ###



    // --- executed after base Awake

    //protected override void OnAwake()

    //{

    //    

    //}

    

    // --- executed after base Start

    //protected override void OnStart()

    //{

    //    

    //}



    // --- Update can be overridden

    //void Update()

    //{

    //

    //}



    // --- executed on button play pressed

    //protected override void OnButtonPlay()

    //{

    //

    //}



    // --- executed on button stop pressed

    //protected override void OnButtonStop()

    //{

    //

    //}



    // --- executed after blocks stack is populated

    //public override void OnPrepareToPlay()

    //{

    //

    //}



    // --- executed on the stack transition from deactive to active

    //public override void OnStackActive()

    //{

    //

    //}



    //protected override void OnEnableInstruction()

    //{

    //

    //}



    //protected override void OnDisableInstruction()

    //{

    //

    //}

}
