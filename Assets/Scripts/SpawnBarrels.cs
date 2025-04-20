using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBarrels : MonoBehaviour
{
    public GameObject[] barrels;
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(barrels[PlayerPrefs.GetInt("mission", 0)], new Vector3(25.11f, 15.124f, -22.61f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
