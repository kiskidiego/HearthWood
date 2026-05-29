using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
