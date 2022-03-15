using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        ResetStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void ResetStats()
    {
        Score.totalScore = 0;
        Score.totalKills = 0;
        Score.enemies = 0;
        Score.batteries = 1;
        Score.obstacles = 0;
        Score.health = Score.maxHealth;
    }
}
