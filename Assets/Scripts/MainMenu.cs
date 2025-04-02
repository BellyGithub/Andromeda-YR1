using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;

    private void Start()
    {
        optionsMenu.SetActive(false); // Hide menu at start
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsMenu.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void PlayDemo()
    {
        SceneManager.LoadScene("Demo-Level");
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
    }

    public void QuitGame()
    {

        Application.Quit();
    }
}
