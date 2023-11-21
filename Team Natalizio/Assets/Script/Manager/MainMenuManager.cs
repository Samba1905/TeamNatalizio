using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("MainMenuButtons")]
    public GameObject continueButton;
    public GameObject newGameButton, optionsButton, creditsButton, exitButton;
    [Header("Panels")]
    public GameObject optionPanel;
    public GameObject videoPanel, audioPanel, gamePanel;

    #region MainMenu
    public void ContinueButton()
    {
        //Funzione di caricamneto ultima scena/salvataggio
    }

    public void NewGameButton()
    {
        //SceneManager.LoadScene(); Carica scena e crea salvataggio
    }

    public void OptionsButton() //Apre menu opzioni
    {
        newGameButton.SetActive(false);
        continueButton.SetActive(false);
        optionsButton.SetActive(false);
        creditsButton.SetActive(false);
        exitButton.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void CreditsButton()
    {

    }

    public void ExitButton() //Chiude il gioco
    {
        Application.Quit();
    }
    #endregion
    #region Options
    public void OptionsVideo()
    {
        optionPanel.SetActive(false);
        videoPanel.SetActive(true);
    }

    public void OptionsAudio()
    {
        optionPanel.SetActive(false);
        audioPanel.SetActive(true);
    }

    public void OptionsGame()
    {
        optionPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void BackToMainMenuFromOptions()
    {
        newGameButton.SetActive(true);
        continueButton.SetActive(true);
        optionsButton.SetActive(true);
        creditsButton.SetActive(true);
        exitButton.SetActive(true);
        optionPanel.SetActive(false);
    }
    #endregion
    #region Video
    public void BackToOptionsFromVideo()
    {
        videoPanel.SetActive(false);
        optionPanel.SetActive(true);
    }
    #endregion
    #region Audio
    public void BackToOptionsFromAudio()
    {
        audioPanel.SetActive(false);
        optionPanel.SetActive(true);
    }
    #endregion
    #region Game
    public void BackToOptionsFromGame()
    {
        gamePanel.SetActive(false);
        optionPanel.SetActive(true);
    }
    #endregion
}
