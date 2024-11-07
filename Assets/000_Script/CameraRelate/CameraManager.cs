using System.Collections;
using System.Linq;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform CameraFollow;
    [SerializeField] private Transform CameraArm;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera uiCamera;

    [SerializeField] private float cameraDistanceScaler = 1f;
    [SerializeField] private float cameraDistanceScalerZC = 1f;
    private Transform playerTransform;
    private bool isFollowingPlayer = false;
    private Coroutine cameraTransitionCoroutine;

    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }

    private Vector3 posForCam;

    public void InitCameraManager()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (GameManager.Instance.CurrentGameState == Enum.GameState.HallState)
        {
            posForCam = CONSTANT_VALUE.OFFSETWHENHALL.OSposition;
        }
        else if(GameManager.Instance.CurrentGameState == Enum.GameState.ZombieState)
        {
            posForCam = CONSTANT_VALUE.OFFSETWHENINZC.OSposition;
        }
    }

    private void OnEnable()
    {
 

    }



    public void CameraLateFollow()
    {
        if (isFollowingPlayer && playerTransform != null)
        {
            FollowPlayer();
        }
    }

    //private void Start()
    //{
    //    GameManager.Instance.onStateChange.AddListener(UpdateCameraPosToGameState);
    //    UpdateCameraPosToGameState(GameManager.Instance.CurrentGameState, GameManager.Instance.CurrentInGameState);
    //    //if (Player.Instance != null)
    //    //{
    //    //    ZCAttributeController zC= Player.Instance.GetComponent<ActorAtributeController>() as ZCAttributeController;
    //    //    if(zC != null)
    //    //    {
    //    //        zC.onUpgradeStat.AddListener(AdjustCameraDistanceZC);
    //    //        ZCStatPlayer zCStatPlayer = zC.Stats.FirstOrDefault(stat => stat.Type == Enum.ZCUpgradeType.CircleRange);
    //    //        for (int i=0;i<zCStatPlayer.HowMuchUpgrade;i=i+10)
    //    //        {
    //    //            AdjustCameraDistanceZC(zCStatPlayer);
    //    //        }
    //    //    }
    //    //}
    //}

    private void UpdateCameraPosToGameState(Enum.GameState gameState, Enum.GameplayState inGameState)
    {
        if (cameraTransitionCoroutine != null)
        {
            StopCoroutine(cameraTransitionCoroutine);
        }

        if (gameState == Enum.GameState.SkinShop)
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
                cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(
                CONSTANT_VALUE.OFFSETWHENINSKINSHOP.OSposition,
                CONSTANT_VALUE.OFFSETWHENINSKINSHOP.OSrotation,
                false
            ));
        }
        else if (gameState == Enum.GameState.Hall)
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
            uiCamera.gameObject.SetActive(true);
            cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(
                CONSTANT_VALUE.OFFSETWHENHALL.OSposition,
                CONSTANT_VALUE.OFFSETWHENHALL.OSrotation,
                false
            ));
        }
        else if (gameState == Enum.GameState.Ingame && inGameState == Enum.GameplayState.PVE)
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
                uiCamera.gameObject.SetActive(false);
                cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(
                posForCam,
                CONSTANT_VALUE.OFFSETWHENINPVP.OSrotation,
                true
            ));
        }
        else if(gameState == Enum.GameState.Ingame && inGameState == Enum.GameplayState.Zombie)
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
            uiCamera.gameObject.SetActive(false);
            cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(
            posForCam,
            CONSTANT_VALUE.OFFSETWHENINPVP.OSrotation,
            true
        ));
        }
        else if (gameState == Enum.GameState.Win && inGameState == Enum.GameplayState.PVE)
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
                cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(
                CONSTANT_VALUE.OFFSETWHENHALL.OSposition,
                CONSTANT_VALUE.OFFSETWHENHALL.OSrotation,
                false
            ));
        }
    }

    private IEnumerator SmoothTransitionToPlayer(Vector3 pos, Vector3 rot, bool followPlayerAfterThis)
    {
        isFollowingPlayer = false;
        Vector3 targetPosition = playerTransform.position + pos;
        Quaternion targetRotation = Quaternion.Euler(rot);
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector3 initialPosition = CameraFollow.position;
        Quaternion initialRotation = CameraArm.rotation;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            CameraFollow.position = Vector3.Lerp(initialPosition, targetPosition, t);
            CameraArm.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            yield return null;
        }

        CameraFollow.position = targetPosition;
        CameraArm.rotation = targetRotation;
        isFollowingPlayer = followPlayerAfterThis;
    }

    public void AdjustCameraDistance()
    { 
        Vector3 newOffset = posForCam * cameraDistanceScaler;
        posForCam = newOffset;
    }
    public void AdjustCameraDistanceZC(ZCStatPlayer statPlayer)
    {
        if (statPlayer.Type == Enum.ZCUpgradeType.CircleRange)
        {
            Vector3 newOffset = posForCam * cameraDistanceScalerZC;
            posForCam = newOffset;
        }
    }
    private void FollowPlayer()
    {
        Vector3 targetPosition = playerTransform.position + posForCam;
        CameraFollow.position = targetPosition;
    }
}
public class OFFSETFORCAMERA
{
    public Vector3 OSposition;
    public Vector3 OSrotation;

    public OFFSETFORCAMERA(Vector3 position, Vector3 rotation)
    {
        this.OSposition = position;
        this.OSrotation = rotation;
    }
}