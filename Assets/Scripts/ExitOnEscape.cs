using UnityEngine;

public class ExitOnEscape : MonoBehaviour 
{
    void Update() 
    {
        // Если нажат ESC - выходим
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
        #if UNITY_EDITOR
            // Останавливаем игру в редакторе Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Закрываем приложение в билде
            Application.Quit();
        #endif
    }
}