using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string scene;
    public enum MainMenuStates
    {
        Play,
        Settings,
        Credits,
        Quit
    }

    
    public MainMenuStates state;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (state)
            {
                case MainMenuStates.Play:
                    PlayFunction();
                    break;
                case MainMenuStates.Settings:
                    //SettingsFunction();
                    break;
                case MainMenuStates.Credits:
                    //CreditsFunction();
                    break;
                case MainMenuStates.Quit:
                    //QuitFunction();
                    break;
            }
        }
        
    }

    private void PlayFunction()
    {
        SceneManager.LoadScene(scene);
    }
}
