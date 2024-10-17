using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform CameraFollow;  
    [SerializeField] private Transform CameraArm;     
    [SerializeField] private float followSpeed = 5f;  
    [SerializeField] private float rotationSpeed = 5f;  
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera uiCamera;

    private Transform playerTransform;
    private bool isFollowingPlayer = false;
    private Coroutine cameraTransitionCoroutine; 

    private static CameraController instance;
    public static CameraController Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameManager.Instance.onStateChange.AddListener(UpdateCameraPosToGameState);
    }

    private void LateUpdate()
    {
        if (isFollowingPlayer && playerTransform != null)
        {
            FollowPlayer();
        }
    }
    private void Start()
    {
        UpdateCameraPosToGameState(Enum.GameState.Hall);
    }

    private void UpdateCameraPosToGameState(Enum.GameState gameState)
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
                if(Player.Instance !=null)
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
        else if (gameState == Enum.GameState.Zone1 || gameState == Enum.GameState.Zone2)
        {
            if (Player.Instance != null)
            {
                playerTransform = Player.Instance.transform;
            }
            uiCamera.gameObject.SetActive(false); 
            cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(
                CONSTANT_VALUE.OFFSETWHENINPVP.OSposition,
                CONSTANT_VALUE.OFFSETWHENINPVP.OSrotation,
                true 
            ));
        }
        else if (gameState == Enum.GameState.Win)
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
        float duration =0.5f;  
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

    private void FollowPlayer()
    {
        Vector3 targetPosition = playerTransform.position + CONSTANT_VALUE.OFFSETWHENINPVP.OSposition;
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