using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_HJH : MonoBehaviour
{
    public bool Attack = false;
    public int Damage = 10;
    public float skillSpeed = 2f;
    bool check = true;
    GameObject usePlayer;
    // Start is called before the first frame update
    void Start()
    {
        usePlayer = GameObject.Find("Aland(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if(this.name.Contains("Player1SkillEffect") && check == true)
        {
            if(usePlayer.transform.rotation.y > 0)
            {
                StartCoroutine(Right());
                check = false;
            }
            else
            {
                StartCoroutine(Left());
                check = false;
            }
        }
    }
    IEnumerator Right()
    {
        while (true)
        {
            transform.position += Vector3.right * Time.deltaTime * skillSpeed;
            gameObject.GetComponent<Renderer>().sortingOrder = 50;
            yield return null;
        }
        
    }
    IEnumerator Left()
    {
        while (true)
        {
            transform.position += Vector3.left * Time.deltaTime * skillSpeed;
            gameObject.GetComponent<Renderer>().sortingOrder = 50;
            yield return null;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(Attack == true)
        {
            if (other.GetComponent<PlayerHp_HJH>() != null && other.GetComponent<PlayerWarrior_HJH>() == null)
            {
                other.GetComponent<PlayerHp_HJH>().Damage(other.transform.position - transform.position,Damage);
                Attack = false;
            }
        }
    }
}
