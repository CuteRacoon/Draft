using UnityEngine;

public class TriggerControllerOld : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        // Находим объект с GameController
        gameController = FindFirstObjectByType<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в триггер вошел игрок
        if (other.CompareTag("Player"))
        {
            // Устанавливаем флаг
            gameController.enterConditionIsReached = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        // Проверяем, что в триггер вошел игрок
        if (other.CompareTag("Player"))
        {
            // Устанавливаем флаг
            gameController.exitConditionIsReached = true;
        }
    }
}