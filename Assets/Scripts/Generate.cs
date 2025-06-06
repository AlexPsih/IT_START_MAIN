using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public int range;
    public int counts=50;
    public GameObject[] planets;
    public GameObject meteor;
    public GameObject pirats;
    private Transform player;
    float timer_mateor=5;
    float timer_pirats=10;
    public MissionRealTime mission;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<Transform>();

        for (int i=0; i< counts; i++)
        {
            Instantiate(planets[Random.RandomRange(0,planets.Length)], new Vector3(Random.RandomRange(-range, range), Random.RandomRange(-range, range), Random.RandomRange(-range, range)*0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(ExecuteWithDelay());
        timer_mateor -= 0.01f;
        timer_pirats -= 0.01f;

        if (timer_mateor<0)
        {
            ExecuteWithDelay();
            timer_mateor = 1;
        }
        if (mission.Mission_type==1)
        {
            Pirats();
        }
    }

    void Pirats()
    {
        float range = player.transform.position.x + 10;
        if (timer_pirats < 0)
        {
            timer_pirats = 10;
            Instantiate(pirats, new Vector3(Random.RandomRange(-range, range), Random.RandomRange(-range, range), 0), Quaternion.identity);
        }
    }
        void ExecuteWithDelay()
    {

        float range = transform.position.x + 50;

        //yield return new WaitForSeconds(20f);

        Instantiate(meteor, new Vector3(Random.RandomRange(-range, range), Random.RandomRange(-range, range), Random.RandomRange(-range, range)), Quaternion.identity);

        //yield return new WaitForSeconds(20f);
    }
}
