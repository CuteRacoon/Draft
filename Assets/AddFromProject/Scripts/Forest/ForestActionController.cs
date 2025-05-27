using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ForestActionController : MonoBehaviour
{
    private DialogueManager dialogueController;
    public static event Action CanDisplayLampBar;
    public static event Action CannotDisplayLampBar;

    public static void RaiseCanDisplayLampBar() => CanDisplayLampBar?.Invoke();
    public static void RaiseCannotDisplayLampBar() => CannotDisplayLampBar?.Invoke();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueController = FindAnyObjectByType<DialogueManager>();
        StartCoroutine(DialogueCoroutine());
    }

    private IEnumerator DialogueCoroutine()
    {
        RaiseCannotDisplayLampBar();
        yield return new WaitForSeconds(4f);
        dialogueController.PlayPartOfPlot("girl_thoughts");
        while (dialogueController.IsDialoguePlaying)
        {
            yield return null;
        }
        StartCoroutine(StartLampLearning());
    }
    private IEnumerator StartLampLearning()
    {
        dialogueController.LearningPanelText("��� ����, ����� ������ �����, ������� F");
        LampController.CanUseLamp = true;
        yield return null;

        // ���, ���� ����� ����� �����
        bool lighted = false;
        while (!lighted)
        {
            // ��������� ������� ������� F
            if (Input.GetKeyDown(KeyCode.F))
                lighted = true;
            yield return null;
        }
        // ��������� ��� 0.5 ������� ����� ��������
        yield return new WaitForSeconds(0.5f);
        dialogueController.HideAllPanels();

        yield return new WaitForSeconds(3f);
        dialogueController.LearningPanelText("������ ������� � ����� �� ����������. ����� ������� ���������");
        RaiseCanDisplayLampBar();
        yield return new WaitForSeconds(4f);
        dialogueController.HideAllPanels();
    }
}
