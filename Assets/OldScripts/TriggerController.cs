using UnityEngine;

public class TriggerControllerOld : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        // ������� ������ � GameController
        gameController = FindFirstObjectByType<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // ���������, ��� � ������� ����� �����
        if (other.CompareTag("Player"))
        {
            // ������������� ����
            gameController.enterConditionIsReached = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        // ���������, ��� � ������� ����� �����
        if (other.CompareTag("Player"))
        {
            // ������������� ����
            gameController.exitConditionIsReached = true;
        }
    }
}