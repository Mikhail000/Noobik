using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Image image;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    private IMessagePublisher _publisher;

    [Inject]
    private void Construct(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    private void Start()
    {
        image.sprite = soundOn;
    }

    public void OnClicked()
    {
        if (image.sprite == soundOn)
        {
            image.sprite = soundOff;
            audioManager.Mute();
        }
        else if(image.sprite == soundOff)
        {
            image.sprite = soundOn;
            audioManager.Mute();
        }
    }
}
