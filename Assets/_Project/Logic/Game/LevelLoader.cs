using UnityEngine;
using Zenject;

public class LevelLoader : MonoBehaviour
{
    //[SerializeField] private GameObject player;

    private GameObject _currentLevelPrefab;

    private PreloaderLevelService _levelPreloader;
    private LevelStorage _storage;

    private int _levelNumb;

    private Transform _startPoint;
    private Transform _finishPoint;
    private Player _player;

    [Inject]
    private void Construct(PreloaderLevelService levelService, LevelStorage storage, Player player)
    {
        _levelPreloader = levelService;
        _storage = storage;

        _levelNumb = _levelPreloader.GetCurrentLevel();

        _player = player;
    }

    private void Start()
    {
        
        
        var levelCfg = _storage.LevelConfig[_levelNumb];
        
        _currentLevelPrefab = Instantiate(levelCfg.levelPrefab);
        Vector3 p = _currentLevelPrefab.GetComponent<LevelEntity>().StartPoint.position;
        _player.SetPosition(p);
        
    }
}