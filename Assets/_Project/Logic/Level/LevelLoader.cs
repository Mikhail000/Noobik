using UnityEngine;
using Zenject;

public class LevelLoader : MonoBehaviour
{

    private GameObject _currentLevelPrefab;
    private int _levelNumb;
    private PreloaderLevelService _levelPreloader;
    private LevelStorage _storage;


    private Transform _startPoint;
    private Transform _finishPoint;
    private Player _player;

    private CameraFollow _camera;

    private DiContainer _container;

    [Inject]
    private void Construct(PreloaderLevelService levelService, LevelStorage storage, 
        Player player, CameraFollow camera, DiContainer container)
    {
        _container = container;
        _levelPreloader = levelService;
        _storage = storage;

        _levelNumb = _levelPreloader.GetCurrentLevel();

        _player = player;
        _camera = camera;
    }

    private void Start()
    {
        var levelCfg = _storage.LevelConfig[_levelNumb];

        _currentLevelPrefab = _container.InstantiatePrefab(levelCfg.levelPrefab);
        Vector3 p = _currentLevelPrefab.GetComponent<LevelEntity>().StartPoint.position;
        _player.SetPosition(p);


        Vector3 c = _currentLevelPrefab.GetComponent<LevelEntity>().CameraPoint.position;
        _camera.SetPosition(c);
        
    }
}