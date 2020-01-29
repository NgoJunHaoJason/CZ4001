using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDestructor : MonoBehaviour
{
    public float destroyDistance; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance > destroyDistance)
                Destroy(gameObject);

        }

        
    }
}
