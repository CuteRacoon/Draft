using UnityEngine;

public class LampController : MonoBehaviour
{
    [SerializeField] private BarScript barScript;

    private bool playerInsideTrigger = false;
    private LampScript lampScript;

    private bool previousCriticalState = false;
    private bool learningCompleted = false;
    private Canvas canvas;
    public bool IsLampOn => lampScript.IsLampOn;

    private LampTriggerController trigger;
    public static LampController Instance { get; private set; }
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        lampScript = FindAnyObjectByType<LampScript>();
    }
    public void LearningCompleted()
    {
        learningCompleted = true;
    }
    private void OnEnable()
    {
        LampTriggerController.OnLampTriggerEnter += OnPlayerEnterLampZone;
        LampTriggerController.OnLampTriggerExit += OnPlayerExitLampZone;

        GameEvents.CanDisplayLampBar += EnableLampBar;
        GameEvents.CannotDisplayLampBar += DisableLampBar;
        GameEvents.LampStateChanging += HandleLampStateChanging;
        GameEvents.BarIsNull += HandleBarNull;
    }
    private void HandleBarNull() // при достижении 0 заряда - выключить лампу
    {
        SetLampState(false);
        StateCanUseLamp(false);
    }

    private void OnDisable()
    {
        LampTriggerController.OnLampTriggerEnter -= OnPlayerEnterLampZone;
        LampTriggerController.OnLampTriggerExit -= OnPlayerExitLampZone;
    }
    private void OnPlayerEnterLampZone(LampTriggerController currentTrigger)
    {
        trigger = currentTrigger;
        playerInsideTrigger = true;

        canvas = trigger.GetComponentInChildren<Canvas>(true);
        canvas.gameObject.SetActive(true);
    }
   
    private void OnPlayerExitLampZone(LampTriggerController currentTrigger)
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
        }
        trigger = null;
        playerInsideTrigger = false;
    }
    private void HandleLampStateChanging()
    {
        if (learningCompleted)
        {
            if(!lampScript.IsLampOn)
            {
                DisableLampBar();
            }
            else EnableLampBar();
        }
    }
    private void Update()
    {
        if (playerInsideTrigger && Input.GetKeyDown(KeyCode.E) && trigger.canInteract)
        {
            ResetBarAndFuel();
            canvas.gameObject.SetActive(false);
            trigger.canInteract = false;
        }

        // Проверка перехода в критическое состояние
        bool isNowCritical = barScript.IsCriticalLevel;

        if (isNowCritical && !previousCriticalState)
        {
            // Критический уровень впервые достигнут
            lampScript?.OnFuelCritical();
        }
        previousCriticalState = isNowCritical;
    }
    public void SetLampState(bool state)
    {
        lampScript.SetLampState(state);
    }
    public void ResetBarAndFuel()
    {
        barScript.ResetBar();
        StateCanUseLamp(true);
        lampScript.ResetIntensity();
        SetLampState(true); // включить лампу
    }
    public void DisableLampBar()
    {
        barScript.DisableBar();
    }
    public void EnableLampBar()
    {
        barScript.EnableBar();
    }
    public void StopLampBar()
    {
        barScript.StopBar();
    }
    public void ResumeLampBar()
    {
        barScript.ResumeBar();
    }
    public void StateCanUseLamp(bool state)
    {
        lampScript.CanUseLamp = state;
    }
}
