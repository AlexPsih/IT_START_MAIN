using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrustForce = 1000f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float maxVelocity = 50f;

    [Header("Fuel Settings")]
    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float fuelConsumptionRate = 5f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem thrustParticles;

    private Rigidbody rb;
    private float currentFuel;
    private int isThrusting = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = maxFuel;

        if (thrustParticles != null)
            thrustParticles.Stop();
    }

    private void Update()
    {
        HandleInput();
        ClampVelocity();
    }

    private void HandleInput()
    {
        // ¬ращение
        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward * -rotationInput * rotationSpeed * Time.deltaTime);

        // “€га
        if (Input.GetKey(KeyCode.W))
        {
            isThrusting = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            isThrusting = -1;
        }

        // ¬ключение/выключение эффектов т€ги
        if (thrustParticles != null)
        {
            if (isThrusting==0 && currentFuel > 0)
            {
                if (!thrustParticles.isPlaying)
                    thrustParticles.Play();
            }
            else
            {
                if (thrustParticles.isPlaying)
                    thrustParticles.Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isThrusting!=0 && currentFuel > 0)
        {
            // ѕримен€ем силу в направлении "вверх" ракеты
            rb.AddForce(transform.up * thrustForce * Time.fixedDeltaTime);
        }
    }



    private void ClampVelocity()
    {
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    public void AddFuel(float amount)
    {
        currentFuel = Mathf.Clamp(currentFuel + amount, 0f, maxFuel);
    }

    public float GetFuelPercentage()
    {
        return currentFuel / maxFuel;
    }
}