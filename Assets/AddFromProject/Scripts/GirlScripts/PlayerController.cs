using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private BasicBehaviour basicBehaviour;
    private MoveBehaviour moveBehaviour;
    private bool canMove = true;
    private bool playerInsideOfTrigger = false;
    private bool isPositionLocked = false;

    public static PlayerController Instance { get ; private set; }
    private void OnEnable()
    {
        GameEvents.NeedToStopSprint += HandleSprintStop;
        GameEvents.NeedToStartSprint += HandleSprintStart;
    }

    public void LockPosition(bool state)
    {
        isPositionLocked = state;
    }
    public bool IsPositionLocked()
    {
        return isPositionLocked;
    }

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        animator = GetComponent<Animator>();
        basicBehaviour = gameObject.GetComponent<BasicBehaviour>();
        moveBehaviour = gameObject.GetComponent<MoveBehaviour>();
    }
    public void SetSprintEnable(bool state)
    {
        basicBehaviour.SetSprintAllowed(state); // если state == true, то запрещаем спринт
    }
    public void SetNewMovementSpeeds(float newWalkSpeed, float newRunSpeed, float newSprintSpeed)
    {
        moveBehaviour.walkSpeed = newWalkSpeed;
        moveBehaviour.runSpeed = newRunSpeed;
        moveBehaviour.sprintSpeed = newSprintSpeed;
    }
    private void HandleSprintStop()
    {
        SetSprintEnable(false);
    }
    private void HandleSprintStart()
    {
        SetSprintEnable(true);
    }
    public bool GetPlayerInside()
    {
        return playerInsideOfTrigger;
    }
    public void SetPlayerInside(bool state)
    {
        playerInsideOfTrigger = state;
    }
    public void SetMovement(bool state)
    {
        canMove = state;
        animator.applyRootMotion = state; // Инвертируем состояние
        animator.enabled = state;
        gameObject.GetComponent<BasicBehaviour>().enabled = state;
        gameObject.SetActive(state);
    }
    public void SetPlayerControl(bool state)
    {
        if (!state)
        {
            basicBehaviour.DisablePlayerControl();
        }
        else
        {
             basicBehaviour.EnablePlayerControl();
        }
    }
    public void SetTransform(Transform trans)
    {
        transform.localPosition = trans.position; // учитываем локальную позицию
        transform.rotation = trans.rotation;
        BasicBehaviour basic = gameObject.GetComponent<BasicBehaviour>();
        basic.ClearLastDirection();
    }

}