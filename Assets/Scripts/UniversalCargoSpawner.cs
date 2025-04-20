using UnityEngine;

public class UniversalCargoSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }

        string cargoList = PlayerPrefs.GetString("cargoList", "");
        Debug.Log($"cargoList received: {cargoList}");
        if (string.IsNullOrEmpty(cargoList))
        {
            Debug.LogWarning("No cargo to spawn.");
            return;
        }

        string[] cargos = cargoList.Split(',');
        int index = 0;

        foreach (string cargoName in cargos)
        {
            Debug.Log($"Attempting to spawn: {cargoName}");
            GameObject prefab = Resources.Load<GameObject>($"Cargo/{cargoName}");
            if (prefab != null)
            {
                Vector3 spawnPosition = spawnPoint.position + new Vector3(index * 1.5f, 0, 0);
                GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
                Debug.Log($"Spawned {cargoName} at position {spawnPosition} with layer {LayerMask.LayerToName(spawnedObject.layer)}");
                index++;
            }
            else
            {
                Debug.LogWarning($"Prefab for {cargoName} not found in Resources/Cargo!");
            }
        }

        PlayerPrefs.SetString("cargoList", "");
        PlayerPrefs.Save();
        Debug.Log("cargoList cleared");
    }
}