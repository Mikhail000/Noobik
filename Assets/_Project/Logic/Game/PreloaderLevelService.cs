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
        Levels = YandexGame.savesData.openLevels;
    }


    public void LoadNextLevel()
    {
        _currentLevelNumb++;
        SceneManager.LoadScene("Gameplay");
    }
}

