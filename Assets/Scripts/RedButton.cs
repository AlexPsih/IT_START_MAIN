using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour, IInteract
{
    public GameObject[] barrels;    // Массив бочек (barrel_1, barrel_2...)
    public GameObject postBox;      // Префаб почтового ящика (Post_Box)
    public Vector3 spawnPosition = new Vector3(25.11f, 15.124f, -22.61f);
    bool active = true;
    
    public void Interact()
    {
        if (active) 
        {
            active = false;

            // 1. Меняем цвет кнопки на зелёный
            GetComponent<Renderer>().material.color = Color.green;

            // 2. Спавним 1 случайную бочку
            int randomBarrelIndex = Random.Range(0, barrels.Length);
            Instantiate(barrels[randomBarrelIndex], spawnPosition, Quaternion.identity);

            // ✨ Сохраняем mission в PlayerPrefs
            PlayerPrefs.SetInt("mission", randomBarrelIndex);
            PlayerPrefs.Save();
            Debug.Log($"RedButton: mission set to {randomBarrelIndex}");

            // 3. Спавним 2 почтовых ящика
            Instantiate(postBox, spawnPosition + new Vector3(2, 0, 0), Quaternion.identity);
            Instantiate(postBox, spawnPosition + new Vector3(-2, 0, 0), Quaternion.identity);
        }
    }
}
