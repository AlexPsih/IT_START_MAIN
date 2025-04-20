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

    public AudioClip[] dialogs;
    public AudioSource dialogsAU;
    float timedialog;

    private Rigidbody rb;
    private float currentFuel;
    private bool isThrusting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = maxFuel;

        if (thrustParticles != null)
            thrustParticles.Stop();

        rotationSpeed = PlayerPrefs.GetInt("manevr", 50);
    }

    private void Update()
    {
        HandleInput();
        UpdateFuel();
        ClampVelocity();

        timedialog -= 0.01f;
        if (timedialog<0)
        {
            timedialog = 5;
            dialogsAU.clip = dialogs[Random.RandomRange(0,dialogs.Length)];
            dialogsAU.Play();
        }
    }

    private void HandleInput()
    {
        // ¬ращение
        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward * -rotationInput * rotationSpeed * Time.deltaTime);

        // “€га
        isThrusting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow);

        // ¬ключение/выключение эффектов т€ги
        if (thrustParticles != null)
        {
            if (isThrusting)
            {
                //if (!thrustParticles.isPlaying)
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
        if (isThrusting && currentFuel > 0)
        {
            // ѕримен€ем силу в направлении "вверх" ракеты
            rb.AddForce(transform.up * thrustForce * Time.fixedDeltaTime);
        }
    }

    private void UpdateFuel()
    {
        if (isThrusting)
        {
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

            if (currentFuel <= 0)
            {
                isThrusting = false;
            }
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