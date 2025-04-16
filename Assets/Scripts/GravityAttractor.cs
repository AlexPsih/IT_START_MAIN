using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    [Header("Настройки Гравитации")]
    [SerializeField] private float gravityForce = 9.8f; 
    [SerializeField] private float attractionRadius = 10f; 
    [SerializeField] private bool inverseSquareLaw = true; 

    [Header("Визаулизация")]
    [SerializeField] private bool showGizmo = true;
    [SerializeField] private Color gizmoColor = Color.cyan;

    float time_animation;

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb != this.GetComponent<Rigidbody>())
            {
                AttractObject(rb);
            }
        }
    }

    private void AttractObject(Rigidbody rb)
    {

        Vector3 direction = transform.position - rb.position;
        float distance = direction.magnitude;

        if (distance == 0f) return;


        direction = direction.normalized;


        float forceMagnitude = gravityForce;

        if (inverseSquareLaw)
        {
            forceMagnitude = gravityForce * rb.mass / Mathf.Pow(distance, 2);
        }
        else
        {
            forceMagnitude = gravityForce * rb.mass * (1 - distance / attractionRadius);
        }


        rb.AddForce(direction * forceMagnitude);
    }


    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, attractionRadius);
            Gizmos.DrawSphere(transform.position, -attractionRadius);

            Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius);

            foreach (Collider collider in colliders)
            {
                //анимция
                time_animation += 0.001f;
                Vector3 pos = Vector3.Lerp(collider.transform.position, transform.position, time_animation);
                Gizmos.color = new Color(0,1,0,1);
                Gizmos.DrawLine(transform.position, collider.transform.position);
                Gizmos.DrawSphere(pos,0.3f);
            }

            if (time_animation>1)
            {
                time_animation = 0;
            }

        }
    }
}