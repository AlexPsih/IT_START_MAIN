using UnityEngine;

public class MenuInteract : MonoBehaviour, IInteract 
{
    public PanelMenu panel;
    public int maxPage = 1;
    public int minPage = 0;

    public void Interact() 
    {
        if (gameObject.name == "arrowR" && panel.page < maxPage) 
        {
            panel.page++;
            Debug.Log($"Переключено на страницу {panel.page}");
        }
        else if (gameObject.name == "arrowL" && panel.page > minPage) 
        {
            panel.page--;
            Debug.Log($"Переключено на страницу {panel.page}");
        }
    }
}