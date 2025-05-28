using System;
using UnityEngine;

public class MonsterTriggerController : TriggerController
{
    public static event Action<MonsterTriggerController> OnMonsterTriggerEnter;
    public static event Action<MonsterTriggerController> OnMonsterTriggerExit;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player") && canInteract)
        {
            OnMonsterTriggerEnter?.Invoke(this);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player") && canInteract)
        {
            OnMonsterTriggerExit?.Invoke(this);
        }
    }
}
