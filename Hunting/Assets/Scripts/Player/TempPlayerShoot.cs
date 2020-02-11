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
    private float thrust = 25f;

    [SerializeField]
    private float shootDelay = 0.5f;

    [SerializeField]
    private bool showArrowTrail = true;

    # endregion

    # region Fields

    private Camera playerCamera = null;

    private Collider playerCollider = null;

    private float shootTimer = 0;

    # endregion

    # region MonoBehaviour Methods

    void Start()
    {
        playerCollider = GetComponent<Collider>();
        playerCamera = GetComponentInChildren<Camera>();

        if (Debug.isDebugBuild)
        {
            if (tempArrowPrefab == null)
                Debug.LogError("Temp Arrow Prefab is not assigned in Temp Player Shoot.");
            if (playerCamera == null)
                Debug.LogError("Player Game Object's Camera is missing");
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
        Vector3 startingPosition = playerCamera.transform.position;
        startingPosition.y -= 0.1f; // right below camera

        // probability not the most straight-forward way to do this, but this works
        Quaternion startingRotation = new Quaternion(
            transform.rotation.x + playerCamera.transform.rotation.x, 
            transform.rotation.y + playerCamera.transform.rotation.y,
            transform.rotation.z + playerCamera.transform.rotation.z,
            transform.rotation.w + playerCamera.transform.rotation.w
        );

        // create arrow within temporary player game object
        GameObject arrowGameObject = Instantiate(
            tempArrowPrefab, 
            startingPosition, 
            startingRotation
        );

        Physics.IgnoreCollision(playerCollider, arrowGameObject.GetComponent<Collider>());

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
            TrailRenderer trailRenderer = arrowGameObject.GetComponentInChildren<TrailRenderer>();

            if (trailRenderer != null && showArrowTrail)
                trailRenderer.emitting = true;

            Rigidbody arrowRigidbody = arrowGameObject.GetComponent<Rigidbody>();

            arrowRigidbody.isKinematic = false;
            
            arrowRigidbody.velocity = arrowGameObject.transform.
                TransformDirection(Vector3.forward) * thrust;
        }

        arrowGameObject.GetComponentInChildren<VRTK.Examples.Archery.Arrow>().inFlight = true;
    }

    # endregion
}
