using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static bool online = false;
    public static bool localMulti = false;

     private void Start() {
          variables.lives = 2;
          variables.levelNumber = 1;
          variables.score = 0;
          online = false;
          localMulti = false;
     }

   public void PlaySingleplayer(){
        SceneManager.LoadScene("Game"); 
   }

   public void PlayLoacalMultiplayer(){
        SceneManager.LoadScene("WinGamesSelect");
        localMulti = true;
   }

    public void PlayOnlineMultiplayer(){
        SceneManager.LoadScene("Game");
        online = true;
   }
}
