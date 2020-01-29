using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AnimalSight : MonoBehaviour
{
    public GameObject playerInRange = null;
    public List<GameObject> herbivoresInRange = new List<GameObject>();
    public List<GameObject> carnivoresInRange = new List<GameObject>();
    public GameObject deadAnimalInRange = null;

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
        else if (animalBehaviour.animalCategory == AnimalBehaviour.AnimalCategory.HERBIVORE)
        {
            if (herbivoresInRange.Contains(other.gameObject))
                return;
            else herbivoresInRange.Add(other.gameObject);
        }
        else if (animalBehaviour.animalCategory == AnimalBehaviour.AnimalCategory.CARNIVORE)
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
        else if (animalBehaviour.animalCategory == AnimalBehaviour.AnimalCategory.HERBIVORE)
            herbivoresInRange.Remove(other.gameObject);
        else if (animalBehaviour.animalCategory == AnimalBehaviour.AnimalCategory.CARNIVORE)
            carnivoresInRange.Remove(other.gameObject);

    }

}
