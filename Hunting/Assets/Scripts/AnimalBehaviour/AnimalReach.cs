using UnityEngine;
using VRTK;

public class AnimalReach : MonoBehaviour
{
    # region Properties

    public GameObject PlayerInRange { get => playerInRange; }
    
    public bool HasPlayerInRange { get => playerInRange != null; }

    # endregion

    # region Fields

    private GameObject playerInRange = null;

    # endregion

    # region Private Methods

    protected void OnTriggerStay(Collider other)
    {
        VRTK_PlayerObject playerObject = other.GetComponent<VRTK_PlayerObject>();
        if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
        {
            playerInRange = other.gameObject;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        VRTK_PlayerObject playerObject = other.GetComponent<VRTK_PlayerObject>();
        if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
        {
            playerInRange = null;
            return;
        }
    }

    # endregion
}
