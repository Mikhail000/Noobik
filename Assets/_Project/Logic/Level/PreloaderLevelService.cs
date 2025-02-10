using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class PreloaderLevelService
{
    public bool[] Levels = new bool[3];
    private int _currentLevelNumb;

    public void SetCurrentLevel(int levelNumb)
    {
        _currentLevelNumb = levelNumb;
    }

    public int GetCurrentLevel()
    {
        return _currentLevelNumb;
    }

    public void LoadSaveCloud()
    {
        //Levels = YG2.saves.openLevels;
    }

    public void SaveAndLoadNextLevel()
    {
        //YG2.saves.openLevels[_currentLevelNumb] = true;

        _currentLevelNumb++;

        SceneManager.LoadScene("Gameplay");
    }

}

