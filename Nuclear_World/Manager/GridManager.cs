using UnityEngine;

namespace CodingSystem_HJH
{


    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

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
        public Players[] players;
        [SerializeField]
        public Map maps;

        public float moveSecond = 1;

        //턴 관련

        private void Start()
        {
            players = FindObjectsOfType<Players>();
            for (int i = 0; i < maps.GridSize.x; i++)
            {
                for (int j = 0; j < maps.GridSize.y; j++)
                {
                    if (maps.GetCell(i, j) != null)
                    {
                        maps.GetCell(i, j).posx = i;
                        maps.GetCell(i, j).posy = j;
                    }
                }
            }
        }

        private void Update()
        {
            if (TurnManager.Instance.codeStart == true)
            {
                if (players[0].canCoding)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].canCoding = false;
                    }
                }
            }

        }



        public bool CanMoveCheck(int x, int y, Players player) //myidx 번의 단말이 x,y로 이동 가능한지
        {
            if(x < 0 || x >= maps.GridSize.x || y < 0 || y >= maps.GridSize.y)
            {
                return false;
            }
            if (maps.GetCell(x, y) == null)
            {
                return false;
            }
            bool dap = maps.GetCell(x, y).CanMove(x, y, player);
            if (player.canMove == false)
            {
                return false;
            }
            #region 과거의 코드
            //switch (map.GetCell(x, y))
            //{
            //    case MapObjectType.none:
            //        dap = true;
            //        break;
            //    case MapObjectType.wall:
            //        dap = false;
            //        break;
            //}
            //for (int i =0; i<players.Length; i++)
            //{
            //    if(i == myIdx)
            //    {
            //        continue;
            //    }
            //    if (players[i].playerPos.x == x && players[i].posY == y)
            //    {
            //        dap = false; 
            //        break;
            //    }
            //}
            #endregion
            return dap;
        }

        public void MoveCell(int beforeX, int beforeY, int x, int y, Players player)
        {
            if (maps.GetCell(x, y) == null)
            {
                Debug.Log("이동할 수 없는 위치입니다.");
                return;
            }
            Cell cell = maps.GetCell(x, y);
            if (maps.GetCell(beforeX, beforeY) != null)
            {
                Cell beforeCell = maps.GetCell(beforeX, beforeY);
                beforeCell.onPlayer = null;
            }
            cell.OnPlayer(player, beforeX, beforeY);
        }

        public void StopAll()
        {
            for (int i = 0; i < maps.GridSize.x; i++)
            {
                for (int j = 0; j < maps.GridSize.y; j++)
                {
                    if (maps.GetCell(i, j) != null)
                    {
                        maps.GetCell(i, j).StopAll();
                    }
                }
            }
            for (int i = 0; i < players.Length; i++)
            {
                players[i].canCoding = true;
            }
        }
    }

}