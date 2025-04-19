using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerHUBDoor : MonoBehaviour
{
    public bool Sklad;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (Sklad)
        {
            if (other.name== "barrel_3_2")
            {
                PlayerPrefs.SetInt("mission",0);
                print("Миссия на время");
            }
            if (other.name == "barrel_3_1")
            {
                PlayerPrefs.SetInt("mission", 1);
                print("Миссия с пиратами");
            }
            if (other.name == "barrel_1")
            {
                PlayerPrefs.SetInt("mission", 2);
            }
            if (other.name == "barrel_2")
            {
                PlayerPrefs.SetInt("mission", 3);
            }
        }
        if (!Sklad && other.name == "FirstPersonController")
        {
            SceneManager.LoadScene(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
