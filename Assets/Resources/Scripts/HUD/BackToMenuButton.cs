using UnityEngine;

public class BackToMenuButton : MonoBehaviour
{
    public void OnBackToMenuButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
