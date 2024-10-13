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

        public bool codeStart = false; //�ڵ� ���� ��ư�� �������� BE2_ExecuteCode���� true�� �ٲ�
        public int codeIdx = 0; // ���° ����� ����ǰ� �ִ���
        public int turn = 0;
        public BE2_InstructionBase[] blocks;
        public BE2_InstructionBase[] nextBlocks;
        public bool turnStart = false; // �� ��Ͽ��� �� ������ �˸�
        public bool turnStop = false; //���� �ٸ� �ڵ� ������ ������Ʈ�� �ö󼭸� �Ͻ�ž �ٲ��ָ� ��
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
                turnText.text = "���� ��:" + turn;
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