using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleWork_HJH : MonoBehaviour
{
    Renderer color;
    // Start is called before the first frame update
    void Start()
    {
        color = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("People"))
        {
            color.material.color = Color.magenta;
        }
    }
}