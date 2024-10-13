using MG_BlocksEngine2.DragDrop;
using MG_BlocksEngine2.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingSystem_HJH
{


    public class SelectableObject : MonoBehaviour
    {
        public int codeMax;
        public int nowCode = 0;
        public GameObject[] myBlocks;
        public BE2_ProgrammingEnv myCanvas;
        public int objectIdx;
        public bool canCoding;
        public bool canPutBlock;
        public bool firstCanCoding;
        void Start()
        {
            canPutBlock = true;
            firstCanCoding = canCoding;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}