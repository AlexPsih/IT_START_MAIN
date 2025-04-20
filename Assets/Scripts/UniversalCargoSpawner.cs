using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCargoSpawner : MonoBehaviour
{
    // Точка спавна (можно задать в инспекторе или найти в сцене)
    public Transform spawnPoint;

    void Start()
    {
        // Если spawnPoint не назначен, используем позицию текущего объекта
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }

        // Читаем список грузов из PlayerPrefs
        string cargoList = PlayerPrefs.GetString("cargoList", "");
        if (string.IsNullOrEmpty(cargoList))
        {
            Debug.Log("No cargo to spawn.");
            return;
        }

        // Разделяем строку на массив имен бочек
        string[] cargos = cargoList.Split(',');

        // Счётчик для позиционирования
        int index = 0;

        // Спавним каждую бочку
        foreach (string cargoName in cargos)
        {
            // Загружаем префаб из папки Resources
            GameObject prefab = Resources.Load<GameObject>($"Cargo/{cargoName}");
            if (prefab != null)
            {
                // Вычисляем позицию с небольшим смещением, чтобы бочки не накладывались
                Vector3 spawnPosition = spawnPoint.position + new Vector3(index * 1.5f, 0, 0);
                Instantiate(prefab, spawnPosition, spawnPoint.rotation);
                index++;
            }
            else
            {
                Debug.LogWarning($"Prefab for {cargoName} not found in Resources/Cargo!");
            }
        }

        // Очищаем список грузов после спавна (опционально)
        PlayerPrefs.SetString("cargoList", "");
        PlayerPrefs.Save();
    }
}
