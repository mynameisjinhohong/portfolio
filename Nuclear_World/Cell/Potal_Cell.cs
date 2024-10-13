using System.Collections;
using UnityEngine;

namespace CodingSystem_HJH
{


    public class Potal_Cell : Cell
    {
        public PotalSet_cell potalControl;
        public AudioSource potalAudio;
        public Animator potalAni;
        public override bool CanMove(int x, int y, Players player)
        {
            if (onPlayer == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnPlayer(Players player, int beforeX, int beforeY)
        {
            base.OnPlayer(player, beforeX, beforeY);
            if (potalControl.Active && ((Mathf.Abs(beforeY - posy) == 1&& beforeX-posx ==0) || (Mathf.Abs(beforeX - posx) == 1&& beforeY - posy == 0))) //포탈이 작동하고, 바로 옆의 좌표에서 왔을 때
            {
                if (potalControl.potal1 == this)
                {
                    Potal_Cell another = potalControl.potal2;
                    if ((GridManager.Instance.CanMoveCheck(another.posx, another.posy, player)))
                    {
                        StartCoroutine(MovePlayer(true, player));
                        potalAni.SetTrigger("PotalStart");
                        potalAudio.Play();
                    }
                }
                else
                {
                    Potal_Cell another = potalControl.potal1;
                    if ((GridManager.Instance.CanMoveCheck(another.posx, another.posy, player)))
                    {
                        StartCoroutine(MovePlayer(false, player));
                        potalAni.SetTrigger("PotalStart");
                        potalAudio.Play();
                    }
                }
            }
        }

        public IEnumerator MovePlayer(bool potal1,Players player)
        {
            TurnManager.Instance.turnStop = true;
            if (potal1)
            {
                Potal_Cell another = potalControl.potal2;
                yield return new WaitForSeconds(0.75f);
                player.transform.position = another.transform.position;
                player.GetComponent<Players>().PlayerPos = new Vector2Int(another.posx, another.posy);
            }
            else
            {
                Potal_Cell another = potalControl.potal1;
                yield return new WaitForSeconds(0.75f);
                player.transform.position = another.transform.position;
                player.GetComponent<Players>().PlayerPos = new Vector2Int(another.posx, another.posy);
            }
            TurnManager.Instance.turnStop = false;
        }
    }
}