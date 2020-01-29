using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AnimalReach : MonoBehaviour
{

    public GameObject playerInRange = null;
    public GameObject herbivoreInRange = null;
    public GameObject deadAnimalInRange = null;


    private void OnTriggerStay(Collider other)
    {

        VRTK_PlayerObject playerObject = other.GetComponent<VRTK_PlayerObject>();
        if (playerObject != null && playerObject.objectType == VRTK_PlayerObject.ObjectTypes.Collider)
        {
            playerInRange = other.gameObject;
            return;
        }
        /*
        AnimalBehaviour animalBehaviour = other.GetComponent<AnimalBehaviour>();
        if (animalBehaviour != null && animalBehaviour.animalCategory == AnimalBehaviour.AnimalCategory.HERBIVORE)
        {
            if (other.GetComponentInChildren<AnimalHealth>().IsDead())
            {
                herbivoreInRange = null;
                deadAnimalInRange = other.gameObject;
            }
            else
                herbivoreInRange = other.gameObject;
            return;
        }
        */
        AnimalBehaviour animalBehaviour = other.gameObject.GetComponent<AnimalBehaviour>();
        AnimalHealth animalHealth = other.gameObject.GetComponentInChildren<AnimalHealth>();

        herbivoreInRange = null;
        deadAnimalInRange = null;

        if (!animalBehaviour || !animalHealth)
            return;

        if (animalHealth.IsDead())
            deadAnimalInRange = other.gameObject;
        else if (animalBehaviour.animalCategory == AnimalBehaviour.AnimalCategory.HERBIVORE)
        {
             herbivoreInRange = other.gameObject;
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

        AnimalBehaviour animalBehaviour = other.GetComponent<AnimalBehaviour>();
        if (animalBehaviour != null && animalBehaviour.animalCategory == AnimalBehaviour.AnimalCategory.HERBIVORE)
        {
            herbivoreInRange = null;
            deadAnimalInRange = null;
            return;
        }

    }

}
