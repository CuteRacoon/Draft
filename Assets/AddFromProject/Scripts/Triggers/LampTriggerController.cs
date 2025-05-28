using System;
using UnityEngine;

public class LampTriggerController : TriggerController
{
    public static event Action<LampTriggerController> OnLampTriggerEnter;
    public static event Action<LampTriggerController> OnLampTriggerExit;

    protected override void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);

        if (other.CompareTag("Player") && canInteract)
        {
            OnLampTriggerEnter?.Invoke(this);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        //base.OnTriggerExit(other);

        if (other.CompareTag("Player") && canInteract)
        {
            OnLampTriggerExit?.Invoke(this);
        }
    }
}
