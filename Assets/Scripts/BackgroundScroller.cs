using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // —корость движени€ фона (подбери на свой вкус)
    public float scrollSpeed = 2f;

    // Ўирина одной картинки фона (замерь и вставь)
    public float backgroundWidth = 20f;

    void Update()
    {
        // ƒвигаем фон влево каждый кадр
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // ≈сли фон ушЄл достаточно далеко влево Ч перемещаем его вправо за второй фон
        if (transform.position.x <= -backgroundWidth)
        {
            Vector3 pos = transform.position;
            pos.x += backgroundWidth * 2f;
            transform.position = pos;
        }
    }
}