using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour, IInteract
{
    public PanelMenu panel;
    public void Interact()
    {
        if (panel.page == 0)
        {
            PlayerPrefs.SetInt("Sheild", PlayerPrefs.GetInt("Sheild", 0)+1);
        }
        else if (panel.page == 1)
        {
            PlayerPrefs.SetInt("manevr", PlayerPrefs.GetInt("manevr", 50) + 35);
        }
        else if (panel.page == 2)
        {
            PlayerPrefs.SetInt("gruz", PlayerPrefs.GetInt("gruz", 2) + 1);
        }
    }
}
