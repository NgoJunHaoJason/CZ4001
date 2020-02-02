using UnityEngine;


/**
*   references:
*   https://codingchronicles.com/unity-vr-development/
day-8-creating-character-first-person-shooter
*   https://youtu.be/blO039OzUZc
*/
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    # region Serialize Fields 

    [SerializeField]
    private int movementSpeed = 3;

    [SerializeField]
    private int jumpingForce = 3;

    [SerializeField]
    private int cameraHorizontalSensitivity = 4;

    [SerializeField]
    private int cameraVerticalSensitivity = 3;

    [SerializeField]

    # endregion

    # region Fields

    private int cameraSmoothing = 2;

    private Rigidbody playerRigidbody = null;

    private Camera playerCamera = null;

    private Vector2 cameraLook = new Vector2();
    private Vector2 smoothVector = new Vector2();

    // prevents player from jumping while already in the air
    private bool isGrounded = true;

    # endregion

    # region MonoBehaviour Methods

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera == null && Debug.isDebugBuild)
            Debug.LogError("No Camera is attached to the Player");
    }

    void FixedUpdate()
    {
        Move();
        CameraLook();
    }

    # endregion

    # region Private Methods

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            isGrounded = true;
    }

    private void Move()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 movement = horizontalAxis * transform.right + 
            verticalAxis * transform.forward;

        movement = movement.normalized * movementSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // should place this method directly in Camera game object
    private void CameraLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        
        Vector2 mouseDirection = new Vector2(mouseX, mouseY);

        mouseDirection.x *= cameraHorizontalSensitivity * cameraSmoothing;
        mouseDirection.y *= cameraVerticalSensitivity * cameraSmoothing;

        smoothVector.x = Mathf.Lerp(smoothVector.x, mouseDirection.x, 1f / cameraSmoothing);
        smoothVector.y = Mathf.Lerp(smoothVector.y, mouseDirection.y, 1f / cameraSmoothing);

        cameraLook += smoothVector;
        cameraLook.y = Mathf.Clamp(cameraLook.y, -70, 70);

        playerCamera.transform.localRotation = Quaternion.AngleAxis(
            -cameraLook.y, 
            Vector3.right
        );

        transform.rotation = Quaternion.AngleAxis(cameraLook.x, transform.up);
    }

    # endregion
}
