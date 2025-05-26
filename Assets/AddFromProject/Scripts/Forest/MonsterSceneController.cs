using System.Collections;
using UnityEngine;

public class MonsterSceneController : MonoBehaviour
{
    private PlayerController playerController;
    private BasicBehaviour basicBehaviour; // Drag the player's BasicBehaviour here in the Inspector
    //private Animation girlMovementAnime;
    public GameObject girlComponent;
    public Transform stayingGirlParentObj;
    private Transform[] stayingGirls;

    private Transform stayingGirlTrans;
    private CameraManager cameraBehaviour;
    private CameraFollowScript cameraFollowScript;

    public float teleportDistanceZ = 20f;
    public float runDuration = 3f;

    private TriggerController currentTrigger;
    private int currentTriggerIndex = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = girlComponent.GetComponentInChildren<PlayerController>();
        basicBehaviour = girlComponent.GetComponentInChildren<BasicBehaviour>();
        //girlMovementAnime = girlComponent.GetComponentInChildren<Animation>();
        cameraBehaviour = FindAnyObjectByType<CameraManager>();
        cameraFollowScript = FindAnyObjectByType<CameraFollowScript>();

        int childCount = stayingGirlParentObj.childCount;
        stayingGirls = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Transform child = stayingGirlParentObj.GetChild(i);
            stayingGirls[i] = child;
            child.gameObject.SetActive(false);
        }
        TriggerController.OnPlayerEnterTrigger += HandlePlayerEnterTrigger;
    }
    void OnDestroy()
    {
        TriggerController.OnPlayerEnterTrigger -= HandlePlayerEnterTrigger;
    }
    void HandlePlayerEnterTrigger(TriggerController trigger)
    {
        currentTrigger = trigger;
        currentTriggerIndex = trigger.index;
    }
    public void StartRunning()
    {
        basicBehaviour.StartGirlInMonsterSceneAnimation(0.5f, 0.5f);
        basicBehaviour.LockTempBehaviour(basicBehaviour.GetHashCode());
        Camera monsterCamera = cameraBehaviour.GetCameraByIndex(currentTriggerIndex);
    }
    public void StopRunning()
    {
        basicBehaviour.EndGirlInMonsterSceneAnimation();
    }
    void TeleportPlayer()
    {
        if (currentTriggerIndex < 0 || currentTriggerIndex >= stayingGirls.Length-1)
        {
            Debug.LogError("������������ ������ stayingGirl ��� ������������.");
            return;
        }

        Transform targetGirl = stayingGirls[currentTriggerIndex-1];
        if (targetGirl == null)
        {
            Debug.LogError("�� ������� ����� stayingGirl �� �������.");
            return;
        }

        girlComponent.transform.position = targetGirl.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController != null && playerController.GetPlayerInside())
        {
            Debug.Log("�������� �������� � ��������");
            StartCoroutine(PlayGirlAnimation());
            playerController.SetPlayerInside(false);
        }
    }
    IEnumerator PlayGirlAnimation()
    {
        // 1.����������� ������
        cameraBehaviour.SwitchCamera(currentTriggerIndex);

        // 2. ������������� ������
        TeleportPlayer();

        // 3. ��������� ��� �� ������
        StartRunning();

        // 4. �������� ������������ �������� ������
        float waitTime = runDuration;
        Camera monsterCamera = cameraBehaviour.GetCameraByIndex(currentTriggerIndex);
        Animation cameraAnimation = monsterCamera.GetComponent<Animation>();
        if (cameraAnimation != null && cameraAnimation.clip != null)
        {
            waitTime = cameraAnimation.clip.length;
        }
        // 4. ���� ��������� �����
        yield return new WaitForSeconds(waitTime);

        // 5. ������������� ��� � ���������� ���������� ������
        StopRunning();
        cameraBehaviour.SwitchCamera(0);
        cameraFollowScript.SetTargetPosition();
        if (currentTrigger != null)
        {
            currentTrigger.canInteract = false;
            currentTrigger = null; // ������� ������
        }
        currentTriggerIndex = -1;
        yield return null;
    }
}
