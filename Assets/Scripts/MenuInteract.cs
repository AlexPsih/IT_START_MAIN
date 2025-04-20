using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInteract : MonoBehaviour, IInteract
{

    public PanelMenu panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (gameObject.name== "arrowR")
        {
            panel.page += 1;
        }
        if (gameObject.name == "arrowL")
        {
            panel.page -= 1;
        }
    }
}
