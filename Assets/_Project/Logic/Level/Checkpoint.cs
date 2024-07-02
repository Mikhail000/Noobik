using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
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
        if (other.TryGetComponent<Player>(out Player player))
        {
            _level.PassedThroughCheckpoint(this);
            ChangeColor();
            meshRenderer.material.color = _green;
        }
    }

    private void ChangeColor()
    {
        
    }
}
