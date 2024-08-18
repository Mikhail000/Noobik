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


    private void FixedUpdate()
    {
        if (transform.position.y < -10) Die();
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
        this.transform.rotation = Quaternion.identity;
    }

    public void Die()
    {
        _publisher.Publish(new DieMessage());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Deadly")
        {
            _publisher.Publish(new DieMessage());
            Debug.Log("GET DEAD");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("GET DEAD");

        if (other.tag == "Deadly")
        {
            _publisher.Publish(new DieMessage());
            Debug.Log("GET DEAD");
        }

        if (other.CompareTag("Deadly"))
        {
            _publisher.Publish(new DieMessage());
            Debug.Log("GET DEAD");
        }
    }

}


