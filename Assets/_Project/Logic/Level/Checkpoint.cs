using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    [SerializeField] private ParticleSystem particles;

    private Material material;

    private bool isPassed;
    private LevelEntity _level;

    public void SetTrackOfCheckpoints(LevelEntity level)
    {
        this._level = level;

        material = mesh.material;   

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
                EnableMaterilEmmision();
                //particles.Play();
                Debug.Log("WALK THROUGH");

            }
        }
    }

    private void EnableMaterilEmmision() => material.EnableKeyword("_EMISSION");
    
}
