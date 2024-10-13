using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
namespace CodingSystem_HJH
{


    public class Players : SelectableObject
    {
        public GameObject[] robots;
        public Animator[] robotAnimators;

        public AudioSource keyObjAudio;
        KeyObject KeyObj;
        
        public KeyObject keyObj
        {
            get
            {
                return KeyObj;
            }
            set
            {
                if (value != null)
                {
                    ChangeRobot((int)value.keyType);
                    value.player = this;
                }
                else
                {
                    ChangeRobot(-1);
                }
                KeyObj = value;
            }
        }

        public void ChangeRobot(int key)
        {
            Transform robotTransform = transform;
            for (int i = 0; i < robots.Length; i++)
            {
                if (robots[i].activeInHierarchy)
                {
                    robotTransform = robots[i].transform;
                }
                robots[i].SetActive(false);
            }
            for (int i = 0; i < robots.Length; i++)
            {
                if (robotTransform != null)
                {
                    robots[i].transform.position = robotTransform.position;
                    robots[i].transform.rotation = robotTransform.rotation;
                }
            }
            robots[key + 1].SetActive(true);
        }

        
        public bool canMove = true;
        [SerializeField]
        private Vector2Int playerPos;
        public Vector2Int PlayerPos
        {
            get
            {
                return playerPos;
            }
            set
            {
                int beforeX = playerPos.x;
                int beforeY = playerPos.y;
                playerPos = value;
                GridManager.Instance.MoveCell(beforeX, beforeY, value.x, value.y, this);
            }
        }
        //0 = 앞, 1 = 오, 2 = 뒤, 3 = 좌
        public enum Direction
        {
            Forward,
            Right,
            Back,
            Left,
        }
        public Direction direction;
        // Start is called before the first frame update

        public void PickKeyItem()
        {
            if (keyObj == null)
            {
                if (GridManager.Instance.maps.GetCell(playerPos.x, playerPos.y).keyObj != null)
                {
                    keyObj = GridManager.Instance.maps.GetCell(playerPos.x, playerPos.y).keyObj;
                    GridManager.Instance.maps.GetCell(playerPos.x, playerPos.y).keyObj = null;
                    keyObj.gameObject.SetActive(false);
                    keyObjAudio.Play();
                }
            }
            else
            {
                if (GridManager.Instance.maps.GetCell(playerPos.x, playerPos.y).keyObj == null)
                {
                    keyObj.KeyObjPos = playerPos;
                    keyObj.gameObject.SetActive(true);
                    keyObj.transform.position = new Vector3(transform.position.x, keyObj.transform.position.y, transform.position.z);
                    keyObj.player = null;
                    keyObj = null;
                }
            }
        }

        void Start()
        {
            GridManager.Instance.MoveCell(0, 0, playerPos.x, playerPos.y, this);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        IEnumerator IdleCheck()
        {
            while (true)
            {
                bool end = false;
                yield return null;
                for (int i = 0; i < robotAnimators.Length; i++)
                {
                    if (robotAnimators[i].gameObject.activeInHierarchy)
                    {
                        if (!robotAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("rob1_idle"))
                        {
                            end = true;
                            break;
                        }
                    }
                }
                if (end)
                {
                    break;
                }
            }
            while (true)
            {
 
                yield return null;
                bool end = false;
                for(int i =0; i<robotAnimators.Length; i++)
                {
                    if (robotAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("rob1_idle"))
                    {
                        robotAnimators[i].transform.localPosition = Vector3.zero;
                        switch (direction)
                        {
                            case Direction.Forward:
                                robotAnimators[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
                                break;
                            case Direction.Right:
                                robotAnimators[i].transform.localRotation = Quaternion.Euler(0, 270, 0);
                                break;
                            case Direction.Back:
                                robotAnimators[i].transform.localRotation = Quaternion.identity;
                                break;
                            case Direction.Left:
                                robotAnimators[i].transform.localRotation = Quaternion.Euler(0, 90, 0);
                                break;
                        }
                        end = true;
                        break;

                    }
                }
                if (end)
                {
                    break;
                }
            }
        }

        //애니메이션 관리 함수
        public void WalkStart()
        {
            for(int i = 0; i< robotAnimators.Length; i++)
            {
                robotAnimators[i].SetTrigger("WalkStart");
            }
        }
        public void WalkEnd()
        {
            for (int i = 0; i < robotAnimators.Length; i++)
            {
                robotAnimators[i].SetTrigger("WalkEnd");
            }
        }
        public void TurnLeft()
        {
            for (int i = 0; i < robotAnimators.Length; i++)
            {
                robotAnimators[i].SetTrigger("TurnLeft");
                StartCoroutine(IdleCheck());
            }
        }
        public void TurnRight()
        {
            for (int i = 0; i < robotAnimators.Length; i++)
            {
                robotAnimators[i].SetTrigger("TurnRight");
                StartCoroutine(IdleCheck());
                
            }
        }

        public void StopAll()
        {
            for(int i =0; i< robots.Length; i++)
            {
                robots[i].transform.localPosition = Vector3.zero;
                robots[i].transform.localRotation = Quaternion.identity;
                
            }
        }
    }
}