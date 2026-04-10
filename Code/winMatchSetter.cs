using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winMatchSetter : MonoBehaviour
{
    public void OneWinMatch()
    {
        variables.winMatches = 1;
        SceneManager.LoadScene("Game");
    }

    public void TwoWinMatch()
    {
        variables.winMatches = 2;
        SceneManager.LoadScene("Game");
    }

    public void ThreeWinMatch()
    {
        variables.winMatches = 3;
        SceneManager.LoadScene("Game");
    }

    public void FourWinMatch()
    {
        variables.winMatches = 4;
        SceneManager.LoadScene("Game");
    }

    public void FiveWinMatch()
    {
        variables.winMatches = 5;
        SceneManager.LoadScene("Game");
    }
}
