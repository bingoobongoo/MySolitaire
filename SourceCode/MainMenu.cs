using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
      SceneManager.LoadScene("Gameplay");
   }

   public static void LoadMainMenu()
   {
      SceneManager.LoadScene("Main Menu");
   }

   public void QuitGame()
   {
      Application.Quit();
      Debug.Log("Game shutting down");
   }
}
