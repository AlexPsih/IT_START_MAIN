using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{
    public GameObject exploison;
    public int strenght=15;
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if ("SP_Rock06(Clone)" == other.name)
        {
            Instantiate(exploison, other.gameObject.transform.position, Quaternion.identity);
            GameObject.Destroy(other.gameObject);
            strenght -= 1;
            
        }
            
        
    }

    private void Update()
    {
        if (strenght<0)
        {
            GameObject.Destroy(gameObject);
        }
        
    }
}
