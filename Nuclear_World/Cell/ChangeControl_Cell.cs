using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodingSystem_HJH
{


    public class ChangeControl_Cell : Cell
    {
        public SelectableObject selectableObject;
        public GameObject[] changeTiles;
        public Change_Cell[] changeCell;
        public Cell[] codingCells;
        private KeyType keyType = KeyType.White;
        bool firstCoding = true;

        public KeyType firstKeyType;
        public KeyType KeyType
        {
            get
            {
                return keyType;
            }
            set
            {
                if (value != keyType)
                {
                    keyType = value;
                    for (int i = 0; i < changeTiles.Length; i++)
                    {
                        changeTiles[i].SetActive(false);
                    }
                    changeTiles[(int)keyType].SetActive(true);
                    for(int i =0; i < changeCell.Length; i++)
                    {
                        changeCell[i].KeyType = keyType;
                    }
                }

            }
        }

        private void Start()
        {
            
            KeyType = firstKeyType;
        }

        private void Update()
        {
            for (int i = 0; i < codingCells.Length; i++)
            {
                if (codingCells[i].onPlayer != null)
                {
                    if (firstCoding)
                    {
                        firstCoding = false;
                        selectableObject.canCoding = true;
                        CodingManager.Instance.Select(selectableObject);
                        TurnManager.Instance.turnStop = true;
                        //플레이어가 올라왔을때
                    }
                }
            }
        }

        public override bool CanMove(int x, int y, Players player)
        {
            return false;
        }
        public override void OnPlayer(Players player, int beforeX, int beforeY)
        {
            base.OnPlayer(player, beforeX, beforeY);
            if (player.GetComponent<Players>().keyObj != null)
            {
                player.GetComponent<Players>().keyObj.keyType = keyType;
            }
        }
        public override void StopAll()
        {
            base.StopAll();
            firstCoding = true;
            KeyType = firstKeyType;
        }

    }
}