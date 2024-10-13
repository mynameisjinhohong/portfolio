using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodingSystem_HJH
{


    public class Cell : MonoBehaviour
    {
        public Players onPlayer; //�� Ÿ�� ���� �ܸ��� �ִ��� ������
        public KeyObject keyObj; //�� Ÿ�� ���� Ű������Ʈ�� �ִ��� ������
        public int posx, posy;
        public virtual bool CanMove(int x, int y, Players player)
        {

            return false;
        }
        public virtual void OnPlayer(Players player, int beforeX, int beforeY)//��Ż�� ���ؼ� ���� �÷��̾� ��ġ�� ����
        {
            onPlayer = player;
        }
        //���� ���߱� �������� �ʱ�ȭ �Ǵ� �͵� �����ֱ�
        public virtual void StopAll()
        {

        }
    }
}