using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public GameObject storyPanel;

    public void OnStartClicked()
    {
        SceneManager.LoadScene("HUB");
    }

    public void OnStoryClicked()
    {
        storyPanel.SetActive(true);
    }

    public void OnBackClicked()
    {
        storyPanel.SetActive(false);
    }
}
