using UniRx;
using UnityEngine;
using Zenject;

public class RestartButton : MonoBehaviour
{
    private IMessagePublisher _publisher;

    [Inject]
    private void Construct(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    public void OnClicked()
    {
        _publisher.Publish(new RestartEvent());
        _publisher.Publish(new OnCloseRestartWindow());
    }
}
