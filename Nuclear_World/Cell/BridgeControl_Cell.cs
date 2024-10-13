using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodingSystem_HJH
{


    [System.Serializable]
    public struct BridgeWay
    {
        public Bridge_Cell[] bridge_Cell;
    }
    public class BridgeControl_Cell : Cell
    {
        public SelectableObject selectableObject;
        bool firstCoding = true;
        public List<BridgeWay> bridgeWays;
        private int way = -1;
        public AudioSource bridgeSound;
        bool firstPlay = true;
        public int Way
        {
            get
            {
                return way;
            }
            set
            {
                if (way != value)
                {
                    way = value;
                    SetWay(way);
                }
            }

        }
        public int firstWay;
        void Start()
        {
            Way = firstWay;
        }

        // Update is called once per frame
        void Update()
        {
            if (onPlayer && firstCoding)
            {
                firstCoding = false;
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
        public void SetWay(int way)
        {
            for (int i = 0; i < bridgeWays.Count; i++)
            {
                for (int j = 0; j < bridgeWays[i].bridge_Cell.Length; j++)
                {
                    bridgeWays[i].bridge_Cell[j].Active = false;
                }
            }
            for (int j = 0; j < bridgeWays[way].bridge_Cell.Length; j++)
            {
                bridgeWays[way].bridge_Cell[j].Active = true;
            }
            if (!firstPlay)
            {
                bridgeSound.Play();
            }
            else
            {
                firstPlay = false;
            }
        }
        public override void StopAll()
        {
            base.StopAll();
            firstCoding = true;
            Way = firstWay;
        }
    }
}