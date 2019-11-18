﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToAkramYouthAR()
    {
        SceneManager.LoadScene("AkramYouthAR");
    }

    public void GoToNovembereAR()
    {
        SceneManager.LoadScene("November");
    }


    public void GoToVideo()
    {
        SceneManager.LoadScene("Video");
    }

    public void GoToAbout()
    {
        SceneManager.LoadScene("About");
    }

    public void OpenYouthApp()
    {
        Application.OpenURL("market://details?id=org.dadabhagwan.youth");
    }

    public void OpenAkramYouthMagazine()
    {
        Application.OpenURL("https://youth.dadabhagwan.org/gallery/akram-youth/");
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
