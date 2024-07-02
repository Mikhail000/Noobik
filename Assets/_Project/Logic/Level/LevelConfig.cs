using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelConfig : ScriptableObject
{
    [field:SerializeField] public int levelNumber { get; private set; }
    [field:SerializeField] public GameObject levelPrefab { get; private set; }
}
