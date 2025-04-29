using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; // Для работы с корутинами

public class TriggerHUBDoor : MonoBehaviour
{
    [Header("Настройки")]
    public bool isWarehouse;

    [Header("Счетчик")]
    public TextMeshPro cargoCounterText;

    public int CurrentCargo => PlayerPrefs.GetInt("currentCargo", 0);
    public bool HasCargo => CurrentCargo > 0;

    void Start()
    {
        // Жесткий сброс значений при старте
        PanelMenu.maxCargo = 2;
        Sheild.strenght = 15;
        PlayerPrefs.SetInt("gruz", PanelMenu.maxCargo);
        PlayerPrefs.SetInt("Sheild", Sheild.strenght);
        PlayerPrefs.SetInt("currentCargo", 0);
        PlayerPrefs.Save();

        UpdateCounter();
    }

    public void UpdateCounter()
    {
        if (cargoCounterText != null)
        {
            cargoCounterText.text = $"{CurrentCargo}/{PanelMenu.maxCargo}";
            cargoCounterText.color = HasCargo ? Color.green : Color.red;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isWarehouse && other.CompareTag("Cargo"))
        {
            AddCargo(other.gameObject);
        }
        else if (!isWarehouse && other.CompareTag("Player"))
        {
            TryLoadPlanet();
        }
    }

    void AddCargo(GameObject cargo)
    {
        int newCount = CurrentCargo + 1;
        if (newCount > PanelMenu.maxCargo) return;

        PlayerPrefs.SetInt("currentCargo", newCount);
        PlayerPrefs.Save();

        string cleanName = cargo.name.Replace("(Clone)", "").Trim();
        CargoManager.Instance.AddCargoPrefab(cleanName);

        Destroy(cargo);
        UpdateCounter();
    }

    void TryLoadPlanet()
    {
        if (HasCargo)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            BlinkDoor();
        }
    }

   void BlinkDoor()
{
    // Получаем рендерер двери (предполагаем, что скрипт висит на объекте двери)
    Renderer doorRenderer = GetComponent<Renderer>();
    if (doorRenderer == null) return;

    // Запоминаем исходный материал
    Material originalMaterial = doorRenderer.material;
    
    // Создаем материалы из ресурсов
    Material redMat = Resources.Load<Material>("3d_models/Materials/red");
    Material greenMat = Resources.Load<Material>("3d_models/Materials/green");
    
    if (redMat == null || greenMat == null)
    {
        Debug.LogError("Не найдены материалы для анимации!");
        return;
    }

    // Запускаем корутину для мигания
    StartCoroutine(BlinkAnimation(doorRenderer, originalMaterial, redMat, greenMat));
}

IEnumerator BlinkAnimation(Renderer renderer, Material originalMat, Material redMat, Material greenMat)
{
    int blinkCount = 6; // Количество миганий
    float blinkDuration = 0.15f; // Длительность каждого мигания
    
    for (int i = 0; i < blinkCount; i++)
    {
        // Чередуем красный и зеленый
        renderer.material = (i % 2 == 0) ? redMat : greenMat;
        yield return new WaitForSeconds(blinkDuration);
    }
    
    // Возвращаем исходный материал
    renderer.material = originalMat;
}

    void OnDisable()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Planet"))
        {
            PlayerPrefs.DeleteKey("currentCargo");
        }
    }
}
