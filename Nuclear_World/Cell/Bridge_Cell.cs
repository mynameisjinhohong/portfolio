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
                //��� ��Ȱ��ȭ �ɶ� ó�� ���ֱ�
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