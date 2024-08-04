using System.Collections;
using UnityEngine;

public class BouncySurface : MonoBehaviour
{
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float height = 2.0f;
    [SerializeField] private float length = 5.0f;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float jumpForce = 10.0f;


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GET HIT");

        if (collision.gameObject.TryGetComponent(out Rigidbody rigidbody))
        {
            Debug.Log("GET HIT");
            StartCoroutine(Jump(rigidbody));
        }
    }

    private IEnumerator Jump(Rigidbody rigidbody)
    {
        float elapsedTime = 0;
        Vector3 startPosition = rigidbody.position;
        Vector3 targetPosition = startPosition + rigidbody.transform.forward * length;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float curveValue = jumpCurve.Evaluate(progress);

            Vector3 jumpOffset = new Vector3(0, curveValue * height, 0);
            Vector3 targetOffset = Vector3.Lerp(startPosition, targetPosition, progress);

            rigidbody.MovePosition(targetOffset + jumpOffset);
            rigidbody.velocity = Vector3.zero; // Reset velocity to ensure controlled movement

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange); // Apply force at the end
    }

}
