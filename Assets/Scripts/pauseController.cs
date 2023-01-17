using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// Pausing and resuming the game by modifying the Time.timeScale.
/// </summary>
public class PauseController : MonoBehaviour
{
    public static bool gameIsPaused;
 
    void Start()
    {
        gameIsPaused = false;
    }
 
    void Update()
    {
        if (gameIsPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
 
    public static void PauseGame()
    {
        gameIsPaused = true;
    }
 
    public static void ResumeGame()
    {
        gameIsPaused = false;
    }
}
