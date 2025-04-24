using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedButton : MonoBehaviour,IInteract
{

    public GameObject[] barrels;
    bool active=true;
    
    public void Interact()
    {
        if (active) 
        {
            active = false;
            int type = Random.RandomRange(0, barrels.Length);
            GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1f);
            for (int i = 0; i < 4; i++)
            {
                Instantiate(barrels[type], new Vector3(25.11f, 15.124f, -22.61f), Quaternion.identity);
            }
        }
    }


}
