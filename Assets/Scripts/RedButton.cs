using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedButton : MonoBehaviour, IInteract
{
    public GameObject[] barrels;
    public GameObject postBoxPrefab; // Префаб post_box, назначьте в инспекторе
    bool active = true;

    public void Interact()
    {
        if (active)
        {
            active = false;
            GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1f);

            // Спавним одну случайную бочку
            int type = Random.Range(0, barrels.Length);
            Instantiate(barrels[type], new Vector3(25.11f, 15.124f, -22.61f), Quaternion.identity);

            // Спавним два post_box
            if (postBoxPrefab != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector3 postBoxPosition = new Vector3(25.11f + (i + 1) * 1.5f, 15.124f, -22.61f);
                    Instantiate(postBoxPrefab, postBoxPosition, Quaternion.identity);
                }
            }
            else
            {
                Debug.LogWarning("postBoxPrefab not assigned in RedButton!");
            }
        }
    }
}