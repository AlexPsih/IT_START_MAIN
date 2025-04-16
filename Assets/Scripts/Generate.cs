using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public int range;
    public int counts=50;
    public GameObject[] planets;
    void Start()
    {
        for (int i=0; i< counts; i++)
        {
            Instantiate(planets[Random.RandomRange(0,planets.Length)], new Vector3(Random.RandomRange(-range, range), Random.RandomRange(-range, range), Random.RandomRange(-range, range)*0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
