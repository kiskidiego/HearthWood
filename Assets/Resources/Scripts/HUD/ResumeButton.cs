using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    public void OnResumeButtonPressed()
    {
        GameManager.Instance.ResumeGame();
    }
}
