using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TriggerHUBDoor : MonoBehaviour
{
    [Header("Настройки")]
    public bool isWarehouse;
    public int maxCargo = 3;
    
    [Header("Счетчик")]
    public TextMeshPro cargoCounterText;
    
    public int CurrentCargo => PlayerPrefs.GetInt("currentCargo", 0);
    public bool HasCargo => CurrentCargo > 0;

    void Start()
    {
        PlayerPrefs.SetInt("currentCargo", 0); // ОБНУЛЯЕМ груз при старте
        PlayerPrefs.Save();
        UpdateCounter();
        Debug.Log($"Старт системы. Грузы: {CurrentCargo}, hasCargo: {HasCargo}");
    }

    void UpdateCounter()
    {
        if (cargoCounterText != null)
        {
            cargoCounterText.text = $"{CurrentCargo}/{maxCargo}";
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
        if (newCount > maxCargo) return;

        PlayerPrefs.SetInt("currentCargo", newCount);
        PlayerPrefs.Save();

        // ✨ Извлекаем имя без (Clone) и записываем в cargoPrefabs
        string cleanName = cargo.name.Replace("(Clone)", "").Trim();
        CargoManager.Instance.AddCargoPrefab(cleanName);

        Destroy(cargo);
        UpdateCounter();
        
        Debug.Log($"Добавлен груз. Всего: {newCount}, PlayerPrefs: {PlayerPrefs.GetInt("currentCargo")}");
    }

    void TryLoadPlanet()
    {
        Debug.Log($"Попытка загрузки. Текущие значения:\n"
                + $"PlayerPrefs: {PlayerPrefs.GetInt("currentCargo")}\n"
                + $"Свойство HasCargo: {HasCargo}\n"
                + $"cargoPrefabs: {string.Join(", ", CargoManager.Instance.cargoPrefabs)}");

        if (HasCargo)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.LogError($"Критическая ошибка! PlayerPrefs: {PlayerPrefs.GetInt("currentCargo")}, но HasCargo={HasCargo}");
            BlinkDoor();
        }
    }

    void BlinkDoor()
    {
        // Визуальная индикация ошибки
    }

    void OnDisable()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Planet"))
        {
            PlayerPrefs.DeleteKey("currentCargo");
            Debug.Log("PlayerPrefs: currentCargo cleared");
        }
    }
}
