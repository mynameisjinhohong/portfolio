using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Item,
    Obstacle,
}

public class Object_Manager_shj : MonoBehaviour
{
    protected Animator animator;
    public Type type;

    bool touch = false;
    protected bool ItmeInActive = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Item_Active(GameObject player) { }
    
    public virtual void Obstacle_Active(GameObject player) { }

    public void Active(GameObject player)
    {
        if(type == Type.Item) //������ �϶�
        {
            Item_Active(player);
            if (ItmeInActive)
            {
                InActive();

            }
        }
        else if(type == Type.Obstacle)
        {
            Obstacle_Active(player);
            if(animator != null) animator.SetTrigger("Touch");
            if (player.GetComponent<Player_shj>().state != Player_State.Rolling) player.GetComponent<Player_shj>().hp--;
        }
    }

    public void InActive() { gameObject.SetActive(false); }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player" && collision.GetComponent<Player_shj>().enabled ) && !touch)
        {
            Active(collision.gameObject);
            touch = true;
        }
    }
}
