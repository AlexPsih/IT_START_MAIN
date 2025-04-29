using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetPlanetScript : MonoBehaviour
{
    public int range; // Диапазон, в котором может появиться планета
    public GameObject[] planet; // Массив префабов планет (должен быть заполнен в Unity)

    void Start()
    {
        // 1. Загружаем номер миссии из сохранённых данных (0 по умолчанию)
        int planetid = PlayerPrefs.GetInt("mission", 0);

        // 2. Размещаем планету в случайной позиции в пределах range
        transform.position = new Vector3(
            Random.Range(-range, range), 
            Random.Range(-range, range), 
            0f // Z = 0, чтобы планета была на 2D-экране
        );

        // 3. Активируем только одну планету, соответствующую mission
        for (int i = 0; i < planet.Length; i++)
        {
            planet[i].SetActive(i == planetid); // Включаем только нужную
        }
    }

    // При столкновении с игроком
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "player")
        {
            // Получаем текущий mission (тип доставленной бочки)
            int planetid = PlayerPrefs.GetInt("mission", 0);

            // Загружаем сцену в зависимости от mission
            switch (planetid)
            {
                case 0: SceneManager.LoadScene(5); break; // barrel_3_2 → сцена 4
                case 1: SceneManager.LoadScene(6); break; // barrel_3_1 → сцена 5
                case 2: SceneManager.LoadScene(4); break; // barrel_1   → сцена 3
                case 3: SceneManager.LoadScene(3); break; // barrel_2   → сцена 2
                default: SceneManager.LoadScene(5); break; // На всякий случай
            }
        }
    }
}