using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [Header("AudioSource")]
    public Slider masterSlider;
    public Slider musicSlider, SFXSlider;
    public AudioSource masterVolume, musicVolume, SFXVolume;
    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/Data.json"))
        {
            LoadDataFile();
        }
    }

    private void LateUpdate()
    {
        AudioChanger();
    }

    #region MainMenu
    public void ContinueButton()
    {
        //Funzione di caricamneto ultima scena/salvataggio
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene("Level1");
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
    public void AudioChanger()
    {
        masterVolume.volume = masterSlider.value;
        musicVolume.volume = musicSlider.value;
        SFXVolume.volume = SFXSlider.value;
    }
    public void BackToOptionsFromAudio()
    {
        SaveDataFile();

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

    void SaveDataFile() //Funzione per salvare impostazioni
    {
        Data data = new Data();

        //Options per audio
        data.masterVolume = masterVolume.volume;
        data.musicVolume = musicVolume.volume;
        data.SFXVolume = SFXVolume.volume;
        //DA AGGIUNGERE TUTTO SUCCESSIVAMENTE

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/Data.json", json);
    }

    void LoadDataFile() //Funzione per caricare impostazioni
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/Data.json");

        Data data = JsonUtility.FromJson<Data>(json);

        //Options per audio
        masterSlider.value = data.masterVolume;
        musicSlider.value = data.musicVolume;
        SFXSlider.value = data.SFXVolume;
        //DA AGGIUNGERE TUTTO SUCCESSIVAMENTE
    }
}
