using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    [SerializeField] private AudioSource musicSource;  // Источник фоновой музыки
    [SerializeField] private AudioClip[] backgroundTracks;  // Массив треков фоновой музыки

    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxSource;  // Источник для звуков эффектов
    [SerializeField] private AudioClip[] soundEffects;  // Массив аудио-клипов для звуков эффектов

    private int currentTrackIndex = 0;

    private bool isMuted;

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);  // Сохраняем объект при смене сцены
        //}
        //else
        //{
        //    Destroy(gameObject);  // Уничтожаем дубликаты
        //}
    }

    private void Start()
    {
        PlayTrack(currentTrackIndex);  // Воспроизводим первый трек

        isMuted = false;
        musicSource.mute = false;
        sfxSource.mute = false;
    }

    private void Update()
    {
        // Проверяем, закончил ли трек играть, и если да, то запускаем следующий
        if (!musicSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    public void Mute()
    {

        if (isMuted)
        {
            isMuted = false;
            musicSource.mute = false;
            sfxSource.mute = false;

            Debug.Log("SOUNDS ON");
        }
        else 
        {
            isMuted = true;
            musicSource.mute = true;
            sfxSource.mute = true;

            Debug.Log("SOUNDS OFF");
        }
    }

    // Функция для воспроизведения трека по индексу
    public void PlayTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= backgroundTracks.Length) return;

        musicSource.clip = backgroundTracks[trackIndex];
        musicSource.Play();
    }

    // Функция для воспроизведения следующего трека
    private void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % backgroundTracks.Length;  // Переход к следующему треку
        PlayTrack(currentTrackIndex);  // Воспроизводим следующий трек
    }

    // Функция для случайного выбора и воспроизведения фоновой музыки
    public void PlayRandomTrack()
    {
        if (backgroundTracks.Length == 0) return;

        int randomIndex = Random.Range(0, backgroundTracks.Length);
        musicSource.clip = backgroundTracks[randomIndex];
        musicSource.Play();
    }

    // Функция для воспроизведения конкретного звукового эффекта
    public void PlaySoundEffect(int soundIndex)
    {
        if (soundIndex < 0 || soundIndex >= soundEffects.Length) return;

        sfxSource.PlayOneShot(soundEffects[soundIndex]);
    }

}
