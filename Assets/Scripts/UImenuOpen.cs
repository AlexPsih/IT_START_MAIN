using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImenuOpen : MonoBehaviour, IInteract
{
    public GameObject menu;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Interact()
    {
        menu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
}
