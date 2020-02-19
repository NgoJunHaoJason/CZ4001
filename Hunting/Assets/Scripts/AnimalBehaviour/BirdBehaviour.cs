using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimalBehaviour;

public class BirdBehaviour : MonoBehaviour
{

    private lb_Bird birdController;
    private GameSettings gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSettings>();
        birdController = this.gameObject.GetComponent<lb_Bird>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit by " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Arrow"))
        {

            Debug.Log("hit by arrow! " + collision.gameObject.tag);
            Collider collider = collision.GetContact(0).thisCollider;
            birdController.KillBird();
            gameController.SubmitHuntedAnimal(AnimalType.Bird);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("hit by " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Arrow"))
        {

            Debug.Log("hit by arrow! " + collision.gameObject.tag);
            // Collider collider = collision.GetContact(0).thisCollider;
            birdController.KillBird();
            gameController.SubmitHuntedAnimal(AnimalType.Bird);
        }
    }
}
