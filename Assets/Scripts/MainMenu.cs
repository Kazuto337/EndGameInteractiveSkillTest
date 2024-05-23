using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu: MonoBehaviour
{
    public void Go2MainMenu()
    {
        ScenesManager.LoadScene(0);
    }

    public void PlayGame()
    {
        ScenesManager.PreLoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
