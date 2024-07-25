using UniRx;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    private IMessagePublisher _publisher;
    private CompositeDisposable _disposable;

    private Vector3 _playerTransform;

    [Inject]
    private void Construct(IMessagePublisher publisher)
    {
        _publisher = publisher;
        _disposable = new();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -10) _publisher.Publish(new DieMessage());
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
        this.transform.rotation = Quaternion.identity;
    }
}


