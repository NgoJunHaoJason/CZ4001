using UnityEngine;


public abstract class AggressiveAnimalBehaviour : AnimalBehaviour
{
    # region Serialize Fields

    [SerializeField]
    protected AnimalReach reach;

    # endregion

    # region Fields

    protected float attackTimer = 0;

    # endregion

    # region Protected Methods

    protected void Attack(bool isPlayer)
    {
        ChangeAnimation(AnimalAnimation.ATTACK);
        destinationReached = true;
        Turn();

        if (isPlayer)
        {
            PlayerHealth playerHealth = reach.PlayerInRange.GetComponentInChildren<PlayerHealth>();
            if (playerHealth == null)
                Debug.LogError("Player is missing PlayerHealth", playerHealth);
            else if (!playerHealth.IsDead)
                playerHealth.TakeDamage(10);
        }
        else 
        {
            AnimalHealth animalHealth = reach.HerbivoreInRange.GetComponentInChildren<AnimalHealth>();
            animalHealth.TakeDamageFrom(gameObject);
        }

        attackTimer = 0;
    }

    # endregion
}
