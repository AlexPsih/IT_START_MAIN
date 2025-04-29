using UnityEngine;
using UnityEngine.UI;

public class CreditsDisplay : MonoBehaviour
{
    public Text creditsText;

    void Update()
    {
        creditsText.text = PanelMenu.credits.ToString() + " CR";
    }
}
