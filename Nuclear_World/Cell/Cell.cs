using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingSystem_HJH
{


    public class Cell : MonoBehaviour
    {
        public Players onPlayer; //이 타일 위에 단말이 있는지 없는지
        public KeyObject keyObj; //이 타일 위에 키오브젝트가 있는지 없는지
        public int posx, posy;
        public virtual bool CanMove(int x, int y, Players player)
        {

            return false;
        }
        public virtual void OnPlayer(Players player, int beforeX, int beforeY)//포탈을 위해서 이전 플레이어 위치를 받음
        {
            onPlayer = player;
        }
        //게임 멈추기 눌렀을때 초기화 되는 것들 적어주기
        public virtual void StopAll()
        {

        }
    }
}