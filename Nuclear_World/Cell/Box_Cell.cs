using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingSystem_HJH
{


    public class Box_Cell : Cell
    {
        bool first = true;
        public GameObject keyObject;
        public GameObject instantiatedKeyObj;
        public KeyType keyType;
        void Update()
        {
            if (onPlayer && first)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                first = false;
                GameObject key = Instantiate(keyObject, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                instantiatedKeyObj = key;
                keyObj = key.GetComponent<KeyObject>();
                keyObj.keyType = keyType;
                keyObj.KeyObjPos = new Vector2Int(posx, posy);
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
            first = true;
            transform.GetChild(0).gameObject.SetActive(true);
            Destroy(instantiatedKeyObj);
        }
    }
}   