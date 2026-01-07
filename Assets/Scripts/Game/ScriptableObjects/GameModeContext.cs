using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Context/GameModeContext")]
public class GameModeContext : ScriptableObject
{
    public GameModeState currentMode;
    public void SetMode(GameModeState mode)
    {
        currentMode = mode;
    }
}
