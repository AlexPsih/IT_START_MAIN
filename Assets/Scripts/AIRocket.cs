using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRocket : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public float activationDistance = 10f;

    [Header("Movement Settings")]
    public float speed = 15f;
    public float acceleration = 2f;
    public float maxSpeed = 30f;
    public float rotationSpeed = 200f;

    [Header("Guidance Settings")]
    public float predictionAmount = 0.5f;
    public float homingSharpness = 5f;

    private float currentSpeed = 0f;
    private bool isActive = false;

    [Header("Effects")]
    public ParticleSystem engineParticles;
    public AudioSource engineSound;

    void Update()
    {
        if (target == null) return;

        // Check distance to activate
        if (!isActive && Vector3.Distance(transform.position, target.position) < activationDistance)
        {
            isActive = true;
            if (engineParticles != null) engineParticles.Play();
            if (engineSound != null) engineSound.Play();
        }

        if (!isActive) return;

        // Calculate direction with simple prediction
        Vector3 targetVelocity = target.GetComponent<Rigidbody>() != null ?
            target.GetComponent<Rigidbody>().velocity * predictionAmount :
            Vector3.zero;

        Vector3 predictedPosition = target.position + targetVelocity;
        Vector3 direction = (predictedPosition - transform.position).normalized;

        // Rotate towards target
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);

        // Accelerate gradually
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

        // Move forward
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // Additional homing effect for tighter turns
        if (homingSharpness > 0)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                homingSharpness * Time.deltaTime);
        }
    }

    void OnDrawGizmos()
    {
        if (target != null && isActive)
        {
            Gizmos.color = Color.red;
            Vector3 targetVelocity = target.GetComponent<Rigidbody>() != null ?
                target.GetComponent<Rigidbody>().velocity * predictionAmount :
                Vector3.zero;
            Vector3 predictedPosition = target.position + targetVelocity;
            Gizmos.DrawLine(transform.position, predictedPosition);
            Gizmos.DrawSphere(predictedPosition, 0.3f);
        }
    }
}