using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetDeliveryZone : MonoBehaviour
{
    private int expectedCargoCount;

    private void Start()
    {
        // Получаем количество грузов, которые заспавнил PlanetCargoSpawner
        expectedCargoCount = CargoManager.Instance.cargoPrefabs.Count;
        Debug.Log($"Ожидается доставка {expectedCargoCount} грузов");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это груз
        if (!other.CompareTag("Cargo")) return;

        // Уничтожаем доставленный груз
        Destroy(other.gameObject);
        
        // Уменьшаем счетчик ожидаемых грузов
        expectedCargoCount--;
        
        Debug.Log($"Груз доставлен. Осталось: {expectedCargoCount}");

        // Если все грузы доставлены
        if (expectedCargoCount <= 0)
        {
            CompleteDelivery();
        }
    }

    private void CompleteDelivery()
    {
        Debug.Log("Все грузы доставлены! Возвращаемся в HUB");
        
        // Очищаем данные о текущем грузе
        PlayerPrefs.DeleteKey("currentCargo");
        PlayerPrefs.Save();
        
        // Очищаем список грузов для следующей миссии
        CargoManager.Instance.ClearCargoPrefabs();
        
        // Возвращаемся в HUB
        SceneManager.LoadScene("HUB");
    }
}