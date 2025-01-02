using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string scene;
    public Transform endPosition;
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
                    SettingsFunction(other.transform);
                    break;
                case MainMenuStates.Credits:
                    CreditsFunction(other.transform);
                    break;
                case MainMenuStates.Quit:
                    QuitFunction();
                    break;
            }
        }
        
    }

    private void PlayFunction()
    {
        SceneManager.LoadScene(scene);
    }

    private void SettingsFunction(Transform player)
    {
        VRTeleportScript.TeleportVrPlayer(player, endPosition.position  );
        
    }

    private void CreditsFunction(Transform player)
    {
        VRTeleportScript.TeleportVrPlayer(player, endPosition.position);   
    }

    private void QuitFunction()
    {
        Application.Quit();
    }
}
