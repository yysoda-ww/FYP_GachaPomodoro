using UnityEngine;
using UnityEngine.SceneManagement;

public class DashboardMenuUI : MonoBehaviour
{
    public void GoToFocusMode()
    {
        SceneManager.LoadScene("FocusMode");
    }

    public void GoToTasks()
    {
        SceneManager.LoadScene("Tasks");
    }

    public void GoToGacha()
    {
        SceneManager.LoadScene("Gacha");
    }

    public void GoToTeam()
    {
        SceneManager.LoadScene("Team");
    }

    public void GoToDashboard()
    {
        SceneManager.LoadScene("Dashboard");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}