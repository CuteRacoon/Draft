using UnityEngine;

public class ForestDialogueController : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private DialogueTriggerController trigger;

    private Canvas canvas;

    private bool playerInsideTrigger = false;
    private int currentTriggerIndex = -1;
    private bool dialogueInProcess = false;

    private GameObject dialogueObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        DialogueTriggerController.OnDialogueTriggerEnter += OnPlayerEnterDialogueZone;
        DialogueTriggerController.OnDialogueTriggerExit += OnPlayerExitDialogueZone;
    }
    private void OnPlayerEnterDialogueZone(DialogueTriggerController currentTrigger)
    {
        trigger = currentTrigger;
        playerInsideTrigger = true;
        currentTriggerIndex = trigger.index;

        canvas = trigger.gameObject.GetComponentInChildren<Canvas>(true);
        canvas.gameObject.SetActive(true);

        dialogueObject = trigger.transform.GetChild(0).gameObject;
    }
    private void OnPlayerExitDialogueZone(DialogueTriggerController currentTrigger)
    {
        trigger = null;
        playerInsideTrigger = false;

        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
        }
        currentTriggerIndex = -1;
        dialogueInProcess = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerInsideTrigger && Input.GetKeyDown(KeyCode.E) && trigger.canInteract && !dialogueInProcess)
        {
            // запуск диалога с печкой и переключение объектов
            ForestCameraManager.Instance.SwitchToDialogueCamera(currentTriggerIndex);
            canvas.gameObject.SetActive(false);
            trigger.canInteract = false;
            dialogueManager.PlayPartOfPlot("bake_dialogue");
            dialogueInProcess = true;
            ForestActionController.Instance.HidePerson();
            dialogueObject.SetActive(true);
        }
    }
}
