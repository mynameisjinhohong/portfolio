using MG_BlocksEngine2.DragDrop;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace CodingSystem_HJH
{


    public class Defence_Cell : Cell
    {
        bool firstCoding = true;
        public MeshRenderer defenceMesh;
        public Material[] defenceMat;
        public SelectableObject selectableObject;
        public KeyType firstKeyType;
        [SerializeField]
        private KeyType keyTypeCheck;
        public KeyType KeyTypeCheck
        {
            get
            {
                return keyTypeCheck;
            }
            set
            {
                keyTypeCheck = value;
                defenceMesh.material = defenceMat[(int)keyTypeCheck];
            }
        }
        public GameObject defenceObj;
        public override bool CanMove(int x, int y, Players player)
        {
            if (onPlayer != null)
            {
                return false;
            }
            bool can = false;
            if (player.keyObj != null)
            {
                if (player.keyObj.keyType == keyTypeCheck)
                {
                    can = true;
                }
            }
            //나중에 조건에 맞춰서 이동 가능한지 불가능한지 체크하고 반환해야됨.
            return can;
        }

        // Start is called before the first frame update
        void Start()
        {
            KeyTypeCheck = firstKeyType;
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
        public override void StopAll()
        {
            base.StopAll();
            firstCoding = true;
            keyTypeCheck = firstKeyType;
        }
    }
}