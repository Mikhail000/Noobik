using UniRx;
using UnityEngine;
using Zenject;

public class NextlevelButton : MonoBehaviour
{
    private IMessagePublisher _publisher;

    [Inject]
    private void Construct(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    public void OnClicked()
    {
        _publisher.Publish(new LaunchNextLevelEvent());
        _publisher.Publish(new OnCloseNextLevelWindow());
    }
}
