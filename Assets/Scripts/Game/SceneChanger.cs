using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger
{
    public void Change(GameState newState)
    {
        Debug.Log(newState.ToString() + "Scene");
        SceneManager.LoadScene(newState.ToString() + "Scene");
    }
}
