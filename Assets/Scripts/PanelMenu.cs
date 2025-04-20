using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelMenu : MonoBehaviour
{
    public int page;
    public TextMeshPro named;
    public TextMeshPro howmany;
    public TextMeshPro price;
    void Start()
    {
        price;
    }

    
    void Update()
    {
        if (page==0 && PlayerPrefs.GetInt("Sheild", 1) < 10)
        {
            named.text = "Щит";
            howmany.text = PlayerPrefs.GetInt("Sheild",0).ToString()+ "/7";
            price.text = "1000$";
        }
        else if (page == 1 && PlayerPrefs.GetInt("manevr", 1) < 5)
        {
            named.text = "Манёвривость";
            howmany.text = (PlayerPrefs.GetInt("manevr", 50)/50).ToString() + "/5";
            price.text = "500$";
        }
        else if (page == 2 && PlayerPrefs.GetInt("gruz", 1)<5)
        {
            named.text = "Грузовой отсек";
            howmany.text = PlayerPrefs.GetInt("gruz", 1).ToString() + "/5";
            price.text = "1500$";
        }
    }
}
