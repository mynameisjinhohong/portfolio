using System;
using BackEnd;
using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePlayManager : Singleton<GamePlayManager>
{
    //
    public MainUI_HJH mainUi;
    public List<Player_HJH> players;
    public List<GameObject> playerPrefabs;
    public PlayerDeck_HJH playerDeck;
    public int myIdx;
    public int SuperGamerIdx;
    public GameBoard_PCI gameBoard;
    public GameObject playerPool;
    public Transform[] PlayerSpawnPosition;
    #region ȣ��Ʈ
    public Queue<Message> messageQueue;
    #endregion
    public List<Color> colorList;

    public bool isUseChaser;
    public bool isSoloTest;
    public GameObject chaserObj;
    public Chaser chaser;
    public bool isWaiting;

    public int classSelectedUser;

    public int errorCount;
    
    //�� ���� �����밪
    public int testValue;
    
    // Start is called before the first frame update
    void Start()
    {
        classSelectedUser = 0;
        
        BackendManager.Instance.DoClassChoiceTime();

        // �Ѿ�� ������ ���� ���� ����Ʈ �ε��� ����
        BackendManager.Instance.userDataList.Sort((UserData lhs, UserData rhs) =>
        {
            if (int.Parse(lhs.playerToken) < int.Parse(rhs.playerToken))
            {
                return 1;
            }
            else if (lhs.playerToken == rhs.playerToken)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        });
        
        // ���ĵ� �ε����� �÷��̾� ����
        DataInit();
    }

    public void SetResolution()
    {
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

    public void DataInit()
    {
        errorCount = 0;
        
        players = new List<Player_HJH>();
        for (int i = 0; i < playerPrefabs.Count; i++)
        {
            if (i < BackendManager.Instance.userDataList.Count)
            {
                GameObject PlayerPrefab = Instantiate(playerPrefabs[i], PlayerSpawnPosition[i]);
                Player_HJH playerHjh = PlayerPrefab.GetComponent<Player_HJH>();
                players.Add(playerHjh);
                playerHjh.isSuperGamer = BackendManager.Instance.userDataList[i].isSuperGamer;
                playerHjh.PlayerName.text = BackendManager.Instance.userDataList[i].playerName;

                if (BackendManager.Instance.userInfo.Nickname == BackendManager.Instance.userDataList[i].playerName)
                {
                    playerHjh.isMine = true;
                    mainUi.myPlayer = playerHjh;
                    mainUi.myPlayer.isMeIconObj.SetActive(true);
                    mainUi.myPlayer.KeysOnValueChanged += mainUi.SetKeysUI;
                }
                playerHjh.PlayerToken = BackendManager.Instance.userDataList[i].playerToken;

                if (BackendManager.Instance.userDataList[i].isSuperGamer)
                {
                    SuperGamerIdx = i;
                }

                players[i].gameObject.SetActive(true);
            }

        }

        for (int i = 0; i < BackendManager.Instance.userDataList.Count; i++)
        {
            if (BackendManager.Instance.userInfo.Nickname == BackendManager.Instance.userDataList[i].playerName)
            {
                myIdx = i;

                Transform parentTransform = PlayerSpawnPosition[myIdx].transform;

                if(Camera.main.TryGetComponent<CameraManager_HJH>(out var cam))
                {
                    cam.target = parentTransform.GetChild(0);
                    cam.transform.position = cam.target.position + new Vector3(0.5f, 0.5f, -10f);
                }
                else
                {
                    Camera.main.transform.SetParent(parentTransform.GetChild(0));

                    Camera.main.transform.localPosition = new Vector3(0.5f, 0.5f, -10f);
                }
            }
        }

        mainUi.playerBG.color = colorList[myIdx];
        
        // �÷��̾� ���� �Ϸ� �� �� ���� �غ�
        InitializeGame();
    }
    // public bool CardIdxCheckNoPlayer(int cardIdx, Transform playerPos)
    // {
    //     Vector2 goPos = new Vector2();
    //     switch (Mathf.Abs(cardIdx))
    //     {
    //         case 1:
    //             if(cardIdx > 0)
    //             {
    //                 goPos = new Vector2(playerPos.position.x, playerPos.position.y + 2);
    //             }
    //             else
    //             {
    //                 goPos = new Vector2(playerPos.position.x, playerPos.position.y + 1);
    //             }
    //             break;
    //         case 2:
    //             if (cardIdx > 0)
    //             {
    //                 goPos = new Vector2(playerPos.position.x + 2, playerPos.position.y);
    //             }
    //             else
    //             {
    //                 goPos = new Vector2(playerPos.position.x + 1, playerPos.position.y);
    //             }
    //             break;
    //         case 3:
    //             if (cardIdx > 0)
    //             {
    //                 goPos = new Vector2(playerPos.position.x - 2, playerPos.position.y);
    //             }
    //             else
    //             {
    //                 goPos = new Vector2(playerPos.position.x - 1, playerPos.position.y);
    //             }
    //             break;
    //         case 4:
    //             if (cardIdx > 0)
    //             {
    //                 goPos = new Vector2(playerPos.position.x, playerPos.position.y - 2);
    //             }
    //             else
    //             {
    //                 goPos = new Vector2(playerPos.position.x, playerPos.position.y - 1);
    //             }
    //             break;
    //         case 5:
    //             if (cardIdx > 0)
    //             {
    //                 goPos = new Vector2(playerPos.position.x, playerPos.position.y - 2);
    //             }
    //             else
    //             {
    //                 goPos = new Vector2(playerPos.position.x, playerPos.position.y - 1);
    //             }
    //             break;
    //     }
    //     if(Mathf.Abs(cardIdx) == 6)
    //     {
    //         if(cardIdx > 0)
    //         {
    //             Random.InitState(CardManager.Instance.seed);
    //             int x = Random.Range(-2, 3);
    //             int y = Random.Range(-2, 3);
    //             if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)playerPos.transform.position.x + x, (int)playerPos.transform.position.y + y)))
    //             {
    //                 goPos = new Vector3(playerPos.transform.position.x + x, playerPos.transform.position.y + y, playerPos.transform.position.z);
    //             }
    //             else
    //             {
    //                 for (int i = -2; i < 3; i++)
    //                 {
    //                     for (int j = -2; j < 3; j++)
    //                     {
    //                         if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)playerPos.transform.position.x + j, (int)playerPos.transform.position.y + i)))
    //                         {
    //                             goPos = new Vector3(playerPos.transform.position.x + j, playerPos.transform.position.y + i, playerPos.transform.position.z);
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             Random.InitState(CardManager.Instance.seed);
    //             int x = Random.Range(-3, 4);
    //             int y = Random.Range(-3, 4);
    //             if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)playerPos.transform.position.x + x, (int)playerPos.transform.position.y + y)))
    //             {
    //                 goPos = new Vector3(playerPos.transform.position.x + x, playerPos.transform.position.y + y, playerPos.transform.position.z);
    //             }
    //             else
    //             {
    //                 for (int i = -3; i < 4; i++)
    //                 {
    //                     for (int j = -3; j < 4; j++)
    //                     {
    //                         if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)playerPos.transform.position.x + j, (int)playerPos.transform.position.y + i)))
    //                         {
    //                             goPos = new Vector3(playerPos.transform.position.x + j, playerPos.transform.position.y + i, playerPos.transform.position.z);
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //     return CheckNoPlayer(goPos);
    // }
    // public bool CheckNoPlayer(Vector2 goPos)
    // {
    //     for (int i = 0; i < players.Count; i++)
    //     {
    //         Vector2 vec = new Vector2((int)players[i].transform.position.x, (int)players[i].transform.position.y);
    //         if ((Vector2)goPos == vec)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    public void InitializeGame()
    {
        BackendManager.Instance.userGradeList = new List<MatchUserGameRecord>();

        // �������� ���� �޼��� ������ �غ�
        Backend.Match.OnMatchRelay = (MatchRelayEventArgs args) => //������ ���� �޼����� Ŭ���̾�Ʈ�� �ݹ����� �� ȣ��Ǵ� �̺�Ʈ
            {
                // ���� ���� �޼����� ���� ó��
                if (args.From.NickName == BackendManager.Instance.userInfo.Nickname)
                {
                    Debug.Log("���� ���� �޼��� ����");
                    return;
                }

                var strByte = System.Text.Encoding.Default.GetString(args.BinaryUserData);
                Message msg = JsonUtility.FromJson<Message>(strByte);

                if (BackendManager.Instance.isMeSuperGamer)
                {
                    Debug.Log("���۰��̸��� ���� �޼����� �۽� �մϴ�");
                    messageQueue.Enqueue(msg);

                    // ���۰��̸Ӱ� �й� ó�� ��û ������ ��� �������� ���� ����
                    if (msg.playerIdx == -99)
                    {
                        if (BackendManager.Instance.winUser == "")
                        {
                            Debug.Log("�¸����� ���� ����־ ���� ó��");
                            return;
                        }
                            
                        Debug.Log($"{BackendManager.Instance.winUser}�¸� ó�� �޼��� ����");
                        BackendManager.Instance.SendResultToServer();
                    }
                    // ������ Ŭ���� �����ϸ� ���۰��̸ӵ� ����
                    else if (msg.cardIdx <= -100)
                    {
                        Debug.Log($"{msg.playerIdx}���� {msg.cardIdx} Ŭ���� ����");
                    }
                }
                else
                {
                    Debug.Log("���۰��̸Ӱ� �ƴ�  ���� �޼����� �����մϴ�");
                    //���� ������ ���࿡ ���۰��̸Ӱ� �����Ÿ�
                    if (args.From.NickName == BackendManager.Instance.userDataList[SuperGamerIdx].playerName) 
                    {
                        if (msg.playerIdx == -10) //-10 �÷��̾� �ε����� ������ ���� �����Ѵ�.
                        {
                            int head = BackendManager.Instance.userDataList.Count;
                            
                            Debug.Log($"�ʻ��� ����{head}");
                            
                            switch (head)
                            {
                                case 1:
                                    gameBoard.Generate(msg.cardIdx, 12, 12, 2);
                                    break;
                                case 2:
                                    gameBoard.Generate(msg.cardIdx, 12, 12, 2);
                                    break;
                                case 3:
                                case 4:
                                    gameBoard.Generate(msg.cardIdx, 14, 14, 4);
                                    break;
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    gameBoard.Generate(msg.cardIdx, 16, 16, 8);
                                    break;
                                default:
                                    UIManager.Instance.OpenRecyclePopup("�ý��� ����", "�� ���� ����", Application.Quit);
                                    break;
                            }
                            CardManager.Instance.seed = msg.cardIdx;
                        }
                        else if (msg.cardIdx <= -100)
                        {
                            Debug.Log($"{msg.playerIdx}���� {msg.cardIdx} Ŭ���� ����");
                        }
                        else if (msg.playerIdx <= 9)
                        {
                            Debug.Log(msg.playerIdx + "  " + msg.cardIdx);
                            CardRealGo(msg.playerIdx, msg.cardIdx); //Ư�� ī�带 ����� �÷��̾ �޾Ƽ� ���� ����
                        }
                    }
                    else
                    {
                        Debug.Log("���۰��̸Ӱ� �ƴ� �������� ������");
                    }
                }
                //Debug.Log($"�������� ���� ������ : {args.From.NickName} : {msg.ToString()}");
            };
        //gameRecord = new Stack<SessionId>();
        //GameManager.OnGameOver += OnGameOver;
        //GameManager.OnGameResult += OnGameResult;
        //myPlayerIndex = SessionId.None;
        //SetPlayerAttribute();
        //OnGameStart();
        
        Backend.Match.OnSessionOffline = (MatchInGameSessionEventArgs args) => {
            
            Debug.Log(args.GameRecord.m_nickname + "���� ������ �����ϼ̽��ϴ�.");

            int OutUserIndex = 0;
            
            for (int i = 0; i < BackendManager.Instance.userDataList.Count; i++)
            {
                if (BackendManager.Instance.userDataList[i].playerName == args.GameRecord.m_nickname)
                {
                    OutUserIndex = i;
                }
            }

            //BackendManager.Instance.userDataList.RemoveAt(OutUserIndex);
            //Destroy(PlayerSpawnPosition[OutUserIndex].gameObject);
            
            // �������� hp 0 ó��, ��Ȱ��ȭ
            players[OutUserIndex].hp = 0;
            PlayerSpawnPosition[OutUserIndex].gameObject.SetActive(false);
            
            foreach (var userData in BackendManager.Instance.inGameUserList)
            {
                if (userData.Value.m_nickname == players[OutUserIndex].PlayerName.text)
                {
                    Debug.Log($"{userData.Value.m_nickname}���� Ż���Ͽ� �й�ó��");
                    
                    // �̹� �й��� ������� ����Ʈ�� ���� ����
                    if(!BackendManager.Instance.userGradeList.Contains(userData.Value))
                        BackendManager.Instance.userGradeList.Add(userData.Value);
                    
                    Debug.Log(BackendManager.Instance.userGradeList.Count + "����Ʈ ũ��Ȯ��");
                }
            }
            
            // �¸� ���� üũ
            CheckAllUsersHP();
            
            // ��Ƴ��� ������ �Ѹ��� ��� ����ִ� ������ ���ó�� ����Ʈ�� �ְ� ���۰��̸ӿ��� ���� ���� ��û
            //if (BackendManager.Instance.userDataList.Count <= 1)
            //{
            //    BackendManager.Instance.winUser = BackendManager.Instance.userDataList[0].playerName; 
            //    Debug.Log("�ٸ������� ��� ������ �¸�ó��");
            //    foreach (var userData in BackendManager.Instance.inGameUserList)
            //    {
            //        if (userData.Value.m_nickname == BackendManager.Instance.userInfo.Nickname)
            //        {
            //            BackendManager.Instance.userGradeList.Add(userData.Value);
            //            BackendManager.Instance.winUser = BackendManager.Instance.userInfo.Nickname;
            //        }
            //    }
            //    
            //    BackendManager.Instance.SendResultToServer();
            //    SendToSuperGamerEndGame();
            //}
        };
        
        Backend.Match.OnChangeSuperGamer = (MatchInGameChangeSuperGamerEventArgs args) => {
            
            Debug.Log($"���� ���̸ӿ��� {args.OldSuperUserRecord}���� ������ �����Ͽ� ���۰��̸Ӹ� �缱���մϴ�");
            
            for (int i = 0; i < BackendManager.Instance.userDataList.Count; i++)
            {
                if (BackendManager.Instance.userDataList[i].playerName == args.NewSuperUserRecord.m_nickname)
                {
                    Debug.Log("���ο� ���۰��̸Ӵ�" + BackendManager.Instance.userDataList[i].playerName + "�� �Դϴ�");
        
                    BackendManager.Instance.userDataList[i].isSuperGamer = true;

                    SuperGamerIdx = i;
                    
                    if (BackendManager.Instance.userDataList[i].playerName == BackendManager.Instance.userInfo.Nickname)
                    {
                        BackendManager.Instance.isMeSuperGamer = true;
                    }
                    else
                    {
                        BackendManager.Instance.isMeSuperGamer = false;
                    }
                }
                else
                {
                    BackendManager.Instance.userDataList[i].isSuperGamer = false;
                }
            }
            
            UIManager.Instance.OpenIndicator();
        };
        
        // ���� ������ ���� ���� �������� ȣ��
        Backend.Match.OnLeaveInGameServer = (MatchInGameSessionEventArgs args) => {
            if (args.ErrInfo == ErrorCode.Success) {
                Debug.Log("OnLeaveInGameServer �ΰ��� ���� ���� ���� : " + args.ErrInfo.ToString());
            } else {
                Debug.LogError("OnLeaveInGameServer �ΰ��� ���� ���� ���� : " + args.ErrInfo + " / " + args.Reason);
            }
        };
        
        // ������ ����� ���� �Ϸ��Ͽ� ������ ����ó��
        Backend.Match.OnMatchResult = (MatchResultEventArgs args) =>
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("8-2. OnMatchResult ���� : " + args.ErrInfo.ToString());
                GameResult(BackendManager.Instance.winUser == BackendManager.Instance.userInfo.Nickname);
            }
            else
            {
                Debug.LogError($"8-2. OnMatchResult ����, �ڵ� : {args.ErrInfo} ���� : {args.Reason}");
                
                foreach (var userData in BackendManager.Instance.inGameUserList)
                {
                    Debug.Log("�������� ������ : " + userData.Value.m_nickname);
                }

                foreach (var grade in BackendManager.Instance.userGradeList)
                {
                    Debug.Log("�Էµ� ��� ������ : " + grade.m_nickname);   
                }
            }
        };
    }
    public void CheckAllUsersHP()
    {
        int remainPlayerCount = 0;

        string remainPlayerNickname = "";
        
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].hp >= 1)
            {
                remainPlayerNickname = players[i].PlayerName.text;
                
                remainPlayerCount++;
            }
        }

        if (remainPlayerCount <= 1)
        {
            Debug.Log("���� ������ �Ѹ��̶� ���� ó��");
            
            BackendManager.Instance.winUser = remainPlayerNickname;

            // �ٳ����� ������ ���� ������ ���۰��̸Ӷ�� ������ ��� �ٷ� ����
            if (BackendManager.Instance.isMeSuperGamer)
            {
                foreach (var userData in BackendManager.Instance.inGameUserList)
                {
                    // �¸������� ������ ���� ������ �г����� ������ üũ
                    if (userData.Value.m_nickname == BackendManager.Instance.winUser)
                    {
                        // �������� �й踮��Ʈ�� ����
                        if(!BackendManager.Instance.userGradeList.Contains(userData.Value))
                            BackendManager.Instance.userGradeList.Add(userData.Value);
                        
                        Debug.Log(BackendManager.Instance.userGradeList.Count + "������ ���������� ������ �й�ó�� ����Ʈũ��");
                    }
                }
                
                BackendManager.Instance.SendResultToServer();
            }
            else
            {
                // ���۰��̸Ӱ� �������� ���¿� �¸�ó��
                SendToSuperGamerEndGame();
            }
        }
    }
    
    public void CreateMap()
    {
        // ���� ���۽� ���۰��̸Ӵ� ���� �����϶�� �޼����� ����
        if (BackendManager.Instance.isMeSuperGamer)
        {
            messageQueue = new Queue<Message>();
            
            Message m = new Message();
            m.playerIdx = -10;
            m.cardIdx = Random.Range(0, 100);
            SendData(m);
            CardManager.Instance.seed = m.cardIdx;
            int head = BackendManager.Instance.userDataList.Count;
            switch (head)
            {
                case 1:
                    gameBoard.Generate(m.cardIdx, 12, 12, 2);
                    break;
                case 2:
                    gameBoard.Generate(m.cardIdx, 12, 12, 2);
                    break;
                case 3:
                case 4:
                    gameBoard.Generate(m.cardIdx, 14, 14, 4);
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                    gameBoard.Generate(m.cardIdx, 16, 16, 8);
                    break;
                default:
                    UIManager.Instance.OpenRecyclePopup("�ý��� ����", "�� ���� ����", Application.Quit);
                    break;
            }
        }
    }
    
    public void SendToSuperGamerEndGame()
    {
        Debug.Log("�¸�ó�� �޼��� ���� ���̸ӿ��� �۽�");
        Message m = new Message();
        m.playerIdx = -99;
        m.cardIdx = -99;
        SendData(m);
    }

    public void newSuperGamerMessageQueueInit()
    {
        messageQueue = new Queue<Message>();
    }
    
    void Update()
    {
        if (BackendManager.Instance != null)
        {
            if (BackendManager.Instance.isMeSuperGamer)
            {
                if (messageQueue == null)
                {
                    return;
                }

                if (messageQueue.Count > 0)
                {
                    Message m = messageQueue.Dequeue();
                    if (m == null)
                    {
                        Debug.LogError("Dequeued message is null.");
                        return;
                    }
                    SendData(m);
                    Debug.Log($"Processing message for playerIdx: {m.playerIdx}, cardIdx: {m.cardIdx}");

                    CardRealGo(m.playerIdx, m.cardIdx);
                }
            }
        }
    }
    
    //public void CardData(int cardIdx,int playerIdx )
    //{
    //    CardMessage msg;
    //    msg = new CardMessage(cardIdx, playerIdx);
    //    if (BackEndMatchManager.GetInstance().IsHost())
    //    {
    //        BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
    //    }
    //    else
    //    {
    //        BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
    //    }
    //}

    [Serializable]
    public class Message
    {
        public int playerIdx;
        public int cardIdx;
    }


    public void SendData(Message mes)
    {
        var jsonData = JsonUtility.ToJson(mes); // Ŭ������ json���� ��ȯ���ִ� �Լ�
        var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonData); // json�� byte[]�� ��ȯ���ִ� �Լ�

        Backend.Match.SendDataToInGameRoom(dataByte);
    }


    public void CardGo(int playerIdx, int cardIdx) //ī�� ���, ������ ��� �ؾߵ�
    {
        if (BackendManager.Instance.isMeSuperGamer)
        {
            Message mes = new Message();
            mes.playerIdx = playerIdx;
            mes.cardIdx = cardIdx;
            messageQueue.Enqueue(mes);
        }
        else
        {
            Message mes = new Message();
            mes.playerIdx = playerIdx;
            mes.cardIdx = cardIdx;
            SendData(mes);
        }
    }

    public void CardRealGo(int playerIdx, int cardIdx)
    {
        if (playerIdx == 9)
        {
            CardManager.Instance.OnCardStart(chaser.transform, cardIdx);
        }
        else
        {
            //if (players.Count <= 1) return;
            
            CardManager.Instance.OnCardStart(players[playerIdx].transform, cardIdx);
        }
    }

    public void GoDamage(Vector2Int pos, int damage)
    {
        if (!BackendManager.Instance.isInitialize) return;
        
        for (int i = 0; i < players.Count; i++)
        {
            Vector2Int vec = new Vector2Int((int)players[i].transform.position.x, (int)players[i].transform.position.y);
            if (vec == pos)
            {
                if (players[i].HP > 0)
                {
                    players[i].HP -= damage;
                }
            }
        }

        CheckAllUsersHP();
    }

    public void GameResult(bool isWin)
    {
        if (isWin)
        {
            string allKillWin = "�¸� �Ͽ����ϴ�!";

            string esacpeWin = "Ż�� ����!";

            mainUi.winTypeText.text = BackendManager.Instance.isEscapeWin ? esacpeWin : allKillWin;
        
            mainUi.winImage.SetActive(true);
            mainUi.gameOver.SetActive(false);
        }
        else
        {
            if(Camera.main.TryGetComponent<CameraManager_HJH>(out var cam))
            {
                cam.target = null;
            }
            else
            {
                Camera.main.transform.SetParent(null);
            }
            players[myIdx].gameObject.SetActive(false);
            
            mainUi.gameOver.SetActive(true);
            mainUi.winImage.SetActive(false);
            
            mainUi.lookAroundBtn.interactable = BackendManager.Instance.winUser == "";
        }
    }

    //������ �ڵ� ���ܵ�
    // Backend.Match.IsGameRoomActivate( callback =>
    // {
    //     switch (callback.GetStatusCode())
    //     {
    //         case "200" :
    //             Debug.Log("�濡 �������� �����մϴ�.");
    //             var roomInfo = callback.GetReturnValuetoJSON();
    //             var addr = roomInfo["serverPublicHostName"].ToString();
    //             var port = Convert.ToUInt16(roomInfo["serverPort"].ToString());
    //             ErrorInfo errorInfo = null;
    //             
    //             if(Backend.Match.JoinGameServer(addr, port, true, out errorInfo) == false)
    //             {
    //                 // ���� Ȯ��
    //                 Debug.Log("������ �õ��մϴ�.");
    //                 return;
    //             }
    //             break;
    //         case "404" :
    //             Debug.Log("���� ������ ������ ���� �����ϴ�.");
    //             UIManager.Instance.OpenRecyclePopup("�ȳ�", "�������� �Ұ����մϴ� Ÿ��Ʋ�� �̵��մϴ�.", GoTitle);
    //             break;
    //     }
    // });
}
