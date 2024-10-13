using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;
using UnityEngine.SceneManagement;
using System;

namespace CodingSystem_HJH
{

    public class DataManager_DontDestroy : Singleton<DataManager_DontDestroy>
    {
        public List<InventoryItem> inventoryItems = new List<InventoryItem>();
        public string playerID = "Player1";
        public Vector3 playerPos = Vector3.zero;
        public int currentDoorID = 0;
        public bool hackSucess;
        public bool[] doorOpenState;
        
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += AddItemtoInventory;
            SceneManager.sceneLoaded += DoorHackedChange;
            SceneManager.sceneLoaded += DoorStateLoad;
        }

        private void AddItemtoInventory(Scene scene, LoadSceneMode loadScene)
        {
            if (scene.name == "GameScene")
            {
                if(inventoryItems.Count > 0)
                {
                    //여기다가 inventory에 아이템 추가하는 코드 넣으면 됨
                    foreach (InventoryItem item in inventoryItems)
                    {
                        MMInventoryEvent.Trigger(MMInventoryEventType.Pick, null, item.TargetInventoryName, item, item.Quantity, 0, playerID);
                    }
                    inventoryItems.Clear();
                }
            }
        }

        private void DoorHackedChange(Scene scene, LoadSceneMode loadScene)
        {
            if(scene.name == "GameScene" && hackSucess)
            {
                Door[] doors = FindObjectsOfType<Door>();
                foreach (Door door in doors)
                {
                    if(door.doorID == currentDoorID)
                    {
                        door.isHacked= true;
                    }
                }
                hackSucess = false;
            }
        }
        public void DoorStateSave()
        {
            Door[] doors = FindObjectsOfType<Door>();
            doorOpenState = new bool[doors.Length];
            for (int i = 0; i < doors.Length; i++)
            {
                doorOpenState[doors[i].doorID] = doors[i].isOpen;
            }
        }

        private void DoorStateLoad(Scene scene, LoadSceneMode loadScene)
        {
            if (scene.name == "GameScene" && doorOpenState.Length > 0)
            {
                Door[] doors = FindObjectsOfType<Door>();
                for (int i = 0; i < doors.Length; i++)
                {
                    doors[i].StartDoor(doorOpenState[doors[i].doorID]);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
