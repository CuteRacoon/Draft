using UnityEditor;
using UnityEngine;

public class HideController : MonoBehaviour
{
    private Canvas canvas;
    private HideTriggerController trigger;
    private ForestActionController action;

    private bool isHidden = false;

    private bool playerInsideTrigger = false;

    private void OnEnable()
    {
        HideTriggerController.OnHideTriggerEnter += OnPlayerEnterHideZone;
        HideTriggerController.OnHideTriggerExit += OnPlayerExitHideZone;
    }
    private void Start()
    {
        action = FindAnyObjectByType<ForestActionController>();
    }
    private void OnPlayerEnterHideZone(HideTriggerController currentTrigger)
    {
        trigger = currentTrigger;
        playerInsideTrigger = true;

        canvas = trigger.GetComponentInChildren<Canvas>(true);
        canvas.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (playerInsideTrigger && Input.GetKeyDown(KeyCode.E) && trigger != null)
        {
            if (!isHidden)
            {
                isHidden = true;
                action.HidePerson(trigger.index+2);
                canvas.gameObject.SetActive(false);
            }
            else
            {
                isHidden = false;
                action.ShowPerson();
                canvas.gameObject.SetActive(true);
            }
        }
    }
    private void OnPlayerExitHideZone(HideTriggerController currentTrigger)
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
        }
        trigger = null;
        playerInsideTrigger = false;
    }
}
