using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetDeliveryZone : MonoBehaviour
{
    private int expectedCargoCount;

    private void Start()
    {
        expectedCargoCount = CargoManager.Instance.cargoPrefabs.Count;
        Debug.Log($"Ожидается доставка {expectedCargoCount} грузов");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Cargo")) return;

        Destroy(other.gameObject);
        PanelMenu.credits += 250; // Увеличиваем общие кредиты
        expectedCargoCount--;

        Debug.Log($"Груз доставлен. Осталось: {expectedCargoCount}");

        if (expectedCargoCount <= 0)
        {
            CompleteDelivery();
        }
    }

    private void CompleteDelivery()
    {
        Debug.Log("Все грузы доставлены! Возвращаемся в HUB");

        PlayerPrefs.DeleteKey("currentCargo");
        PlayerPrefs.Save();

        CargoManager.Instance.ClearCargoPrefabs();

        SceneManager.LoadScene("HUB");
    }
}
