using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TriggerHUBDoor : MonoBehaviour
{
    public bool Sklad;
    public int mesto;
    public int mestomax=1;
    public TextMeshPro mestotext;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (Sklad && mesto<mestomax)
        {
            mesto += 1;
            if (other.name== "barrel_3_2(Clone)")
            {
                PlayerPrefs.SetInt("mission",0);
                print("Миссия на время");
                GameObject.Destroy(other.gameObject);
            }
            if (other.name == "barrel_3_1(Clone)")
            {
                PlayerPrefs.SetInt("mission", 1);
                print("Миссия с пиратами");
                GameObject.Destroy(other.gameObject);
            }
            if (other.name == "barrel_1(Clone)")
            {
                PlayerPrefs.SetInt("mission", 2);
                GameObject.Destroy(other.gameObject);
            }
            if (other.name == "barrel_2(Clone)")
            {
                PlayerPrefs.SetInt("mission", 3);
                GameObject.Destroy(other.gameObject);
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
        mestomax = PlayerPrefs.GetInt("gruz",2); 

        mestotext.text =mesto.ToString()+"/"+mestomax.ToString();
    }
}
