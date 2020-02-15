using UnityEngine;


public class AnimalHealth : MonoBehaviour
{
    # region Serialize Fields
    [SerializeField]
    private float maxHealth = 3;
    
    [SerializeField]
    private float fightFleeThreshold = 0;

    [SerializeField]
    public float damageDelay = 0.5f; // time interval before damage can be taken
    # endregion

    #region Properties
    /// <summary>
    /// Used to check if this animal has ever been attacked by the player.
    /// </summary>
    public bool AttackedByPlayer { get; private set; }

    /// <summary>
    /// Used to check if who got the last hit on this animal.
    /// </summary>
    public GameObject LastAttackedBy { get; private set; }

    public bool ShouldFlee { get => currentHealth < fightFleeThreshold; }

    public bool IsDead { get => currentHealth <= 0;}
    # endregion

    #region Fields

    private bool recentlyDamaged = false;

    private float currentHealth = 3;

    private float damageTimer = 0;
    # endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (Debug.isDebugBuild)
                Debug.Log("Arrow collided with " + gameObject.name);
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            TakeDamageFrom(player);
        }
    }
    #endregion
    
    # region Public Methods
    public void TakeDamageFrom(GameObject attacker, int damage=1)
    {
        if (attacker != GameObject.FindGameObjectWithTag("Player"))
        {
            if (damageTimer > 0)
            {
                damageTimer -= Time.deltaTime;
                return;
            }
            else
            {
                damageTimer = damageDelay;
            }
        }
        else
        {
            AttackedByPlayer = true;
        }

        currentHealth -= damage;
        recentlyDamaged = true;
        LastAttackedBy = attacker;

        if (Debug.isDebugBuild)
            Debug.Log("Animal took damage; current health: " + currentHealth.ToString());
    }

    public bool IsRecentlyDamaged()
    {
        if (recentlyDamaged)
        {
            recentlyDamaged = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    # endregion
}
