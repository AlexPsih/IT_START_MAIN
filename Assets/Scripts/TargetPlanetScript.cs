using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetPlanetScript : MonoBehaviour
{
    public int range;
    int planetid;
    public GameObject[] planet;
    void Start()
    {
        transform.position = new Vector3(Random.RandomRange(-range, range), Random.RandomRange(-range, range), Random.RandomRange(-range, range) * 0f);
        planet[Random.RandomRange(0,planet.Length)].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name=="player")
        {
            SceneManager.LoadScene(2);
        }
    }
}
