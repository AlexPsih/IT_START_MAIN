using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] 
    private float rotationSpeed = 30f; // Скорость вращения (градусы в секунду)

    void Update()
    {
        // Вращаем объект по оси Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
