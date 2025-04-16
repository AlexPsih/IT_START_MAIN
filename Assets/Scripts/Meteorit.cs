using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorit : MonoBehaviour
{
    public float speed = 10f; // Скорость полета метеорита
    public float rotationSpeed = 50f; // Скорость вращения метеорита
    private Transform player; // Ссылка на трансформ игрока
    private Rigidbody rb;
    Vector3 direction;

    void Start()
    {
        // Находим игрока по тегу (убедитесь что у игрока установлен тег "Player")
        player = GameObject.Find("player").GetComponent<Transform>();

        rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
        }

        direction = (player.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Направление к игроку
            //Vector3 direction = (player.position - transform.position).normalized;

            // Двигаем метеорит с помощью физики
            rb.velocity = direction * speed;

            // Вращаем метеорит для эффекта
            rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
        }
    }
}
