using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadNewGame()
    {
        SceneManager.LoadScene("Tryb zwykły");
    }
    public void LoadMenu()
    {
      
        SceneManager.LoadScene("GUI");
    }

    public void LoadChallengesMode()
    {
        SceneManager.LoadScene("Tryb wyzwań");
    }

    public void LoadSettings() =>
        SceneManager.LoadScene("Ustawienia");

    public void LoadMinutka() =>
        SceneManager.LoadScene("Minutka");


    public void LoadPoorzednia() =>
        SceneManager.LoadScene("Podrzad");
    public void ChangeDifficulty(){
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
