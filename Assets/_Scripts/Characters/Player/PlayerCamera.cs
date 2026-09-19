using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1;  // ther bigger this number is, the longer it takes the camera to reach its position during movement
    [SerializeField] private float upAndDownRoationSpeed = 220;
    [SerializeField] private float leftAndRightRoationSpeed = 220;
    [SerializeField] private float minimumPivot = -30; // the lowest point you're able to look down
    [SerializeField] private float maximumPivot = 60; // the highest point you're able to look up
    [SerializeField] private float cameraCollisionRadius = .2f;
    [SerializeField] private LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; // used for camera Collisions (move the camera object to this position upon colliding)
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;
    private float cameraZPosition; // value used for camera collisions
    private float targetCameraZPosition; // value used for camera collisions


    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraGameActions()
    {
        if(player != null)
        {
            HandleFollowTarget();
            HandleCameraRotations();
            HandleCameraCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleCameraRotations()
    {
        // rotate up and right the camera
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRoationSpeed) * Time.deltaTime;

        // rotate left and right the camera
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRoationSpeed) * Time.deltaTime;

        // clamp the up and down look angle between a min and max value
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        // rotate this game object left and righ (Y axis)
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // rotate this object un and right (X axis)
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        targetCameraZPosition = cameraZPosition;

        RaycastHit hit;

        // direction for collision check
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        // check if theres an object infront of our desired direction (see above)
        if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius,
                                direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            // if there is, we get our distance from it
            float distanceFromObject = Vector3.Distance(cameraPivotTransform.position, hit.point);

            // then equate our target Z position to the follow
            targetCameraZPosition = -(distanceFromObject - cameraCollisionRadius);
        }

        // if our target position is less than our collision radius, we subtract our collision radius (making it snap back)
        if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }
        
        // then apply our final position using a lerp
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

}
