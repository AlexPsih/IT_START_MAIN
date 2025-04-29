using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CargoManager : MonoBehaviour
{
    public static CargoManager Instance;
    public List<string> cargoPrefabs = new List<string>();
    private string[] fallbackPrefabs = { "barrel_1", "barrel_2", "barrel_3_1", "barrel_3_2", "post_box" };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"CargoManager initialized in scene {SceneManager.GetActiveScene().name}");
        }
        else
        {
            Debug.LogWarning($"CargoManager: Duplicate instance found in scene {SceneManager.GetActiveScene().name}. Destroying this one.");
            Destroy(gameObject);
            return;
        }
    }

    void OnLevelWasLoaded(int level)
    {
        Debug.Log($"CargoManager: Scene loaded - {SceneManager.GetActiveScene().name}. cargoPrefabs: {string.Join(", ", cargoPrefabs)}");
    }

    public void ClearCargoPrefabs()
    {
        cargoPrefabs.Clear();
        Debug.LogWarning($"CargoManager: cargoPrefabs cleared in scene {SceneManager.GetActiveScene().name}");
    }

    public void AddCargoPrefab(string prefabName)
    {
        cargoPrefabs.Add(prefabName);
        Debug.Log($"CargoManager: Added prefab {prefabName}. Current list: {string.Join(", ", cargoPrefabs)}");
    }

    public void EnsureCargoPrefabs()
    {
        if (cargoPrefabs.Count == 0)
        {
            Debug.LogWarning($"CargoManager: cargoPrefabs is empty in scene {SceneManager.GetActiveScene().name}. Filling with unique fallback prefabs.");
            int count = Random.Range(2, Mathf.Min(5, fallbackPrefabs.Length + 1));
            var availablePrefabs = new List<string>(fallbackPrefabs);

            for (int i = 0; i < count && availablePrefabs.Count > 0; i++)
            {
                int randomIndex = Random.Range(0, availablePrefabs.Count);
                string randomPrefab = availablePrefabs[randomIndex];
                AddCargoPrefab(randomPrefab);
                availablePrefabs.RemoveAt(randomIndex);
            }
        }
    }
}