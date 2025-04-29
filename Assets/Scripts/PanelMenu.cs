using UnityEngine;
using TMPro;

public class PanelMenu : MonoBehaviour 
{
    public int page;
    public TextMeshPro named;
    public TextMeshPro howmany;
    public TextMeshPro price;
    public static int credits = 500;
    public static int maxCargo;

    void Start()
    {
        page = Mathf.Clamp(page, 0, 1);

        // Жесткий сброс значений при старте
        maxCargo = 2;
        Sheild.strenght = 15;
        PlayerPrefs.SetInt("gruz", maxCargo);
        PlayerPrefs.SetInt("Sheild", Sheild.strenght);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (page == 0) 
        {
            named.text = "Щит";
            howmany.text = $"{Sheild.strenght} / 22";
            price.text = "100CR";
        } 
        else if (page == 1) 
        {
            named.text = "Грузовой отсек";
            howmany.text = $"{maxCargo} / 10";
            price.text = "500CR";
        }
    }
}
