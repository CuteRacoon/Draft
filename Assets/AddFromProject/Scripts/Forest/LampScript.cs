using UnityEngine;
using System.Collections;

public class LampController : MonoBehaviour
{
    [Header("Настройки лампы")]
    public static bool CanUseLamp = true;              // Можно ли управлять лампой
    public GameObject lampObject;               // Сюда перетащить объект с лампой в инспекторе
    private Light lampLight;                   // Сам компонент Light
    private Material lampMaterial;

    public float fadeDuration = 3f;           // Время затухания/включения
    public float maxIntensity = 1f;           // Максимальная яркость
    private Color initialEmissionColor;

    private bool isLampOn = false;
    private Coroutine fadeCoroutine;
    void Start()
    {
        if (lampObject != null)
        {
            lampLight = lampObject.GetComponentInChildren<Light>();

            Renderer rend = lampObject.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                lampMaterial = rend.material;
                lampMaterial.EnableKeyword("_EMISSION");

                // Сохраняем исходный цвет
                initialEmissionColor = lampMaterial.GetColor("_EmissionColor");

                // Начальное состояние — лампа выключена
                lampMaterial.SetColor("_EmissionColor", Color.black);
            }

            if (lampLight != null)
            {
                lampLight.intensity = 0f;
                lampObject.SetActive(false);
            }
        }
        BarScript.FuelLevelCritical += OnFuelCritical;
        CanUseLamp = false;
    }
    private void OnDisable()
    {
        BarScript.FuelLevelCritical -= OnFuelCritical;
    }
    private void OnFuelCritical()
    {
        // Реакция на критический уровень топлива
        if (lampLight != null)
        {
            lampLight.intensity = maxIntensity * 0.5f;
        }
    }

    void Update()
    {
        if (CanUseLamp && Input.GetKeyDown(KeyCode.F))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            StartCoroutine(ToggleLamp());
        }
    }

    IEnumerator ToggleLamp()
    {
        if (lampObject == null || lampLight == null || lampMaterial == null)
        {
            Debug.LogWarning("LampController: Не все компоненты назначены.");
            yield break;
        }

        isLampOn = !isLampOn;
        float startIntensity = lampLight.intensity;
        float endIntensity = isLampOn ? maxIntensity : 0f;

        Color startEmission = lampMaterial.GetColor("_EmissionColor");
        Color endEmission = isLampOn ? initialEmissionColor : Color.black;

        float timer = 0f;

        if (isLampOn)
        {
            lampObject.SetActive(true); // Включаем сразу
        }

        // Плавное изменение яркости
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            lampLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
            lampMaterial.SetColor("_EmissionColor", Color.Lerp(startEmission, endEmission, t));
            yield return null;
        }

        lampLight.intensity = endIntensity;
        lampMaterial.SetColor("_EmissionColor", endEmission);

        // Если выключаем — задержка и деактивация объекта
        if (!isLampOn)
        {
            lampObject.SetActive(false);
        }
    }
}
