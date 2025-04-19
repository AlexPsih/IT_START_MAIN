using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{
    public GameObject exploison;
    public int strenght=15;
    public Camera camera;

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if ("SP_Rock06(Clone)" == other.name)
        {
            camera.orthographicSize = 27.71f;
            Time.timeScale = 0.1f;
            Instantiate(exploison, other.gameObject.transform.position, Quaternion.identity);
            GameObject.Destroy(other.gameObject);
            strenght -= 1;
            
        }
            
        
    }

    private void Update()
    {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 36.21f, 0.01f);
        Time.timeScale = Mathf.Lerp(Time.timeScale,1,0.01f);
        if (strenght<0)
        {
            GameObject.Destroy(gameObject);
            camera.orthographicSize = 36.21f;
            Time.timeScale = 1f;
        }
        
    }
}
