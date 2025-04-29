using UnityEngine;

public class BuyButton : MonoBehaviour, IInteract 
{
    public PanelMenu panel;
    public TriggerHUBDoor cargoSystem; // Ссылка на систему груза

    public void Interact() 
    {
        if (panel.page == 0) 
        {
            if (PanelMenu.credits >= 100 && Sheild.strenght < 22) 
            {
                PanelMenu.credits -= 100;
                Sheild.strenght++;
                PlayerPrefs.SetInt("Sheild", Sheild.strenght);
                PlayerPrefs.Save();
                Debug.Log($"Улучшен щит. Текущий уровень: {Sheild.strenght}");
            }
        } 
        else if (panel.page == 1) 
        {
            if (PanelMenu.credits >= 500 && PanelMenu.maxCargo < 10) 
            {
                PanelMenu.credits -= 500;
                PanelMenu.maxCargo++;
                PlayerPrefs.SetInt("gruz", PanelMenu.maxCargo);
                PlayerPrefs.Save();
                
                // Немедленное обновление интерфейса
                if (cargoSystem != null) 
                {
                    cargoSystem.UpdateCounter();
                }
                Debug.Log($"Улучшен грузовой отсек. Новый лимит: {PanelMenu.maxCargo}");
            }
        }
    }
}