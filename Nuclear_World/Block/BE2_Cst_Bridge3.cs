using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;

using MG_BlocksEngine2.Block;




public class BE2_Cst_Bridge3 : BE2_InstructionBase, I_BE2_Instruction
{




    // --- Method used to implement Operation Blocks (will only be called by type: operation)

    public new string Operation()

    {

        string result = "2";

        

        // --- use Section0Inputs[inputIndex] to get the Block inputs from the first section (index 0).

        // --- Optionally, use GetSectionInputs(sectionIndex)[inputIndex] to get inputs from a different section

        // --- the input values can be retrieved as .StringValue, .FloatValue or .InputValues 

        // Section0Inputs[inputIndex];

        

        // --- opeartion results are always of type string.

        // --- bool return strings are usually "1", "true", "0", "false".

        // --- numbers are returned as strings and converted on the input get.

        return result;

    }

}
