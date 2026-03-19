using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int levelIndex;
    public int requiredStars;
    public string[] scenes;
}
