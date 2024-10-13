using CodingSystem_HJH;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Environment;
using MG_BlocksEngine2.Serializer;
using MG_BlocksEngine2.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingSystem_HJH
{
    public class GamePlayManager : MonoBehaviour
    {

        public static GamePlayManager Instance;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        //이 스테이지가 어떤 스테이지인지
        public int stageNum;



        //플레이어 초기 상태
        [System.Serializable]
        public class PlayerFirstState
        {
            public Vector2Int playerPos;
            public Vector3 position;
            public Quaternion rotation;
            public KeyObject keyObj;
            public Players.Direction direction;
            public bool canMove;
        }
        //키오브젝트 초기 상태
        [System.Serializable]
        public class KeyObjFirstState
        {
            public Vector2Int objPos;
            public Vector3 position;
        }
        [System.Serializable] //코드 경로 및 코딩 환경
        public class CodeFirstState
        {
            public string paths;
            public BE2_ProgrammingEnv programmingEnv;
        }
        BE2_ProgrammingEnv[] allCanvas;
        public CodeFirstState[] codeFirstStates;
        public KeyObjFirstState[] keyObjFirstStates;
        KeyObject[] keyObjects;
        public PlayerFirstState[] playerFirstStates;
        //게임 클리어 관련
        public Main_Cell[] mainCells;
        public GameClearUI gameClearUI;
        public int playCount;
        bool gameClear = false;
        public bool GameClear
        {
            get
            {
                return gameClear;
            }
            set
            {
                if(value != gameClear)
                {
                    if (PlayerPrefs.GetInt("Stage" + stageNum) > TurnManager.Instance.turn)
                    {
                        PlayerPrefs.SetInt("Stage" + stageNum, TurnManager.Instance.turn);
                    }
                    gameClearUI.GameClear();
                    TurnManager.Instance.turnStop = true;
                    //게임 클리어할때 발생하는 일들 다 여기에 적어주기
                    gameClear = value;
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            if(!PlayerPrefs.HasKey("Stage" + stageNum))
            {
                PlayerPrefs.SetInt("Stage" + stageNum, 0);
            }
            mainCells = FindObjectsOfType<Main_Cell>();
            playerFirstStates = new PlayerFirstState[GridManager.Instance.players.Length];
            for (int i = 0; i < GridManager.Instance.players.Length; i++)
            {
                playerFirstStates[i] = new PlayerFirstState();
                playerFirstStates[i].playerPos = GridManager.Instance.players[i].PlayerPos;
                playerFirstStates[i].position = GridManager.Instance.players[i].transform.position;
                playerFirstStates[i].rotation = GridManager.Instance.players[i].transform.rotation;
                playerFirstStates[i].keyObj = GridManager.Instance.players[i].keyObj;
                playerFirstStates[i].direction = GridManager.Instance.players[i].direction;
                playerFirstStates[i].canMove = GridManager.Instance.players[i].canMove;
            }
            keyObjects = FindObjectsOfType<KeyObject>();
            keyObjFirstStates = new KeyObjFirstState[keyObjects.Length];
            for (int i = 0; i < keyObjects.Length; i++)
            {
                keyObjFirstStates[i] = new KeyObjFirstState();
                keyObjFirstStates[i].objPos = keyObjects[i].KeyObjPos;
                keyObjFirstStates[i].position = keyObjects[i].transform.position;
            }
            //SaveCode();
            StartCoroutine(SaveCo());
        }

        IEnumerator SaveCo()
        {
            allCanvas = FindObjectsOfType<BE2_ProgrammingEnv>();
            codeFirstStates = new CodeFirstState[allCanvas.Length - GridManager.Instance.players.Length];
            int minus = 0;
            for (int i = 0; i < allCanvas.Length; i++)
            {
                yield return null;
                Players player;
                if (!allCanvas[i].targetObject.TryGetComponent<Players>(out player))
                {
                    codeFirstStates[i - minus] = new CodeFirstState();
                    codeFirstStates[i - minus].programmingEnv = allCanvas[i];
                    codeFirstStates[i - minus].paths = BE2_Paths.TranslateMarkupPath(BE2_Paths.SavedCodesPath) + allCanvas[i].targetObject.name + ".BE2";
                    //Debug.Log(allCanvas[i].targetObject.name);
                    SaveCode(codeFirstStates[i - minus].paths, allCanvas[i]);
                }
                else
                {
                    minus++;
                }
            }
        }

        public void SaveCode()
        {
            allCanvas = FindObjectsOfType<BE2_ProgrammingEnv>();
            codeFirstStates = new CodeFirstState[allCanvas.Length - GridManager.Instance.players.Length];
            int minus = 0;
            for (int i = 0; i < allCanvas.Length; i++)
            {
                Players player;
                if (!allCanvas[i].targetObject.TryGetComponent<Players>(out player))
                {
                    codeFirstStates[i - minus] = new CodeFirstState();
                    codeFirstStates[i - minus].programmingEnv = allCanvas[i];
                    codeFirstStates[i - minus].paths = BE2_Paths.TranslateMarkupPath(BE2_Paths.SavedCodesPath) + allCanvas[i].targetObject.name + ".BE2";
                    //Debug.Log(allCanvas[i].targetObject.name);
                    SaveCode(codeFirstStates[i - minus].paths, allCanvas[i]);
                }
                else
                {
                    minus++;
                }
            }
        }
        public void SaveButton()
        {
            foreach (var item in codeFirstStates)
            {
                SaveCode(item.paths, item.programmingEnv);
            }
        }
        int test = 0;
        public void SaveCode(string path, BE2_ProgrammingEnv programmingEnv)
        {
            BE2_BlocksSerializer.SaveCode(path, programmingEnv);
            //Debug.Log("SaveCode : " + path + " : " + programmingEnv.BlocksList[0].Transform.GetComponentsInChildren<I_BE2_Block>().Length);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        public void LoadCode(string path, BE2_ProgrammingEnv programmingEnv)
        {
            if(BE2_BlocksSerializer.LoadCode(path, programmingEnv))
            {
                //Debug.Log("로드 성공");
            }
            else
            {
                //Debug.Log("로드 실패");
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
        public void StopAll()
        {
            //스탑 버튼 누르면 싹다 초기상태로 돌아가야됨
            TurnManager.Instance.StopAll();
            GridManager.Instance.StopAll();
            CodingManager.Instance.StopAll();
            for (int i = 0; i < playerFirstStates.Length; i++)
            {
                GridManager.Instance.players[i].PlayerPos = playerFirstStates[i].playerPos;
                GridManager.Instance.players[i].transform.position = playerFirstStates[i].position;
                GridManager.Instance.players[i].transform.rotation = playerFirstStates[i].rotation;
                GridManager.Instance.players[i].direction = playerFirstStates[i].direction;
                GridManager.Instance.players[i].canMove = playerFirstStates[i].canMove;
                GridManager.Instance.players[i].StopAll();
                GridManager.Instance.players[i].keyObj = playerFirstStates[i].keyObj;
            }
            for (int i = 0; i < keyObjFirstStates.Length; i++)
            {
                keyObjects[i].player = null;
                keyObjects[i].gameObject.SetActive(true);
                keyObjects[i].gameObject.transform.position = keyObjFirstStates[i].position;
                keyObjects[i].KeyObjPos = keyObjFirstStates[i].objPos;
                keyObjects[i].keyType = keyObjects[i].firstKeyType;
            }
            for (int i = 0; i < codeFirstStates.Length; i++)
            {
                codeFirstStates[i].programmingEnv.ClearBlocks();
                LoadCode(codeFirstStates[i].paths, codeFirstStates[i].programmingEnv);
                codeFirstStates[i].programmingEnv.UpdateBlocksList();
                //Debug.Log(codeFirstStates[i].paths + " : " + codeFirstStates[i].programmingEnv.BlocksList[0].Transform.GetComponentsInChildren<I_BE2_Block>().Length);
            }
        }
    }
}