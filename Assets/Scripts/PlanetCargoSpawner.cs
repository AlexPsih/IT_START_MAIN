using UnityEngine;

public class PlanetCargoSpawner : MonoBehaviour
{
    public Transform spawnArea;
    public float spacing = 2f;
    public int cargoCount;  // Новая переменная для отслеживания загруженных грузов

    void Start()
    {
        Debug.Log($"PlanetCargoSpawner: cargoPrefabs before ensuring: {string.Join(", ", CargoManager.Instance.cargoPrefabs)}");
        CargoManager.Instance.EnsureCargoPrefabs();

        for (int i = 0; i < CargoManager.Instance.cargoPrefabs.Count; i++)
        {
            Vector3 spawnPos = spawnArea.position + new Vector3(
                Random.Range(-spacing, spacing), 
                0, 
                Random.Range(-spacing, spacing)
            );

            string prefabName = CargoManager.Instance.cargoPrefabs[i];
            GameObject prefab = Resources.Load<GameObject>($"Cargo/{prefabName}");
            if (prefab != null)
            {
                Instantiate(prefab, spawnPos, Quaternion.identity);
                Debug.Log($"Spawned cargo: {prefabName}");
            }
            else
            {
                Debug.LogError($"Missing prefab in Resources/Cargo/: {prefabName}");
            }
        }

        cargoCount = CargoManager.Instance.cargoPrefabs.Count;  // Сохраняем количество грузов
    }
}
