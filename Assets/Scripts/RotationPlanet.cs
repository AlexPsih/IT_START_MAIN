using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlanet : MonoBehaviour
{
    Vector3 rot_vector;
    float range;
    public float speed=1;
    void Start()
    {
        range = Random.RandomRange(-5,5);
        rot_vector = new Vector3(range, range, range);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rot_vector*0.1f*speed);
    }


}
