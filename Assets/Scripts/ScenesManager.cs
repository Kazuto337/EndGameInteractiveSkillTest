using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
    public static void PreLoadScene(int buildIndex)
    {
        SceneManager.LoadSceneAsync(buildIndex);
    }
}
