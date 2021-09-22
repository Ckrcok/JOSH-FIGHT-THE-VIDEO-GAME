using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUi;

    [SerializeField] private bool isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            ActivateMenu();
        }

        else
        {
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        Time.timeScale = 0;
        pauseMenuUi.SetActive(true);
        GameObject.Find("playerChar").GetComponent<PlayerController>().enabled = false;
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        pauseMenuUi.SetActive(false);
        isPaused = false;
        GameObject.Find("playerChar").GetComponent<PlayerController>().enabled = true;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
