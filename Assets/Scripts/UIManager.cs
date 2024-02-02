using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] GameObject pauseScreen;

    bool _isPaused = false;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    void Awake()
    {
        if(pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && pauseScreen != null)
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }
}
