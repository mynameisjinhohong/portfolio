using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_HJH : Object_Manager_shj
{
    //public Animator rockAni;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //public void RockTouch()
    //{
    //    rockAni.SetTrigger("Touch");

    //}

    //public void RockOff()
    //{
    //    gameObject.SetActive(false);
    //}
    public override void Obstacle_Active(GameObject player)
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager_HJH>().RockBreakSoundPlay();
    }

}
