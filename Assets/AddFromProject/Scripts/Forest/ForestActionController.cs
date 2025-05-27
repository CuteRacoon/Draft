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
        dialogueController.LearningPanelText("Для того, чтобы зажечь лампу, нажмите F");
        LampController.CanUseLamp = true;
        yield return null;

        // Ждём, пока игрок зажжёт лампу
        bool lighted = false;
        while (!lighted)
        {
            // Проверяем нажатие клавиши F
            if (Input.GetKeyDown(KeyCode.F))
                lighted = true;
            yield return null;
        }
        // Подождать ещё 0.5 секунды перед скрытием
        yield return new WaitForSeconds(0.5f);
        dialogueController.HideAllPanels();

        yield return new WaitForSeconds(3f);
        dialogueController.LearningPanelText("Однако топливо в лампе не бесконечно. Шкала топлива подскажет");
        RaiseCanDisplayLampBar();
        yield return new WaitForSeconds(4f);
        dialogueController.HideAllPanels();
    }
}
