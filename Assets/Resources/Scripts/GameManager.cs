using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private float _resetDelay = 3f;
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private GameObject _pauseMenu;
    bool _isPaused = false;
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        _pauseAction.action.Enable();
        _pauseAction.action.performed += OnPauseInput;
    }
    void OnDisable()
    {
        _pauseAction.action.Disable();
        _pauseAction.action.performed -= OnPauseInput;
    }
    public void Win()
    {
        Time.timeScale = 0f;
        _winScreen.SetActive(true);
        StartCoroutine(BackToMenuCoroutine());
    }
    public void Lose()
    {
        Time.timeScale = 0f;
        _loseScreen.SetActive(true);
        StartCoroutine(BackToMenuCoroutine());
    }

    IEnumerator BackToMenuCoroutine()
    {
        yield return new WaitForSecondsRealtime(_resetDelay);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isPaused = false;
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isPaused = true;
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }
}
