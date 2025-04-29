using UnityEngine;
using System.Collections.Generic; // Добавляем директиву

public class CargoListTriggerZone : MonoBehaviour
{
    [SerializeField] private string[] possiblePrefabs = { "barrel_1", "barrel_2", "barrel_3_1", "barrel_3_2", "post_box" };
    private bool hasTriggered = false;

    void OnEnable()
    {
        hasTriggered = false;
        Debug.Log("CargoListTriggerZone: Reset hasTriggered");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"CargoListTriggerZone: Trigger entered by {other.name} (Tag: {other.tag})");
        if (!other.CompareTag("Player") || hasTriggered) return;

        hasTriggered = true;
        CargoManager.Instance.ClearCargoPrefabs();

        // Получаем mission из PlayerPrefs
        int mission = PlayerPrefs.GetInt("mission", 0);
        string missionPrefab = GetMissionPrefab(mission);
        Debug.Log($"CargoListTriggerZone: mission={mission}, selected prefab={missionPrefab}");

        // Добавляем префаб, соответствующий mission
        CargoManager.Instance.AddCargoPrefab(missionPrefab);

        // Добавляем случайные уникальные префабы
        int count = Random.Range(1, Mathf.Min(4, possiblePrefabs.Length)); // 1-3 дополнительных префаба
        var availablePrefabs = new List<string>(possiblePrefabs);
        availablePrefabs.Remove(missionPrefab); // Исключаем missionPrefab

        for (int i = 0; i < count && availablePrefabs.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availablePrefabs.Count);
            string randomPrefab = availablePrefabs[randomIndex];
            CargoManager.Instance.AddCargoPrefab(randomPrefab);
            availablePrefabs.RemoveAt(randomIndex);
        }

        Debug.Log($"Триггер активирован! Добавлено {count + 1} названий в cargoPrefabs: {string.Join(", ", CargoManager.Instance.cargoPrefabs)}");
    }

    private string GetMissionPrefab(int mission)
    {
        switch (mission)
        {
            case 0: return "barrel_3_2"; // mission 0 → SATRUN2
            case 1: return "barrel_3_1"; // mission 1 → SATRUN1
            case 2: return "barrel_1";   // mission 2 → Planet1
            case 3: return "barrel_2";   // mission 3 → Planet2
            default: return "barrel_3_2";
        }
    }
}