using Unity.VisualScripting;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void OnCollisionStay(Collision collision)
    {
        // ѕровер€ем, соприкасаетс€ ли велосипед с землей (слой "Ground")
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ≈сли велосипед перестает соприкасатьс€ с землей
        if (collision.gameObject.tag == "Ground")
        {
            _isGrounded = false;
        }
    }
}
