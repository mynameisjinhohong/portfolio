using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingSystem_HJH
{
    public class Main_Cell : Cell
    {
        public bool isGoal = false;
        public MainAround_Cell[] aroundCells;
        private void Start()
        {
            for (int i = 0; i < aroundCells.Length; i++)
            {
                aroundCells[i].mainCell = this;
            }
        }
        public override bool CanMove(int x, int y, Players player)
        {
            return false;
        }

        public override void StopAll()
        {
            base.StopAll();
            isGoal = false;
        }

    }
}