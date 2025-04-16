using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Vector3 vector_rot; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(vector_rot*0.05f);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine( transform.position, transform.position+vector_rot*4);
        Gizmos.DrawSphere(transform.position + vector_rot * 4,0.2f);
    }
}
