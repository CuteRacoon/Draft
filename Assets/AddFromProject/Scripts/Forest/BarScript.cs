using UnityEngine;
using UnityEngine.UI;
using System;

public class BarScript : MonoBehaviour
{
    [Header("UI Settings")]
    private Image barImage;

    [Header("Timing Settings")]
    public float duration = 5f; // Время в секундах, за которое шкала должна исчезнуть

    private float timer = 0f;
    private bool isRunning = true;
    private bool isCriticalLevel = false;
    private GameObject parentObj;

    private Color normalColor;
    public Color criticalColor = new Color(0xF2 / 255f, 0xC9 / 255f, 0x89 / 255f); // F2C989
    public static event Action FuelLevelCritical;

    private void Awake()
    {
        Image[] allImages = GetComponentsInChildren<Image>(includeInactive: true);
        foreach (var img in allImages)
        {
            if (img.gameObject != this.gameObject)
            {
                barImage = img;
                break;
            }
        }

        if (barImage == null)
        {
            Debug.LogError("BarScript не нашел компонент Image внутри дочерних объектов!");
        }
        normalColor = barImage.color;
        ForestActionController.CanDisplayLampBar += EnableBar;
        ForestActionController.CannotDisplayLampBar += DisableBar;
        parentObj = transform.parent != null ? transform.parent.gameObject : null;
    }
    private void Start()
    {
        ResetBar();
    }
    private void EnableBar()
    {
        if (parentObj != null)
            parentObj.SetActive(true);
        ResumeBar();
    }

    private void DisableBar()
    {
        if (parentObj != null)
            parentObj.SetActive(false);
        StopBar();
    }

    private void Update()
    {
        if (!isRunning || barImage == null || duration <= 0f)
            return;

        timer += Time.deltaTime;
        float fill = Mathf.Clamp01(1f - (timer / duration));
        barImage.fillAmount = fill;
        if (fill <= 0.4f && !isCriticalLevel)
        {
            barImage.color = criticalColor;
            isCriticalLevel = true;
            FuelLevelCritical?.Invoke();
        }
        if (fill <= 0f)
        {
            isRunning = false;
            // можно вызвать событие или уведомление тут, если нужно
        }
    }

    public void ResetBar()
    {
        timer = 0f;
        if (barImage != null)
        {
            barImage.fillAmount = 1f;
            barImage.color = normalColor;
        }
        isCriticalLevel = false;
        isRunning = true;
    }
    public void StopBar()
    {
        isRunning = false;
    }
    public void ResumeBar()
    {
        isRunning = true;
    }
}

