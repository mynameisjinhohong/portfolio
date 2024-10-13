using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CodingSystem_HJH
{


    public class Change_Cell : Cell
    {
        public ChangeControl_Cell changeControlCell;
        private KeyType keyType;
        public AudioSource changeAudio;
        public KeyType KeyType
        {
            get
            {
                return keyType;
            }
            set
            {
                if (value != keyType)
                {
                    keyType = value;
                }

            }
        }


        public override bool CanMove(int x, int y, Players player)
        {
            if(onPlayer == null)
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
            if (player.GetComponent<Players>().keyObj != null)
            {
                player.GetComponent<Players>().keyObj.keyType = keyType;
                changeAudio.Play();
            }
        }
    }
}