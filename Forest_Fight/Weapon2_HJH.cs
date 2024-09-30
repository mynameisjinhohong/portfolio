using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2_HJH : MonoBehaviour
{
    public bool Attack = false;
    public int Damage = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name.Contains("SkillEffect"))
        {
            Destroy(gameObject, 1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Attack == true)
        {
            if (other.GetComponent<PlayerHp_HJH>() != null && other.GetComponent<PlayerDwarf_HJH>() == null)
            {
                other.GetComponent<PlayerHp_HJH>().Damage(other.transform.position - transform.position, Damage);
                Attack = false;
            }
        }
    }
}
