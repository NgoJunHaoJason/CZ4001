using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DeterministicAnimalSpawner : MonoBehaviour
{
    public CharacterController charController;
    public Transform SpawnPoint;

    public float minSpawnDistance;
    public float maxSpawnDistance;

    public float spawnDelay;
    public float movementSpeed;

    public GameObject rabbit;
    public GameObject[] deer;
    public GameObject[] cattle;
    public GameObject goat;
    public GameObject ibex;
    public GameObject boar;
    public GameObject wolf;
    public GameObject bear;

    public int numRabbit;
    public int numDeer;
    public int numCattle;
    public int numGoat;
    public int numIbex;
    public int numBoar;
    public int numWolf;
    public int numBear;
    
    private float probRabbit;
    private float probDeer;
    private float probCattle;
    private float probGoat;
    private float probIbex;
    private float probBoar;
    private float probWolf;
    private float probBear;

    private Vector3 terrainMinPosition;
    private Vector3 terrainMaxPosition;

    private Vector3 destination;
    private bool destinationReached;


    private int nextAnimal = 0;
    private float spawnTimer = -1;

    // Start is called before the first frame update
    void Start()
    {
        TerrainManager terrainManager = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainManager>();
        terrainMinPosition = terrainManager.terrainMinPosition;
        terrainMaxPosition = terrainManager.terrainMaxPosition;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
            Move();
            return;
            
        }

        if (destinationReached)
        {
            SpawnAnimal();
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + new Vector3(0, 2, 0);
        ChangeDestination();

    }

    private void ChangeDestination()
    {
        float range = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector2 direction = Random.insideUnitCircle.normalized;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        float movableX = Mathf.Clamp(player.transform.position.x + direction.x * range , terrainMinPosition.x, terrainMaxPosition.x);
        float movableZ = Mathf.Clamp(player.transform.position.z + direction.y * range, terrainMinPosition.z, terrainMaxPosition.z);
        destination = new Vector3(movableX, 0, movableZ);

        destinationReached = false;
        spawnTimer = spawnDelay;

    }

    private void Move()
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0;

        if (direction.magnitude < 0.5)
        {
            destinationReached = true;
        }
        else
        {
            charController.SimpleMove(direction.normalized * movementSpeed);
        }

    }

    private void SpawnAnimal()
    {

        switch (nextAnimal)
        {
            case 0:
                if (GameObject.FindGameObjectsWithTag("Rabbit").Length < numRabbit)
                    Spawn(rabbit);
                break;
            case 1:
                if (GameObject.FindGameObjectsWithTag("Deer").Length < numDeer)
                    Spawn(deer[Random.Range(0, 2)]);
                break;
            case 2:
                if (GameObject.FindGameObjectsWithTag("Cattle").Length < numCattle)
                    Spawn(cattle[Random.Range(0, 2)]);
                break;
            case 3:
                if (GameObject.FindGameObjectsWithTag("Ibex").Length < numIbex)
                    Spawn(ibex);
                break;
            case 4:
                if (GameObject.FindGameObjectsWithTag("Goat").Length < numGoat)
                    Spawn(goat);
                break;
            case 5:
                if (GameObject.FindGameObjectsWithTag("Boar").Length < numBoar)
                    Spawn(boar);
                break;
            case 6:
                if (GameObject.FindGameObjectsWithTag("Wolf").Length < numWolf)
                    Spawn(wolf);
                break;
            case 7:
                if (GameObject.FindGameObjectsWithTag("Bear").Length < numBear)
                    Spawn(bear);
                break;
        }

        nextAnimal = (nextAnimal+1) % 8; 
    }

    private void Spawn(GameObject animal)
    {
        Instantiate(animal, SpawnPoint.position, Quaternion.Euler(0,0,0));
    }
  

}
