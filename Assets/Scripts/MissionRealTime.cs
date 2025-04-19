using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionRealTime : MonoBehaviour
{
    public Text texttimer;
    float timer_s =60;
    float timer_m=5;
    public int Mission_type;
    void Start()
    {
        Mission_type = PlayerPrefs.GetInt("mission",0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mission_type == 0)
        {
            timer_s -= 0.1f;
            if (timer_s < 0)
            {
                timer_s = 60;
                timer_m -= 1;
            }
            if (timer_s < 10)
            {
                texttimer.text = "время 0" + timer_m.ToString() + ":0" + ((int)timer_s).ToString();
            }
            else
            {
                texttimer.text = "время 0" + timer_m.ToString() + ":" + ((int)timer_s).ToString();
            }
        }
        else if (Mission_type==1)
        {
            texttimer.text = "";
        }
    }
}
