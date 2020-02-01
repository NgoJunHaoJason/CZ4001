using UnityEngine;


/**
*   references:
*   https://codingchronicles.com/unity-vr-development/day-8-creating-character-first-person-shooter
*   https://youtu.be/blO039OzUZc
*/
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int movementSpeed = 3;

    [SerializeField]
    private int jumpingForce = 3;

    [SerializeField]
    private int cameraHorizontalSensitivity = 4;

    [SerializeField]
    private int cameraVerticalSensitivity = 3;

    [SerializeField]
    private int cameraSmoothing = 2;

    private Rigidbody playerRigidbody;
    private Camera playerCamera;

    private Vector2 cameraLook = new Vector2();
    private Vector2 smoothVector = new Vector2();

    private bool isGrounded = true;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CameraLook();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = true;
        }
    }

    void Move()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 movement = horizontalAxis * transform.right + verticalAxis * transform.forward;

        movement = movement.normalized * movementSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void CameraLook()
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

        playerCamera.transform.localRotation = Quaternion.AngleAxis(-cameraLook.y, Vector3.right);

        transform.rotation = Quaternion.AngleAxis(cameraLook.x, transform.up);
    }
}
