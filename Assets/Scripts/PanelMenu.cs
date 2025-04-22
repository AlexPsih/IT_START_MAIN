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
        
    }

    
    void Update()
    {
        if (page==0)
        {
            named.text = "Щит";
            howmany.text = PlayerPrefs.GetInt("Sheild",-1).ToString()+ "/7";
            price.text = "100$";
        }
        else if (page == 1)
        {
            named.text = "Манёвривость";
            howmany.text = (PlayerPrefs.GetInt("manevr", -1)/50).ToString() + "/7";
            price.text = "1000$";
        }
        else if (page == 2)
        {
            named.text = "Грузовой отсек";
            howmany.text = PlayerPrefs.GetInt("gruz", -1).ToString() + "/7";
            price.text = "300$";
        }
    }
}
