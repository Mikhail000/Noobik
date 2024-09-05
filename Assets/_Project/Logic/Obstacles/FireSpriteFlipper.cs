using UnityEngine;

public class FireSpriteFlipper : MonoBehaviour
{
    public Texture[] textures; // ������ ������� ��� ��������
    public float frameRate = 0.1f; // �������� �������� (������� �� ����)

    private Material material; // ��������, � �������� ����������� ��������
    private int currentFrame;
    private float timer;

    void Start()
    {
        // �������� �������� �� MeshRenderer
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % textures.Length; // ��������� ����
            material.mainTexture = textures[currentFrame]; // ������ �������� ���������
        }
    }
}
