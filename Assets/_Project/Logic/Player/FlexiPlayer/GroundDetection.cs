using Unity.VisualScripting;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector3 offset;

    private bool _isGrounded;
    private bool _isHitted;
    private bool _isTriggered;
    public bool IsGrounded => _isGrounded;



    private void FixedUpdate()
    {
        _isGrounded = Summary();

        Debug.Log(_isTriggered);
    }


    private bool Summary()
    {
        return _isHitted || _isTriggered;
    }

    private void GroundCheck()
    {

        Vector3 boxCenter = transform.position + offset + transform.up * (boxSize.y / 2);

        _isHitted = Physics.BoxCast(boxCenter, boxSize, -transform.up, transform.rotation, 
            maxDistance, LayerMask.GetMask("Ground"));

    }

    private void OnDrawGizmos()
    {
        Vector3 boxCenter = transform.position + offset - Vector3.up * (boxSize.y / 2);

        Gizmos.color = _isGrounded ? Color.green : Color.red;

        Gizmos.DrawRay(boxCenter, -transform.up * maxDistance);

        Gizmos.DrawWireCube(boxCenter - transform.up * maxDistance, boxSize);
    }

    #region TriggerWork

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            //Debug.Log("Enter");
            _isTriggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            //Debug.Log("Stay");
            _isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            //Debug.Log("Exit");
            _isTriggered = false;
        }
    }

    #endregion

}
