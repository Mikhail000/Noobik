using UnityEngine;

public class FireSpriteFlipper : MonoBehaviour
{
    public Texture[] textures; // Массив текстур для анимации
    public float frameRate = 0.1f; // Скорость анимации (секунды на кадр)

    private Material material; // Материал, к которому применяются текстуры
    private int currentFrame;
    private float timer;

    void Start()
    {
        // Получаем материал из MeshRenderer
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % textures.Length; // Обновляем кадр
            material.mainTexture = textures[currentFrame]; // Меняем текстуру материала
        }
    }
}
