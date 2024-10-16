using UnityEngine;

public class PistonAnimation : MonoBehaviour
{
    [SerializeField] private Rigidbody pistonRigidbody;

    [SerializeField] private float extendDistance;

    [SerializeField] private float pushDuration;
    [SerializeField] private float pullDuration;

    [SerializeField] private float extendPause;
    [SerializeField] private float reducePause;

    [SerializeField] private AnimationCurve _pullCurve;
    [SerializeField] private AnimationCurve _pushCurve;

    private Vector3 _initialLocalPosition;
    private float _elapsedTime;
    private bool _isPushing;
    private bool _isPulling;
    private bool _isPausingPushed;
    private bool _isPausingPulled;

    private void Start()
    {
        _initialLocalPosition = transform.localPosition;
        _elapsedTime = 0f;
        _isPushing = true;

        pistonRigidbody.centerOfMass = new Vector3(-0.55f, 0f, 0f);
    }

    private void Update()
    {
        if (_isPushing)
        {
            Push();
        }
        else if (_isPausingPushed)
        {
            PauseAtPush();
        }
        else if (_isPulling)
        {
            Pull();
        }
        else if (_isPausingPulled)
        {
            PauseAtPull();
        }
    }

    private void Push()
    {
        _elapsedTime += Time.deltaTime;
        float progress = _elapsedTime / pushDuration;
        if (progress >= 1f)
        {
            progress = 1f;
            _isPushing = false;
            _isPausingPushed = true;
            _elapsedTime = 0f;
        }
        float curveValue = _pushCurve.Evaluate(progress);

        // ��������� ����, ��������� � �������
        Vector3 targetLocalPosition = Vector3.Lerp(_initialLocalPosition, _initialLocalPosition + Vector3.up * extendDistance, curveValue);
        Vector3 targetWorldPosition = transform.parent.TransformPoint(targetLocalPosition); // ��������� -> �������
        pistonRigidbody.MovePosition(targetWorldPosition); // ���������� ����� Rigidbody
    }

    private void Pull()
    {
        _elapsedTime += Time.deltaTime;
        float progress = _elapsedTime / pullDuration;
        if (progress >= 1f)
        {
            progress = 1f;
            _isPulling = false;
            _isPausingPulled = true;
            _elapsedTime = 0f;
        }
        float curveValue = _pullCurve.Evaluate(progress);


        // ��������� ����, ��������� � �������
        Vector3 targetLocalPosition = Vector3.Lerp(_initialLocalPosition + Vector3.up * extendDistance, _initialLocalPosition, curveValue);
        Vector3 targetWorldPosition = transform.parent.TransformPoint(targetLocalPosition); // ��������� -> �������
        pistonRigidbody.MovePosition(targetWorldPosition); // ���������� ����� Rigidbody
    }

    private void PauseAtPush()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= extendPause)
        {
            _isPausingPushed = false;
            _isPulling = true;
            _elapsedTime = 0f;
        }
    }

    private void PauseAtPull()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= reducePause)
        {
            _isPausingPushed = false;
            _isPushing = true;
            _elapsedTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            pistonRigidbody = GetComponent<Rigidbody>();
        }

        // ���������� ���� ��� Gizmos
        Gizmos.color = Color.red;

        // ��������� ����� � ������ ����� Rigidbody
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);

        // ��������� ����� �� ������� ������� �� ������ �����
        Gizmos.DrawLine(transform.position, GetComponent<Rigidbody>().worldCenterOfMass);
    }

}
