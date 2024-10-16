using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform CameraFollow;  // Camera position target
    [SerializeField] private Transform CameraArm;     // Camera rotation target
    [SerializeField] private float followSpeed = 5f;  // Speed for position transition
    [SerializeField] private float rotationSpeed = 5f;  // Speed for rotation transition
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera uiCamera;

    private Transform playerTransform;
    private bool isFollowingPlayer = false; // Track if camera is in follow mode
    private Coroutine cameraTransitionCoroutine; // To handle smooth transitions

    public static CameraController instance;

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
        UpdateCameraPosToGameState(Enum.GameState.Hall); 
    }

    private void LateUpdate()
    {
        if (isFollowingPlayer && playerTransform != null)
        {
            FollowPlayer();
        }
    }

    private void UpdateCameraPosToGameState(Enum.GameState gameState)
    {
        if (cameraTransitionCoroutine != null)
        {
            StopCoroutine(cameraTransitionCoroutine);
        }

        if (gameState == Enum.GameState.Zone1 || gameState == Enum.GameState.Zone2)
        {
            playerTransform = Player.Instance.transform;
            uiCamera.gameObject.SetActive(false);
            cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(CONSTANT_VALUE.OFFSETWHENINPVP.OSposition,CONSTANT_VALUE.OFFSETWHENINPVP.OSrotation,true));

        }
        else if (gameState == Enum.GameState.Hall)
        {
            CameraFollow.position = CONSTANT_VALUE.OFFSETWHENHALL.OSposition;
            CameraArm.rotation = Quaternion.Euler(CONSTANT_VALUE.OFFSETWHENHALL.OSrotation);
            isFollowingPlayer = false;
            uiCamera.gameObject.SetActive(true);
        }
        else if(gameState == Enum.GameState.Win)
        {         
            cameraTransitionCoroutine = StartCoroutine(SmoothTransitionToPlayer(CONSTANT_VALUE.OFFSETWHENHALL.OSposition, CONSTANT_VALUE.OFFSETWHENHALL.OSrotation,false));
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