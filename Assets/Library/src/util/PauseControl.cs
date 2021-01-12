using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public bool gameIsPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            // would be nice if we could reduce timescale to 0 gradually over the course of one second or so instead of instantly
        }
        else
        {
            Time.timeScale = 1;
            //same as above but getting back up to timescale of 1
        }
    }
}