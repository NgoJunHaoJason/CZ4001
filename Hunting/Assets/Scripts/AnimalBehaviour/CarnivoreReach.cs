using UnityEngine;
using VRTK;


public class CarnivoreReach : AnimalReach
{
    # region Properties
    public GameObject HerbivoreInRange { get => herbivoreInRange; }

    public bool HasHerbivoreInRange { get => herbivoreInRange != null; }

    public bool HasDeadAnimalInRange { get => deadAnimalInRange != null; }
    # endregion

    # region Fields
    private GameObject herbivoreInRange = null;
    private GameObject deadAnimalInRange = null;
    # endregion

    # region Private Methods
    protected new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        VRTK_PlayerObject playerObject = other.GetComponent<VRTK_PlayerObject>();
        if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
            return;

        AnimalBehaviour animalBehaviour = other.gameObject.GetComponent<AnimalBehaviour>();
        AnimalHealth animalHealth = other.gameObject.GetComponentInChildren<AnimalHealth>();

        herbivoreInRange = null;
        deadAnimalInRange = null;

        if (!animalBehaviour || !animalHealth)
            return;

        if (animalHealth.IsDead)
            deadAnimalInRange = other.gameObject;
        else if (animalBehaviour.Category == AnimalBehaviour.AnimalCategory.HERBIVORE)
        {
            herbivoreInRange = other.gameObject;
        }
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        
        VRTK_PlayerObject playerObject = other.GetComponent<VRTK_PlayerObject>();
        if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
            return;

        AnimalBehaviour animalBehaviour = other.GetComponent<AnimalBehaviour>();
        if (animalBehaviour != null && animalBehaviour.Category == AnimalBehaviour.AnimalCategory.HERBIVORE)
        {
            herbivoreInRange = null;
            deadAnimalInRange = null;
            return;
        }
    }
    # endregion
}
