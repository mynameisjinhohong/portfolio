using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.InventoryEngine;
using UnityEngine.UI;
using SurviveCoding.External.InventoryEngine;
namespace CodingSystem_HJH
{
    [System.Serializable]
    public struct Items
    {
        public InventoryItem inventoryItems;
        public int count;
    }

    public class GameClearUI : MonoBehaviour
    {
        [System.Serializable]
        public class InventoryItems
        {
            public Items[] items;
        }
        [SerializeField]
        public InventoryItems[] rewardItem;
        public Image[] rewardItemImages;
        public GameObject[] gameTab;
        public int[] turnCheck;
        public GameObject[] stars;
        public TMP_Text turnText;
        public TMP_Text playCountText;
        public TMP_Text bestPlayCountText;
        public int nowTab;
        public AudioSource clearAudio;
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < gameTab.Length; i++)
            {
                gameTab[i].SetActive(false);
            }
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GameClear()
        {
            clearAudio.Play();
            for (int i = 0; i < gameTab.Length; i++)
            {
                gameTab[i].SetActive(false);
            }
            gameTab[0].SetActive(true);
            nowTab = 0;
            List<Items> items = new List<Items>();
            for (int i = 0; i < turnCheck.Length; i++)
            {
                if (TurnManager.Instance.turn <= turnCheck[i])
                {
                    stars[i].SetActive(true);
                    if (DataManager_DontDestroy.Instance != null)
                    {
                        for(int j = 0; j < rewardItem[i].items.Length; j++)
                        {
                            items.Add(rewardItem[i].items[j]);
                            for(int k = 0; k < rewardItem[i].items[j].count; k++)
                            {
                                DataManager_DontDestroy.Instance.inventoryItems.Add(rewardItem[i].items[j].inventoryItems) ;
                            }
                        }
                    }
                }
            }
            for(int i = 0; i < rewardItemImages.Length; i++)
            {
                if (i < items.Count)
                {
                    rewardItemImages[i].gameObject.SetActive(true);
                    rewardItemImages[i].sprite = items[i].inventoryItems.Icon;
                    rewardItemImages[i].transform.parent.parent.GetChild(1).gameObject.SetActive(true);
                    rewardItemImages[i].transform.parent.parent.GetChild(1).GetComponent<TMP_Text>().text = items[i].count.ToString();
                }
                else
                {
                    rewardItemImages[i].transform.parent.parent.GetChild(1).gameObject.SetActive(false);
                    rewardItemImages[i].gameObject.SetActive(false);
                }
            }
            turnText.text = "소요 턴: " + TurnManager.Instance.turn;
            playCountText.text = "시도 횟수: " + GamePlayManager.Instance.playCount;
            bestPlayCountText.text = "최고 기록: " + PlayerPrefs.GetInt("Stage" + GamePlayManager.Instance.stageNum);
        }
        public void StartTab()
        {
            for (int i = 0; i < gameTab.Length; i++)
            {
                gameTab[i].SetActive(false);
            }
            nowTab = 0;
            gameTab[nowTab].SetActive(true);
        }

        public void NextTab()
        {
            for (int i = 0; i < gameTab.Length; i++)
            {
                gameTab[i].SetActive(false);
            }
            nowTab++;
            gameTab[nowTab].SetActive(true);
        }

        public void EndTab()
        {
            for(int i = 0; i < gameTab.Length; i++)
            {
                gameTab[i].SetActive(false);
            }
            nowTab = gameTab.Length - 1;
            gameTab[nowTab].SetActive(true);
        }
        public void RestartScene()
        {
            LoadingManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void HackSucess()
        {
            if(DataManager_DontDestroy.Instance != null)
            {
                DataManager_DontDestroy.Instance.hackSucess = true;
            }
        }
        public void GoToGameScene()
        {
            LoadingManager.LoadScene("GameScene");
            if (DataManager_DontDestroy.Instance != null)
            {
                var display = new ItemDisplayTest();
                display.RD();
            }
        }
    }
}