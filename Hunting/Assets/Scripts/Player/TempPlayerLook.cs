using UnityEngine;


/**
*   references:
*   https://codingchronicles.com/unity-vr-development/day-8-creating-character-first-person-shooter
*   https://youtu.be/blO039OzUZc
*/
[RequireComponent(typeof(Rigidbody))]
public class TempPlayerLook : MonoBehaviour
{
    # region Serialize Fields

    [SerializeField]
    private int cameraHorizontalSensitivity = 4;

    [SerializeField]
    private int cameraVerticalSensitivity = 3;

    [SerializeField]
    private int cameraSmoothing = 2;

    # endregion

    # region Fields

    private Camera playerCamera = null;

    private Vector2 cameraLook = new Vector2();

    private Vector2 smoothVector = new Vector2();

    # endregion

    # region MonoBehaviour Methods

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();

        if (Debug.isDebugBuild && playerCamera == null)
            Debug.LogError("Camera is not attached to Player game object", gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraLook();
    }

    # endregion

    # region Private Methods

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

        playerCamera.transform.localRotation = Quaternion.AngleAxis(-cameraLook.y, Vector3.right);

        transform.rotation = Quaternion.AngleAxis(cameraLook.x, transform.up);
    }

    # endregion
}

