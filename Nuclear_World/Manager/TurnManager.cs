using MG_BlocksEngine2.Block.Instruction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
namespace CodingSystem_HJH
{


    public class TurnManager : MonoBehaviour
    {
        public static TurnManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool codeStart = false; //코드 시작 버튼을 눌렀을때 BE2_ExecuteCode에서 true로 바뀜
        public int codeIdx = 0; // 몇번째 블록이 실행되고 있는지
        public int turn = 0;
        public BE2_InstructionBase[] blocks;
        public BE2_InstructionBase[] nextBlocks;
        public bool turnStart = false; // 각 블록에서 턴 시작을 알림
        public bool turnStop = false; //만약 다른 코딩 가능한 오브젝트에 올라서면 턴스탑 바꿔주면 됨
        public float minTurnTime = 0.5f;
        public float turnTimer = 0;
        public TMP_Text turnText;
        // Start is called before the first frame update
        void Start()
        {
            blocks = new BE2_InstructionBase[CodingManager.Instance.selectableObjects.Length];
            nextBlocks = new BE2_InstructionBase[CodingManager.Instance.selectableObjects.Length];
        }

        // Update is called once per frame
        void Update()
        {
            if(turnText != null)
            {
                turnText.text = "현재 턴:" + turn;
            }
            if (turnStart)
            {
                if (turnStop)
                {
                    return;
                }
                turnTimer += Time.deltaTime;
                if (turnTimer < minTurnTime)
                {
                    return;
                }
                bool blockNull = true;
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i] != null)
                    {
                        blockNull = false;
                        break;
                    }
                }
                if (blockNull)
                {
                    blocks = nextBlocks.ToArray();
                    ClearBlock(nextBlocks);
                }
                if (blocks[codeIdx] != null)
                {
                    blocks[codeIdx].CodeStart = true;
                    if (blocks[codeIdx].CodeDone)
                    {
                        codeIdx++;
                    }
                }
                else
                {
                    codeIdx++;
                }
                if (codeIdx < blocks.Length)
                {
                    return;
                }
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i] != null)
                    {
                        blocks[i].NextCodeStart();
                    }
                    if (i == blocks.Length - 1)
                    {
                        ClearBlock(blocks);
                        turnStart = false;
                        turn++;
                        codeIdx = 0;
                        turnTimer = 0;
                        for (int j = 0; j < GamePlayManager.Instance.mainCells.Length; j++)
                        {
                            if (!GamePlayManager.Instance.mainCells[j].isGoal)
                            {
                                break;
                            }
                            if (j == GamePlayManager.Instance.mainCells.Length - 1)
                            {
                                if (GamePlayManager.Instance.GameClear == false)
                                {
                                    GamePlayManager.Instance.GameClear = true;
                                }
                            }
                        }
                    }
                }

            }
        }

        public void CodeStart(SelectableObject obj, BE2_InstructionBase block)
        {
            if (turnStart == false)
            {
                turnStart = true;
            }
            nextBlocks[obj.objectIdx] = block;
        }

        public void ClearBlock(BE2_InstructionBase[] blocks)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = null;
            }
        }

        public void StopAll()
        {
            turn = 0;
            turnTimer = 0;
            codeStart = false;
            codeIdx = 0;
            turnStart = false;
            turnStop = false;
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i] != null)
                {
                    blocks[i].OnStackActive();
                    blocks[i] = null;
                }
            }
            for (int i = 0; i < nextBlocks.Length; i++)
            {
                if (nextBlocks[i] != null)
                {
                    nextBlocks[i].OnStackActive();
                    blocks[i] = null;
                }
            }
        }

    }

}