using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private bool isPassed;
    private Color _red = new Color(1.0f, 0f, 0.0f, 0.5f);
    private Color _green = new Color(0.0f, 1f, 0.0f, 0.5f);
    private LevelEntity _level;

    public void SetTrackOfCheckpoints(LevelEntity level)
    {
        this._level = level;

    }

    public void ResetCheckpoint()
    {
        meshRenderer.material.color = _red;
    }


    public Transform GetTranform()
    {
        return this.transform;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (!isPassed)
        {
            if (other.TryGetComponent<Player>(out Player p))
            {
                _level.PassedThroughCheckpoint(this);
                isPassed = true;
                meshRenderer.material.color = _green;
                Debug.Log("WALK THROUGH");
                SceneManager.LoadScene("Gameplay");

            }
        }
    }
}
