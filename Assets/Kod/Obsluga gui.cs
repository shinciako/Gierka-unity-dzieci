using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Loading new game
    public void LoadNewGame()
    {
        // Loading new scene
        SceneManager.LoadScene("Tryb zwykły");
    }
    // Function of loading menu
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

    public void ChangeDifficulty(){
        
    }

    public void ExitGame()
    {
        // Zamyka aplikację
        Application.Quit();
    }
}
