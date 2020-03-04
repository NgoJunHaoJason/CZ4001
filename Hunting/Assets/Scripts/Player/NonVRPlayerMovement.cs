using UnityEngine;


/**
*   references:
*   https://codingchronicles.com/unity-vr-development/day-8-creating-character-first-person-shooter
*   https://youtu.be/blO039OzUZc
*/
[RequireComponent(typeof(Rigidbody))]
public class NonVRPlayerMovement : MonoBehaviour
{
    # region Serialize Fields
    [SerializeField]
    private int movementSpeed = 3;

    [SerializeField]
    private int jumpingForce = 3;
    # endregion

    # region Fields
    private Rigidbody playerRigidbody = null;

    private bool isGrounded = true;
    # endregion

    # region MonoBehaviour Methods
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            isGrounded = true;
        }
    }
    # endregion

    # region Private Methods
    private void Move()
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
    # endregion
}

