using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    
    public void WinGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Win");
            Win();
        }
    }

    void Win()
    {
        SceneManager.LoadScene(2);
    }
    
    public void EndGame ()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Restart();
        }    
        
    }

    void Restart()
    {
        SceneManager.LoadScene("GameOver");
    }
}
