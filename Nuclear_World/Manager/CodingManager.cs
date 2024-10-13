using Legacy.LJH;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.DragDrop;
using MG_BlocksEngine2.Environment;
using MG_BlocksEngine2.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodingSystem_HJH
{


    public class CodingManager : MonoBehaviour
    {
        public static CodingManager Instance;


        public BE2_UI_SelectionPanel[] selectionPanels;
        public BE2_ProgrammingEnv[] allCanvas;
        public BE2_UI_SelectionBlock[] allBlock;
        public SelectableObject[] selectableObjects;
        public SelectableObject firstSelectableObj;
        public SelectableObject nowSelect;
        public TMP_Text blockText;
        public GameObject startBlock;
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
            BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDrop, UpdateSelectionBlock);
            BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDrop, DropBlockCheck);
            BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDragOut, UpdateSelectionBlock);
            selectableObjects = FindObjectsOfType<SelectableObject>();
            for (int i = 0; i < selectableObjects.Length; i++)
            {
                selectableObjects[i].objectIdx = i;
            }
            allCanvas = FindObjectsOfType<BE2_ProgrammingEnv>();
            allBlock = FindObjectsOfType<BE2_UI_SelectionBlock>();
        }

        private void Start()
        {
            Select(firstSelectableObj);
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    SelectableObject select = null;
                    Transform target = hit.collider.transform;
                    while(target != null)
                    {
                        target.TryGetComponent<SelectableObject>(out select);
                        if (select != null)
                        {
                            break;
                        }
                        else
                        {
                            target = target.parent;
                        }
                    }
                    if(select != null)
                    {
                        Select(select);
                    }
                }
            }
        }

        void UpdateSelectionBlock(I_BE2_Block block)
        {
            StartCoroutine(C_VerifyBlock());
            if (nowSelect.codeMax <= nowSelect.nowCode)
            {
                nowSelect.canPutBlock = false;
                BE2_DragDropManager.Instance.canCoding = nowSelect.canCoding;
            }
            else
            {
                nowSelect.canPutBlock = true;
                BE2_DragDropManager.Instance.canCoding = nowSelect.canCoding;
            }
        }
        void DropBlockCheck(I_BE2_Block block)
        {
            if (!nowSelect.canPutBlock)
            {
                Destroy(block.Transform.gameObject);
            }
        }

        IEnumerator C_VerifyBlock()
        {
            // make sure the blocks are placed/removed before counting 
            yield return new WaitForEndOfFrame();

            int tempCount = 0;
            nowSelect.myCanvas.UpdateBlocksList();
            foreach (I_BE2_Block block in nowSelect.myCanvas.BlocksList)
            {
                if (block.Type == BlockTypeEnum.trigger)
                {
                    tempCount += block.Transform.GetComponentsInChildren<I_BE2_Block>().Length;
                }
            }
            nowSelect.nowCode = tempCount -1;
            blockText.text = "남은 블럭 수 : " + (nowSelect.codeMax - nowSelect.nowCode);
        }
        //public void DropBlock(I_BE2_Block block)
        //{
        //    if (nowSelect != null)
        //    {
        //        if (!nowSelect.canPutBlock)
        //        {
        //            Destroy(block.Transform.gameObject);
        //        }
        //        else
        //        {
        //            nowSelect.nowCode++;
        //            if (nowSelect.nowCode >= nowSelect.codeMax)
        //            {
        //                nowSelect.canPutBlock = false;
        //                BE2_DragDropManager.Instance.canCoding = nowSelect.canCoding;
        //            }
        //            blockText.text = "남은 블럭 수 : " + (nowSelect.codeMax - nowSelect.nowCode);
        //        }
        //    }
        //}

        public void TrashBlock() //현재 활성화된 블록을 제외
        {
            nowSelect.myCanvas.ClearBlocks();
            GameObject startB = Instantiate(startBlock, nowSelect.myCanvas.transform);
            startB.transform.localPosition = new Vector3(200, -200, 0);
            startB.transform.localScale = new Vector3(1, 1, 1);
            startB.name = startBlock.name;
            BE2_Block newBlock = startB.GetComponent<BE2_Block>();
            newBlock.Drag.OnPointerUp();
            nowSelect.nowCode = 0;
            nowSelect.canPutBlock = true;
            blockText.text = "남은 블럭 수 : " + nowSelect.codeMax;
            nowSelect.myCanvas.UpdateBlocksList();
        }

        //public void DestroyBlock(I_BE2_Block block)
        //{
        //    if (nowSelect != null)
        //    {
        //        nowSelect.nowCode -= block.Transform.GetComponentsInChildren<I_BE2_Block>().Length; ;
        //        if (nowSelect.nowCode < nowSelect.codeMax)
        //        {
        //            nowSelect.canPutBlock = true;
        //            BE2_DragDropManager.Instance.canCoding = nowSelect.canCoding;
        //        }
        //        blockText.text = "남은 블럭 수 : " + (nowSelect.codeMax - nowSelect.nowCode);
        //    }
        //}

        public void Select(SelectableObject select)
        {
            select.myCanvas.UpdateBlocksList();
            nowSelect = select;
            StartCoroutine(C_VerifyBlock());
            for (int i = 0; i < allBlock.Length; i++)
            {
                allBlock[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < select.myBlocks.Length; i++)
            {
                if (select.myBlocks[i] != null)
                {
                    select.myBlocks[i].SetActive(true);
                }
            }
            for (int i = 0; i < allCanvas.Length; i++)
            {
                allCanvas[i].Visible = false;
            }
            select.myCanvas.Visible = true;
            for (int i = 0; i < selectionPanels.Length; i++)
            {
                if (selectionPanels[i] != null)
                {
                    selectionPanels[i].UpdateLayout();
                }
            }
            BE2_DragDropManager.Instance.canCoding = select.canCoding;
        }

        public void StopAll()
        {
            for (int i = 0; i < selectableObjects.Length; i++)
            {
                if (selectableObjects[i] is Players)
                {

                }
                else
                {
                    selectableObjects[i].nowCode = 0;
                    selectableObjects[i].canCoding = selectableObjects[i].firstCanCoding;
                }
            }
            if(nowSelect is Players)
            {
                BE2_DragDropManager.Instance.canCoding = true;
            }
            else
            {
                BE2_DragDropManager.Instance.canCoding = false;
            }
        }


    }
}