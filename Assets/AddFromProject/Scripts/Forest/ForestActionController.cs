using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ForestActionController : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private LampController lampController;
    private PlayerController playerController;
    private CameraManager cameraManager;

    private bool lampState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraManager = FindAnyObjectByType<CameraManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        lampController = FindAnyObjectByType<LampController>();
        playerController = FindAnyObjectByType<PlayerController>();

        StartCoroutine(DialogueCoroutine());
    }

    private IEnumerator DialogueCoroutine()
    {
        GameEvents.RaiseCannotDisplayLampBar();
        yield return new WaitForSeconds(4f);
        dialogueManager.PlayPartOfPlot("girl_thoughts");
        while (dialogueManager.IsDialoguePlaying)
        {
            yield return null;
        }
        StartCoroutine(StartLampLearning());
    }
    public void HidePerson(int cameraIndex)
    {
        StartCoroutine(StartHideCoroutine(cameraIndex));
    }
    IEnumerator StartHideCoroutine(int cameraIndex)
    {
        lampState = lampController.IsLampOn;

        playerController.SetPlayerControl(false);
        playerController.SetMovement(false);
        lampController.DisableLampBar();
        lampController.StateCanUseLamp(false);
        lampController.ChangeLampState(false);

        cameraManager.SwitchCamera(4);
        Animation anime = cameraManager.GetCameraByIndex(4).GetComponent<Animation>();
        anime.enabled = true; // запускаем анимацию
        anime.Play("HideAnimation");

        // Ждём, пока длительность клипа не пройдёт
        yield return new WaitForSeconds(2.5f);
        cameraManager.SwitchCamera(cameraIndex);

    }
    public void ShowPerson()
    {
        cameraManager.SwitchCamera(0);
        lampController.StateCanUseLamp(true);
        playerController.SetPlayerControl(true);
        playerController.SetMovement(true);
        lampController.ChangeLampState(lampState);
    }
    private IEnumerator StartLampLearning()
    {
        playerController.SetPlayerControl(false);

        dialogueManager.LearningPanelText("Для того, чтобы зажечь лампу, нажмите F");
        lampController.StateCanUseLamp(true);
        yield return null;

        while (!lampController.IsLampOn)
        {
            yield return null;
        }
        lampController.StateCanUseLamp(false);

        // Подождать ещё 0.5 секунды перед скрытием
        yield return new WaitForSeconds(0.5f);
        dialogueManager.HideAllPanels();

        yield return new WaitForSeconds(1.5f);
        dialogueManager.LearningPanelText("Однако топливо в лампе не бесконечно. Шкала топлива подскажет");
        GameEvents.RaiseCanDisplayLampBar();
        lampController.ChangeLampState(true);

        yield return new WaitForSeconds(0.1f);

        lampController.StopLampBar();

        yield return new WaitForSeconds(4f);
        dialogueManager.HideAllPanels();

        lampController.StateCanUseLamp(true);

        lampController.ResumeLampBar();
        lampController.LearningCompleted();

        playerController.SetPlayerControl(true);
    }
}
