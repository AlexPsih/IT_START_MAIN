using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TriggerHUBDoor : MonoBehaviour
{
    public bool Sklad;
    public int mesto;
    public int mestomax = 2; // Устанавливаем 2 по умолчанию, как в PlayerPrefs
    public TextMeshPro mestotext;

    void Start()
    {
        // Очищаем cargoList при старте
        PlayerPrefs.SetString("cargoList", "");
        PlayerPrefs.Save();
        // Логируем начальное значение mestomax
        mestomax = PlayerPrefs.GetInt("gruz", mestomax);
        Debug.Log($"Initial mestomax: {mestomax}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (Sklad && mesto < mestomax)
        {
            Debug.Log($"Trigger entered by {other.name}, mesto = {mesto}, mestomax = {mestomax}");

            // Получаем текущий список грузов
            string cargoList = PlayerPrefs.GetString("cargoList", "");
            string cargoName = other.name.Replace("(Clone)", "");

            // Обрабатываем объект
            bool isValidObject = false;
            if (other.name == "barrel_3_2(Clone)")
            {
                PlayerPrefs.SetInt("mission", 0);
                Debug.Log("Миссия на время");
                isValidObject = true;
            }
            else if (other.name == "barrel_3_1(Clone)")
            {
                PlayerPrefs.SetInt("mission", 1);
                Debug.Log("Миссия с пиратами");
                isValidObject = true;
            }
            else if (other.name == "barrel_1(Clone)")
            {
                PlayerPrefs.SetInt("mission", 2);
                Debug.Log("Миссия на время");
                isValidObject = true;
            }
            else if (other.name == "barrel_2(Clone)")
            {
                PlayerPrefs.SetInt("mission", 3);
                Debug.Log("Миссия с пиратами");
                isValidObject = true;
            }
            else if (other.name == "post_box(Clone)")
            {
                Debug.Log("post_box added to cargoList");
                isValidObject = true;
            }
            else
            {
                Debug.LogWarning($"Unknown object {other.name} entered trigger");
                return; // Не засчитываем неизвестные объекты
            }

            if (isValidObject)
            {
                // Увеличиваем mesto
                mesto += 1;
                // Добавляем объект в cargoList
                cargoList += (cargoList == "" ? "" : ",") + cargoName;
                PlayerPrefs.SetString("cargoList", cargoList);
                PlayerPrefs.Save();
                Debug.Log($"cargoList updated: {cargoList}");

                // Уничтожаем объект
                GameObject.Destroy(other.gameObject);
                Debug.Log($"Destroyed {other.name}");
            }
        }
        else
        {
            Debug.Log($"Trigger ignored: Sklad={Sklad}, mesto={mesto}, mestomax={mestomax}");
        }

        if (!Sklad && other.name == "FirstPersonController")
        {
            Debug.Log("Loading scene 1");
            SceneManager.LoadScene(1);
        }
    }

    void Update()
    {
        // Обновляем mestomax из PlayerPrefs
        int newMestomax = PlayerPrefs.GetInt("gruz", mestomax);
        if (newMestomax != mestomax)
        {
            mestomax = newMestomax;
            Debug.Log($"mestomax updated to: {mestomax}");
        }

        if (mestotext != null)
        {
            mestotext.text = mesto.ToString() + "/" + mestomax.ToString();
        }
    }
}