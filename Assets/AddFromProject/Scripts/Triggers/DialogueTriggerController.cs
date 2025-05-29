using System;
using UnityEngine;

public class DialogueTriggerController : TriggerController
{
    public static event Action<DialogueTriggerController> OnDialogueTriggerEnter;
    public static event Action<DialogueTriggerController> OnDialogueTriggerExit;
    protected override void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);

        if (other.CompareTag("Player") && canInteract)
        {
            OnDialogueTriggerEnter?.Invoke(this);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        //base.OnTriggerExit(other);

        if (other.CompareTag("Player") && canInteract)
        {
            OnDialogueTriggerExit?.Invoke(this);
        }
    }
}
