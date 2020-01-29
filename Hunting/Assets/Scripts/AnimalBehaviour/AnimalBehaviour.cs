using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalBehaviour : MonoBehaviour
{
    public CharacterController charController;
    public Animator animator;

    public AnimalHealth health;
    public AnimalSight sight;
    public AnimalReach reach;

    public AudioSource audioSource;

    public enum AnimalCategory { HERBIVORE, CARNIVORE }
    public AnimalCategory animalCategory;

    public enum AnimalType { Rabbit, Deer, Cattle, Goat, Ibex, Boar, Wolf, Bear}
    public AnimalType animalType;

    public int minMovableDistance;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    public float obstacleDetectionRange;
    public int idleActionInterval;

    public enum AnimalAnimation { ATTACK, WALK, RUN, IDLE, DIE, EAT, HOWL }
    protected AnimalAnimation currentAnimation = AnimalAnimation.IDLE;

    protected Vector3 terrainMinPosition;
    protected Vector3 terrainMaxPosition;

    private GameSettings gameLoader;
    private Vector3 previousLocation;

    protected Vector3 destination;
    protected bool destinationReached = true;
    protected bool fleeing = false;

    protected float actionTimer;
    protected float movementTimer;
    protected float chaseTimer;


    // Start is called before the first frame update
    void Start()
    {
        TerrainManager terrainManager = GameObject.FindGameObjectWithTag("Terrain").GetComponent<TerrainManager>();
        terrainMinPosition = terrainManager.terrainMinPosition;
        terrainMaxPosition = terrainManager.terrainMaxPosition;

        gameLoader = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSettings>();

    }

    protected void ChangeDestination(GameObject target, float distanceMult)
    {
        if (target != null)
        {
                destination = target.transform.position;
        }
        else
        {
            do
            {

                int sign = Random.Range(0, 1f) > 0.5 ? 1 : -1;
                int movableDistance = (int)(minMovableDistance * distanceMult);
                float movableX = Mathf.Clamp(transform.position.x +  sign * Random.Range(minMovableDistance, movableDistance), terrainMinPosition.x, terrainMaxPosition.x);
                sign = Random.Range(0, 1f) > 0.5 ? 1 : -1;
                float movableZ = Mathf.Clamp(transform.position.z + sign * Random.Range(minMovableDistance, movableDistance), terrainMinPosition.z, terrainMaxPosition.z);
                destination = new Vector3(movableX, 0, movableZ);

            } while (CollisionDetected(destination - transform.position));

        }

        destinationReached = false;
    }

    protected void Turn()
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0;

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);

    }

    private bool CollisionDetected(Vector3 direction)
    {
        direction.y = 0;

        Transform ray = transform;

        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward * obstacleDetectionRange, Color.red);

        if (Physics.Raycast(ray.position + new Vector3(0, 0.5f, 0), direction, out RaycastHit hit, obstacleDetectionRange))
        {
            if (hit.collider.gameObject.CompareTag("Terrain") || hit.collider.gameObject.CompareTag("Tree"))
            {
                return true;
            }
        }

        return false;
    }

    protected void Move(float speed)
    {
        /*
        Vector3 direction = destination - transform.position;
        direction.y = 0;

        if (direction.magnitude < 0.5)
        {
            destinationReached = true;
            Idle();
        }
        else
        {
            charController.Move(direction.normalized * speed);
            movementTimer += Time.deltaTime;
        }
        */

        Vector3 direction = destination - transform.position;
        direction.y = 0;

        if (CollisionDetected(direction))
        {
            ChangeDestination(null, 1f);
        }


        if (direction.magnitude < 0.5)
        {
            destinationReached = true;
            Idle();
        }
        else {

            charController.SimpleMove(direction.normalized * speed);
            movementTimer -= Time.deltaTime;

        }
        
    }

    public void Flee()
    {
        if (destinationReached)
            ChangeDestination(null, 2f);
        fleeing = true;
        Run();
        
    }

    public void Chase(GameObject target)
    {
        ChangeDestination(target, 1f);
        if (chaseTimer > 0)
        {
            Run();
            chaseTimer -= Time.deltaTime;
        }
        else
        {
            Walk();
        }

    }


    protected void Attack(bool isPlayer)
    {
        ChangeAnimation(AnimalAnimation.ATTACK);
        destinationReached = true;
        Turn();
        if (isPlayer)
        {
            PlayerHealth playerHealth = reach.playerInRange.GetComponentInChildren<PlayerHealth>();
        }
        else 
        {
            AnimalHealth animalHealth = reach.herbivoreInRange.GetComponentInChildren<AnimalHealth>();
            animalHealth.TakeDamageFrom(gameObject);
        }
    }

    public void Die()
    {
        ChangeAnimation(AnimalAnimation.DIE);
        if (GameObject.FindGameObjectWithTag("Player") == health.GetAttacker())
            gameLoader.SubmitHuntedAnimal(animalType);
        Destroy(gameObject, 30f);
    }

    protected void Eat()
    {
        ChangeAnimation(AnimalAnimation.EAT);
        chaseTimer = idleActionInterval * 2;
    }

    protected void Howl()
    {
        ChangeAnimation(AnimalAnimation.HOWL);
    }

    protected void Idle()
    {
        ChangeAnimation(AnimalAnimation.IDLE);
        chaseTimer = idleActionInterval;
    }

    protected void Walk()
    {
        ChangeAnimation(AnimalAnimation.WALK);
        Turn();
        Move(walkSpeed);

    }

    protected void Run()
    {
        ChangeAnimation(AnimalAnimation.RUN);
        Turn();
        Move(runSpeed);
    }

    public abstract void RandomIdle();

    public abstract void ChangeAnimation(AnimalAnimation newAnimation);
    
}
