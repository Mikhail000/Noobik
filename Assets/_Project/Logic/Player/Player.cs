using UniRx;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private SlimePhysicJump jump;
    [SerializeField] private WheelGroundChecker groundChecker;

    [Header("Wheels")]

    [SerializeField] private WheelCollider frontWheel;
    [SerializeField] private WheelCollider rearWheel;

    [Header("Wheels params")]

    [SerializeField] private float normalForwardStiffnessFrontWheel;
    [SerializeField] private float normalSideStiffnessFrontWheel;

    [SerializeField] private float normalForwardStiffnessRearWheel;
    [SerializeField] private float normalSideStiffnessRearWheel;

    [SerializeField] private float slipperyForwardStiffnessFrontWheel;
    [SerializeField] private float slipperySideStiffnessFrontWheel;

    [SerializeField] private float slipperyForwardStiffnessRearWheel;
    [SerializeField] private float slipperySideStiffnessRearWheel;


    private IMessagePublisher _publisher;
    private CompositeDisposable _disposable;

    private Vector3 _playerTransform;

    private string _surfaceName;

    private WheelFrictionCurve _frontForwardFrictionCurve;
    private WheelFrictionCurve _frontSideFrictionCurve;

    private WheelFrictionCurve _rearForwardFrictionCurve;
    private WheelFrictionCurve _rearSideFrictionCurve;

    [Inject]
    private void Construct(IMessagePublisher publisher)
    {
        _publisher = publisher;
        _disposable = new();
    }


    private void FixedUpdate()
    {
        if (transform.position.y < -10) Die();  

        _surfaceName = groundChecker.GetSurfaceType();

        SwitchStiffness();

    }

    public void SetPosition(Vector3 pos)
    {
        rigidbody.velocity = Vector3.zero;

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
            Die();
            Debug.Log("GET DEAD");
        }


        if (other.tag == "Slime")
        {
            jump.Bounc(other, rigidbody.velocity.normalized);
        }

    }

    private void OnParticleCollision(GameObject other)
    {

        if (other.tag == "Deadly")
        {
            Die();
        }

        if (other.CompareTag("Deadly"))
        {
            Die();
        }
    }


    private void SwitchStiffness()
    {
        if (_surfaceName == "Slippery")
        {
            _frontForwardFrictionCurve = frontWheel.forwardFriction;
            _frontSideFrictionCurve = frontWheel.sidewaysFriction;

            _rearForwardFrictionCurve = rearWheel.forwardFriction;
            _rearSideFrictionCurve = rearWheel.sidewaysFriction;

            _frontForwardFrictionCurve.stiffness = slipperyForwardStiffnessFrontWheel;
            _frontSideFrictionCurve.stiffness = slipperySideStiffnessFrontWheel;

            _rearForwardFrictionCurve.stiffness = slipperyForwardStiffnessRearWheel;
            _rearSideFrictionCurve.stiffness = slipperySideStiffnessRearWheel;

            frontWheel.forwardFriction = _frontForwardFrictionCurve;
            frontWheel.sidewaysFriction = _frontSideFrictionCurve;

            rearWheel.forwardFriction = _rearForwardFrictionCurve;
            rearWheel.sidewaysFriction = _rearSideFrictionCurve;
        }

        if (_surfaceName == "Ground")
        {
            _frontForwardFrictionCurve = frontWheel.forwardFriction;
            _frontSideFrictionCurve = frontWheel.sidewaysFriction;

            _rearForwardFrictionCurve = rearWheel.forwardFriction;
            _rearSideFrictionCurve = rearWheel.sidewaysFriction;

            _frontForwardFrictionCurve.stiffness = normalForwardStiffnessFrontWheel;
            _frontSideFrictionCurve.stiffness = normalSideStiffnessFrontWheel;

            _rearForwardFrictionCurve.stiffness = normalForwardStiffnessRearWheel;
            _rearSideFrictionCurve.stiffness = normalSideStiffnessRearWheel;

            frontWheel.forwardFriction = _frontForwardFrictionCurve;
            frontWheel.sidewaysFriction = _frontSideFrictionCurve;

            rearWheel.forwardFriction = _rearForwardFrictionCurve;
            rearWheel.sidewaysFriction = _rearSideFrictionCurve;
        }
    }

}


