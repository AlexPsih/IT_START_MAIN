using UnityEngine;

public class Sheild : MonoBehaviour 
{
    public GameObject exploison;
    public Camera camera;
    public static int strenght;

    private void Start()
    {
        // Жесткий сброс при старте
        strenght = 15;
        PlayerPrefs.SetInt("Sheild", strenght);
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if ("SP_Rock06(Clone)" == other.name) 
        {
            camera.orthographicSize = 27.71f;
            Time.timeScale = 0.1f;
            Instantiate(exploison, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            strenght--;
            PlayerPrefs.SetInt("Sheild", strenght);
            PlayerPrefs.Save();
        }
    }

    private void Update() 
    {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 36.21f, 0.01f);
        Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 0.01f);

        if (strenght < 0) 
        {
            Destroy(gameObject);
            camera.orthographicSize = 36.21f;
            Time.timeScale = 1f;
        }
    }
}
