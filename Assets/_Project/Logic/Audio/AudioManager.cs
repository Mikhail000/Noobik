using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    [SerializeField] private AudioSource musicSource;  // �������� ������� ������
    [SerializeField] private AudioClip[] backgroundTracks;  // ������ ������ ������� ������

    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxSource;  // �������� ��� ������ ��������
    [SerializeField] private AudioClip[] soundEffects;  // ������ �����-������ ��� ������ ��������

    private int currentTrackIndex = 0;

    private bool isMuted;

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);  // ��������� ������ ��� ����� �����
        //}
        //else
        //{
        //    Destroy(gameObject);  // ���������� ���������
        //}
    }

    private void Start()
    {
        PlayTrack(currentTrackIndex);  // ������������� ������ ����

        isMuted = false;
        musicSource.mute = false;
        sfxSource.mute = false;
    }

    private void Update()
    {
        // ���������, �������� �� ���� ������, � ���� ��, �� ��������� ���������
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

    // ������� ��� ��������������� ����� �� �������
    public void PlayTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= backgroundTracks.Length) return;

        musicSource.clip = backgroundTracks[trackIndex];
        musicSource.Play();
    }

    // ������� ��� ��������������� ���������� �����
    private void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % backgroundTracks.Length;  // ������� � ���������� �����
        PlayTrack(currentTrackIndex);  // ������������� ��������� ����
    }

    // ������� ��� ���������� ������ � ��������������� ������� ������
    public void PlayRandomTrack()
    {
        if (backgroundTracks.Length == 0) return;

        int randomIndex = Random.Range(0, backgroundTracks.Length);
        musicSource.clip = backgroundTracks[randomIndex];
        musicSource.Play();
    }

    // ������� ��� ��������������� ����������� ��������� �������
    public void PlaySoundEffect(int soundIndex)
    {
        if (soundIndex < 0 || soundIndex >= soundEffects.Length) return;

        sfxSource.PlayOneShot(soundEffects[soundIndex]);
    }

}
