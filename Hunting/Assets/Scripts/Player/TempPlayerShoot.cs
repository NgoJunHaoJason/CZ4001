using UnityEngine;


// reference: scripts within
// Assets/Library/VRTK/LegacyExampleFiles/ExampleResources/Scripts/Archery
[RequireComponent(typeof(Collider))]
public class TempPlayerShoot : MonoBehaviour
{
    # region Serialize Fields

    [SerializeField]
    private GameObject tempArrowPrefab = null;

    [SerializeField]
    private float thrust = 20f;

    [SerializeField]
    private float shootDelay = 1f;

    # endregion

    # region Fields

    private Collider playerCollider = null;

    private float shootTimer = 0;

    # endregion

    # region MonoBehaviour Methods

    void Start()
    {
        playerCollider = GetComponent<Collider>();

        if (Debug.isDebugBuild)
        {
            if (tempArrowPrefab == null)
                Debug.LogError("Temp Arrow Prefab is not assigned in Temp Player Shoot.");
        }
    }

    void FixedUpdate()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButtonUp(0) && shootTimer >= shootDelay)
        {
            ShootArrow();
            shootTimer = 0;
        }
    }

    # endregion

    # region Private Methods

    private void ShootArrow()
    {
        Vector3 startingPosition = transform.position;
        startingPosition.y += 0.6f; // right below camera

        // create arrow within temporary player game object
        GameObject arrowGameObject = Instantiate(
            tempArrowPrefab, 
            startingPosition, 
            transform.rotation
        );

        Physics.IgnoreCollision(playerCollider, arrowGameObject.GetComponent<Collider>());

        Rigidbody arrowRigidbody = arrowGameObject.GetComponent<Rigidbody>();

        if (arrowGameObject == null)
        {
            if (Debug.isDebugBuild)
                Debug.LogError(
                    "Temp Arrow Prefab is missing RigidBody component", 
                    arrowGameObject
                );
        }
        else
        {
            arrowRigidbody.velocity = arrowGameObject.transform.
                TransformDirection(Vector3.forward) * thrust;
        }
    }

    # endregion
}
