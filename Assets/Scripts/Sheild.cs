using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if ("SP_Rock06(Clone)" == other.name)
        {
            GameObject.Destroy(other.gameObject);
            GameObject.Destroy(gameObject);
        }
    }
}
