using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text girlText;
    public Text ovenText;
    public GameObject textPanel;
    public GameObject hintPanel;
    public GameObject lights;
    public GameObject steamEffect;
    public GameObject gameProcess;
    public GameObject dialogueProcess;

    public bool enterConditionIsReached = false;
    public bool exitConditionIsReached = false;

    private bool dialogueStarted = false;
    private bool hintPanelIsActive = false;

    void Start()
    {
        // отключаем всё, что не должно быть включено
        textPanel.SetActive(false);
        hintPanel.SetActive(false);
    }

    void Update()
    {
        // выводим подсказку на экран
        if (enterConditionIsReached && !dialogueStarted && !hintPanelIsActive)
        {
            hintPanel.SetActive(true);
            hintPanelIsActive = true;
 
            steamEffect.SetActive(true);
        }
        // при выходе из триггерной области
        if (exitConditionIsReached)
        {
            hintPanel.SetActive(false);
            hintPanelIsActive = false;
            exitConditionIsReached = false;
            enterConditionIsReached = false;
            steamEffect.SetActive(false);
        }
        // если игрок иниицировал диалог
        if (Input.GetKeyDown(KeyCode.E) && enterConditionIsReached && !dialogueStarted)
        {
            dialogueStarted = true;
            setDialogueScene();
        }

        // Когда диалог окончен, игрок снова может двигаться
        if (!dialogueStarted)
        {
            lights.SetActive(false);
            setDialogueScene();
            hintPanelIsActive = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void setDialogueScene()
    {
        if (dialogueStarted)
        {
            gameProcess.SetActive(false);
            dialogueProcess.SetActive(true);
            hintPanel.SetActive(false);

            StartCoroutine(WaitForDialogue());
        }
        else
        {
            textPanel.SetActive(false);
        }
        gameProcess.SetActive(!dialogueStarted);
        dialogueProcess.SetActive(dialogueStarted);
    }

    private IEnumerator WaitForDialogue() // запускаем диалог с печкой
    {
        girlText.text = ovenText.text = null;
        yield return new WaitForSeconds(1f);
        lights.SetActive(true);
        yield return new WaitForSeconds(1f);

        textPanel.SetActive(true);

        yield return StartCoroutine(ShowDialogue("Здравствуй, Алёнушка!", 4f, 0));

        yield return StartCoroutine(ShowDialogue("- Батюшки... грибов переела, похоже", 4f, 1));
        yield return StartCoroutine(ShowDialogue("Скушай пиро...", 1.5f, 0));

        yield return StartCoroutine(ShowDialogue("- Жуть какая... Привидится же говорящая печка...", 5f, 1));
        dialogueStarted = false;
    }
    private IEnumerator ShowDialogue(string text, float delay, int speaker)
    {
        if (speaker == 0)
        {
            girlText.text = null;
            ovenText.text = text;
        }
        else if (speaker == 1)
        {
            girlText.text = text;
            ovenText.text = null;
        }

        yield return new WaitForSeconds(delay); // Ждем указанное время
    }
}
