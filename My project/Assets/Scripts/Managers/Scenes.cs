using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;

public class Scenes : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button mainMenuButton;
    public Button continueButton;

    public GameObject pauseMenu;

    [HideInInspector]
    private Scenes Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = GetComponent<Scenes>();
        Time.timeScale = 1f;

        if (startButton) startButton.onClick.AddListener(StartGame);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(MainMenu);
        if (continueButton) continueButton.onClick.AddListener(Pause);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;
        if (Input.GetKeyUp(KeyCode.P))
            Pause();
    }

    private void StartGame()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
            SceneManager.LoadScene("GameScene");
    }

    private void QuitGame()
    {
        GameManager.Instance.Quit();
    }

    private void MainMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            SceneManager.LoadScene("MainMenu");
    }

    public static void GameOver()
    {
        Debug.Log("GameOver function reached");
        GameOverInvoke();
    }
    private static void GameOverInvoke()
    {
        Debug.Log("GameOverInvoke reached");
        if (SceneManager.GetActiveScene().name != "GameOver")
            SceneManager.LoadScene("GameOver");
    }

    private void Pause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
