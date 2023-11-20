using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    #region Singleton
    private static MainMenuManager _instance;
    public static MainMenuManager Instance
    {
        get
        {
            if (_instance == null) Debug.Log("MainMenuManager null");
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        #region Singleton
        if (_instance) Destroy(gameObject);
        else _instance = this;

        DontDestroyOnLoad(gameObject);
        #endregion
    }

    public void ContinueButton()
    {

    }

    public void NewGameButton()
    {

    }

    public void OptionsButton()
    {

    }

    public void CreditsButton()
    {

    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
