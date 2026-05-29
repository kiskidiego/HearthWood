using UnityEngine;

public class BackToMenuButton : MonoBehaviour
{
    public void OnBackToMenuButtonPressed()
    {
        Time.timeScale = 1f; // Ensure time is running when going back to menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
