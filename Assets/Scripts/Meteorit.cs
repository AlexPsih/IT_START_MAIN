using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorit : MonoBehaviour
{
    public float speed = 10f; // �������� ������ ���������
    public float rotationSpeed = 50f; // �������� �������� ���������
    private Transform player; // ������ �� ��������� ������
    private Rigidbody rb;
    Vector3 direction;

    void Start()
    {
        // ������� ������ �� ���� (��������� ��� � ������ ���������� ��� "Player")
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
            // ����������� � ������
            //Vector3 direction = (player.position - transform.position).normalized;

            // ������� �������� � ������� ������
            rb.velocity = direction * speed;

            // ������� �������� ��� �������
            rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
        }
    }
}
