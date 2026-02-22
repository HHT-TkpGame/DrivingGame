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
    public DriveType currentType;
    public void SetType(DriveType type)
    {
        currentType = type;
    }
}
