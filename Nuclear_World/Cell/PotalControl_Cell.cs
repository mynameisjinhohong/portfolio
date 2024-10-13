using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingSystem_HJH
{


    public class PotalControl_Cell : Cell
    {
        public SelectableObject selectableObject;
        public PotalSet_cell potalSet_Cell;
        public bool firstCoding = true;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (onPlayer && firstCoding)
            {
                firstCoding = false;
                potalSet_Cell.FirstCheck();
                selectableObject.canCoding = true;
                CodingManager.Instance.Select(selectableObject);
                TurnManager.Instance.turnStop = true;
                //플레이어가 올라왔을때
            }
        }
        public override bool CanMove(int x, int y, Players player)
        {
            if (onPlayer == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void StopAll()
        {
            base.StopAll();
            firstCoding = true;
            potalSet_Cell.Active = false;
        }
    }
}