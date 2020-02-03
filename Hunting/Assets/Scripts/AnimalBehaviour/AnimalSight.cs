using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class AnimalSight : MonoBehaviour
{
    # region Properties

    public GameObject PlayerInRange { get => playerInRange; }

    public bool HasPlayerInRange { get => playerInRange != null; }

    public GameObject DeadAnimalInRange { get => deadAnimalInRange; }

    public bool HasDeadAnimalInRange { get => deadAnimalInRange != null; }

    public bool HasCarnivoreInRange { get => carnivoresInRange.Count > 0; }

    public GameObject FirstHerbivoreInRange { get => herbivoresInRange[0]; }

    public bool HasHerbivoreInRange { get => herbivoresInRange.Count > 0; }

    # endregion

    # region Fields

    private GameObject playerInRange = null;

    private GameObject deadAnimalInRange = null;

    private List<GameObject> carnivoresInRange = new List<GameObject>();

    private List<GameObject> herbivoresInRange = new List<GameObject>();

    # endregion

    # region Private Methods

    private void OnTriggerStay(Collider other)
    {
        VRTK_PlayerObject playerObject = other.GetComponent<VRTK_PlayerObject>();
        if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
        {
            playerInRange = other.gameObject;
            return;
        }

        AnimalBehaviour animalBehaviour = other.gameObject.GetComponent<AnimalBehaviour>();
        AnimalHealth animalHealth = other.gameObject.GetComponentInChildren<AnimalHealth>();

        herbivoresInRange.RemoveAll(GameObject => GameObject == null);
        carnivoresInRange.RemoveAll(GameObject => GameObject == null);
        deadAnimalInRange = null;

        if (!animalBehaviour || !animalHealth)
            return;

        if (animalHealth.IsDead())
            deadAnimalInRange = other.gameObject;
        else if (animalBehaviour.Category == AnimalBehaviour.AnimalCategory.HERBIVORE)
        {
            if (herbivoresInRange.Contains(other.gameObject))
                return;
            else herbivoresInRange.Add(other.gameObject);
        }
        else if (animalBehaviour.Category == AnimalBehaviour.AnimalCategory.CARNIVORE)
        {
            if (carnivoresInRange.Contains(other.gameObject))
                return;
            else carnivoresInRange.Add(other.gameObject);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        VRTK_PlayerObject playerObject = other.GetComponent<VRTK_PlayerObject>();
        if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
        {
            playerInRange = null;
            return;
        }

        AnimalBehaviour animalBehaviour = other.gameObject.GetComponent<AnimalBehaviour>();

        if (animalBehaviour == null)
            return;
        else if (animalBehaviour.Category == AnimalBehaviour.AnimalCategory.HERBIVORE)
            herbivoresInRange.Remove(other.gameObject);
        else if (animalBehaviour.Category == AnimalBehaviour.AnimalCategory.CARNIVORE)
            carnivoresInRange.Remove(other.gameObject);
    }

    # endregion
}
