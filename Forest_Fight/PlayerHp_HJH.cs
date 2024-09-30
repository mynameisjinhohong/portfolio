using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHp_HJH : MonoBehaviourPun
{
    ImpactReceiver_HJH impact;
    [SerializeField]
    public int hp = 0;
    PlayerMove_HJH pm;
    CameraShaker_HJH cs;
    public GameObject effect;
    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            pm.state = PlayerMove_HJH.State.Attacked;
            hp = value;
        }
    }

    public void Damage(Vector3 point,int power)
    {
        string thing = point.x.ToString() + " " + point.y.ToString() + " " + point.z.ToString();
        photonView.RPC("Dam", RpcTarget.All, thing, power);

    }
    [PunRPC]
    void Dam(string thing, int power)
    {

            string[] th = thing.Split();
            Vector3 point = new Vector3(float.Parse(th[0]), float.Parse(th[1]), float.Parse(th[2]));
            if (point.x > 0)
            {
                pm.moveVec = Vector3.zero;
                impact.AddImpact(new Vector3(1, 1, 0), ((hp / 30) + 1) * 50);
            }
            else
            {
                pm.moveVec = Vector3.zero;
                impact.AddImpact(new Vector3(-1, 1, 0), ((hp / 30) + 1) * 50);
            }
            cs.Shake((float)hp / 100, 0.5f);
            GameObject ef = Instantiate(effect);
            ef.transform.position = transform.position + new Vector3(0, 1, 0);
            ef.GetComponent<Renderer>().sortingOrder = 50;
            Hp += power;
        
    }
    private void Awake()
    {
        impact = GetComponent<ImpactReceiver_HJH>();
        if (gameObject.name.Contains("Aland"))
        {
            pm = GetComponent<PlayerWarrior_HJH>();
        }
        else if (gameObject.name.Contains("Warrior"))
        {
            pm = GetComponent<PlayerDwarf_HJH>();
        }
        else if (gameObject.name.Contains("Alice"))
        {
            pm = GetComponent<PlayerAlice_LHS>();
        }
        else if (gameObject.name.Contains("Archer"))
        {
            pm = GetComponent<PlayerArcher_LHS>();
        }
        cs = GetComponent<CameraShaker_HJH>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
