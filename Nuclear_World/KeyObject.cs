using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodingSystem_HJH
{


    public enum KeyType
    {
        White = -1,
        Yellow,
        Blue,
        Red,
    }

    public class KeyObject : MonoBehaviour
    {
        public Players player;
        public float rotateSpeed;
        public GameObject keyObj;
        public KeyType firstKeyType;
        KeyType KeyType;
        public Material[] keyMat;

        public KeyType keyType
        {
            get
            {
                return KeyType;
            }
            set
            {
                keyObj.GetComponent<MeshRenderer>().material = keyMat[(int)value];
                if(player != null)
                {
                    player.ChangeRobot((int)value);
                }
                KeyType = value;
            }
        }

        [SerializeField]
        private Vector2Int keyObjPos;
        public Vector2Int KeyObjPos

        {
            get
            {
                return keyObjPos;
            }
            set
            {
                int beforeX = keyObjPos.x;
                int beforeY = keyObjPos.y;
                if (GridManager.Instance.maps.GetCell(beforeX, beforeY) != null)
                {
                    GridManager.Instance.maps.GetCell(beforeX, beforeY).keyObj = null;
                }
                keyObjPos = value;
                GridManager.Instance.maps.GetCell(value.x, value.y).keyObj = this;
            }
        }
        private void Awake()
        {
            keyType = firstKeyType;

        }
        private void Start()
        {
            if (GridManager.Instance.maps.GetCell(keyObjPos.x, keyObjPos.y) != null)
            {
                GridManager.Instance.maps.GetCell(keyObjPos.x, keyObjPos.y).keyObj = this;
            }
        }

        private void Update()
        {
            keyObj.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }

    }
}