using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public abstract class AnimalBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    # region Enums
    public enum AnimalCategory { HERBIVORE, CARNIVORE }

    public enum AnimalType { Rabbit, Deer, Cattle, Goat, Ibex, Boar, Wolf, Bear, Bird}

    public enum AnimalAnimation { ATTACK, WALK, RUN, IDLE, DIE, EAT, HOWL }
    # endregion

    # region Properties
    public AnimalCategory Category { get => animalCategory; }
    # endregion

    # region Serialize Fields
    [SerializeField]
    private AnimalCategory animalCategory;

    [SerializeField]
    private AnimalType animalType;

    [SerializeField]
    private float walkSpeed = 1;

    [SerializeField]
    private float runSpeed = 2;

    [SerializeField]
    private float turnSpeed = 3;

    [SerializeField]
    private float deathDuration = 30;

    [SerializeField]
    private float dyingSpeed = 1e-10f;

    [SerializeField]
    private int minMovableDistance;

    [SerializeField]
    private float obstacleDetectionRange;

    [SerializeField]
    protected int idleActionInterval;

    [SerializeField]
    protected AnimalHealth health;
    
    [SerializeField]
    protected AnimalSight sight;

    // quick hack to make sure that children game objects follow parent gameobject
    [SerializeField]
    private Transform[] childrenTransforms;
    # endregion

    # region Fields
    protected Animator animator = null;

    protected AudioSource audioSource = null;

    protected AnimalAnimation currentAnimation = AnimalAnimation.IDLE;

    protected bool destinationReached = true;

    protected bool fleeing = false;

    protected float actionTimer = 0;
    
    protected float chaseTimer = 0;

    private float deathTimer = 0;

    private CharacterController characterController = null;

    private GameSettings gameLoader = null;

    private Vector3 terrainMinPosition = new Vector3();

    private Vector3 terrainMaxPosition = new Vector3();

    private Vector3 destination = new Vector3();

    private float movementTimer = 0;
    # endregion

    # region MonoBehaviour Methods
    void Start()
    {
        TerrainManager terrainManager = GameObject.
            FindGameObjectWithTag("Terrain").GetComponent<TerrainManager>();
        terrainMinPosition = terrainManager.terrainMinPosition;
        terrainMaxPosition = terrainManager.terrainMaxPosition;
        agent = this.GetComponent<NavMeshAgent>();

        gameLoader = GameObject.FindGameObjectWithTag("GameController").
            GetComponent<GameSettings>();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
    }
    # endregion

    # region Private Methods
    private bool CollisionDetected(Vector3 direction)
    {
        direction.y = 0;

        Transform ray = transform;

        Debug.DrawRay(
            transform.position + new Vector3(0, 1, 0), 
            transform.forward * obstacleDetectionRange, 
            Color.red
        );

        if (
            Physics.Raycast(ray.position + new Vector3(0, 0.5f, 0), 
            direction, 
            out RaycastHit hit, 
            obstacleDetectionRange)
        )
        {
            if (hit.collider.gameObject.CompareTag("Terrain") || 
            hit.collider.gameObject.CompareTag("Tree"))
            {
                return true;
            }
        }

        return false;
    }

    private void Move(float speed)
    {
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
        else 
        {
            agent.speed = speed;
            agent.SetDestination(destination);
            //characterController.SimpleMove(direction.normalized * speed);
            movementTimer -= Time.deltaTime;
        }

        // quick hack to make sure that children game objects follow parent gameobject
        foreach (Transform childTransform in childrenTransforms)
        {
            childTransform.localPosition = Vector3.zero;
            childTransform.localRotation = Quaternion.identity;
        }
    }
    # endregion

    # region Protected Methods
    protected void ChangeDestination(GameObject target, float distanceMult)
    {
        if (target != null)
            destination = target.transform.position;
        else
        {
            do
            {
                int sign = Random.Range(0, 1f) > 0.5 ? 1 : -1;
                int movableDistance = (int)(minMovableDistance * distanceMult);
                float movableX = Mathf.Clamp(
                    transform.position.x +  sign * Random.Range(minMovableDistance, movableDistance), 
                    terrainMinPosition.x, terrainMaxPosition.x
                );
                
                sign = Random.Range(0, 1f) > 0.5 ? 1 : -1;
                float movableZ = Mathf.Clamp(
                    transform.position.z + sign * Random.Range(minMovableDistance, movableDistance), 
                    terrainMinPosition.z, terrainMaxPosition.z
                );
                
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
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            rotation, 
            turnSpeed * Time.deltaTime
        );

        // quick hack to make sure that children game objects follow parent gameobject
        foreach (Transform childTransform in childrenTransforms)
        {
            childTransform.localPosition = Vector3.zero;
            childTransform.localRotation = Quaternion.identity;
        }
    }

    protected void Flee()
    {
        if (destinationReached)
            ChangeDestination(null, 2f);
        
        fleeing = true;
        Run();
    }

    protected void Chase(GameObject target)
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

    protected void Die()
    {
        if (deathTimer == 0) // animal just died
        {
            foreach (GameObject arrowGameObject in health.AttachedArrows)
                Destroy(arrowGameObject);

            ChangeAnimation(AnimalAnimation.DIE); 

            if (health.LastAttackedBy.CompareTag("Player"))
                gameLoader.SubmitHuntedAnimal(animalType);
        }

        if (deathTimer < deathDuration)
        {
            deathTimer += Time.deltaTime;
        }
        else
        {
            // allow animal to sink through ground
            Physics.IgnoreCollision(
                GameObject.FindGameObjectWithTag("Terrain").GetComponent<Collider>(),
                this.GetComponent<Collider>()
            );

            if (this.transform.position.y > -5f)
                this.transform.Translate(-Vector3.up * Time.deltaTime * dyingSpeed, Space.World);
            else
                Destroy(this.gameObject);
        }
    }

    protected void Eat()
    {
        ChangeAnimation(AnimalAnimation.EAT);
        chaseTimer = idleActionInterval * 2;
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
    # endregion

    # region Abstract Methods
    protected abstract void RandomIdle();

    protected abstract void ChangeAnimation(AnimalAnimation newAnimation);
    # endregion
}
