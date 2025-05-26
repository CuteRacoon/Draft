using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // Ссылка на transform игрока
    public bool followPlayer = true; // Флаг следования за игроком
    private Vector3 offset; // Начальное смещение камеры относительно игрока

    void Start()
    {
        Vector3 startPosition = new Vector3(
               player.position.x,
               transform.position.y,
               transform.position.z
           );
        transform.position = startPosition;
        // Сохраняем начальное смещение между камерой и игроком
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (followPlayer)
        {
            // Если флаг включен, камера следует за игроком по X
            Vector3 newPosition = new Vector3(
                player.position.x + offset.x,
                transform.position.y,
                transform.position.z
            );
            transform.position = newPosition;
        }
    }
}