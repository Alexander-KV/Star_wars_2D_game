using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // насколько быстро едет фон 
    public float scrollSpeed = 2f;

    // ширина одного спрайта фона, нужна чтобы знать когда делать "прыжок"
    // замеряй в юнити и вставляй точное значение
    public float backgroundWidth = 20f;

    void Update()
    {
        // каждый кадр двигаем фон влево
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // если фон уехал достаточно далеко влево — телепортируем его вправо
        // так создаётся иллюзия бесконечного фона без дырок
        if (transform.position.x <= -backgroundWidth)
        {
            Vector3 pos = transform.position;
            pos.x += backgroundWidth * 2f; // прыгаем вправо на две ширины
            transform.position = pos;
        }
    }
}
