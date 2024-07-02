using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelStorage", menuName = "Level/LevelStorage")]
public class LevelStorage : ScriptableObject
{
    [field:SerializeField] public List<LevelConfig> LevelConfig { get; private set; }
}



