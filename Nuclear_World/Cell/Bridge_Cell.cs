using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodingSystem_HJH
{


    public class Bridge_Cell : Cell
    {
        private bool active = false;
        public GameObject bridgeLight;
        public GameObject cellLight;
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                bridgeLight.SetActive(active);
                cellLight.SetActive(active);
                //블록 비활성화 될때 처리 해주기
            }
        }
        public override bool CanMove(int x, int y, Players player)
        {
            if (onPlayer == null)
            {
                return active;
            }
            else
            {
                return false;
            }

        }
    }
}