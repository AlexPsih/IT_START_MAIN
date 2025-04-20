using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TriggerHUBDoor : MonoBehaviour
{
    public bool Sklad;
    public int mesto;
    public int mestomax=1;
    public TextMeshPro mestotext;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Sklad && mesto < mestomax)
        {
            mesto += 1;
            // �������� ������� ������ ������ �� PlayerPrefs ��� ������� ������
            string cargoList = PlayerPrefs.GetString("cargoList", "");
            // ������� ������� (Clone) �� ����� �������
            string cargoName = other.name.Replace("(Clone)", "");

            if (other.name == "barrel_3_2(Clone)")
            {
                PlayerPrefs.SetInt("mission", 0);
                print("������ �� �����");
                cargoList += (cargoList == "" ? "" : ",") + cargoName;
                GameObject.Destroy(other.gameObject);
            }
            if (other.name == "barrel_3_1(Clone)")
            {
                PlayerPrefs.SetInt("mission", 1);
                print("������ � ��������");
                cargoList += (cargoList == "" ? "" : ",") + cargoName;
                GameObject.Destroy(other.gameObject);
            }
            if (other.name == "barrel_1(Clone)")
            {
                PlayerPrefs.SetInt("mission", 2);
                cargoList += (cargoList == "" ? "" : ",") + cargoName;
                GameObject.Destroy(other.gameObject);
            }
            if (other.name == "barrel_2(Clone)")
            {
                PlayerPrefs.SetInt("mission", 3);
                cargoList += (cargoList == "" ? "" : ",") + cargoName;
                GameObject.Destroy(other.gameObject);
            }

            // ��������� ����������� ������ ������
            PlayerPrefs.SetString("cargoList", cargoList);
            PlayerPrefs.Save();
        }
        if (!Sklad && other.name == "FirstPersonController")
        {
            SceneManager.LoadScene(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mestomax = PlayerPrefs.GetInt("gruz",2); 

        mestotext.text =mesto.ToString()+"/"+mestomax.ToString();
    }
}
