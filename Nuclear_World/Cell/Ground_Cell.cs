using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodingSystem_HJH
{


    public class Ground_Cell : Cell
    {
        public int groundIdx = 0;

        private void Start()
        {
            //groundIdx = Random.Range(0, transform.childCount);
            //for(int i =0; i < transform.childCount; i++)
            //{
            //    if(i != groundIdx)
            //    {
            //        transform.GetChild(i).gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        transform.GetChild(i).gameObject.SetActive(true);
            //    }
            //}
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
    }
}