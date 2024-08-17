using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    private bool isPassed;
    private LevelEntity _level;

    public void SetTrackOfCheckpoints(LevelEntity level)
    {
        this._level = level;

        particles.Pause();
    }

    public void ResetCheckpoint()
    {
        particles.Stop();
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
                particles.Play();
                Debug.Log("WALK THROUGH");

            }
        }
    }
}
