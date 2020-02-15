using UnityEngine;


public abstract class AggressiveAnimalBehaviour : AnimalBehaviour
{
    # region Serialize Fields
    // this is rather dangerous as AnimalReach can be assigned in Unity editor
    // when CarnivoreReach shuld be assigned; should update in the future
    [SerializeField]
    protected AnimalReach reach;

    [SerializeField]
    private float attackInterval = 2;
    # endregion

    # region Fields
    private float attackTimer = 0;
    # endregion

    # region Protected Methods
    protected void Attack(bool isTargetPlayer)
    {
        if (attackTimer >= attackInterval)
        {
            ChangeAnimation(AnimalAnimation.ATTACK);
            destinationReached = true;
            Turn();

            if (isTargetPlayer)
            {
                PlayerHealth playerHealth = reach.PlayerInRange.GetComponentInChildren<PlayerHealth>();
                
                if (playerHealth == null)
                    Debug.LogError("Player is missing PlayerHealth", playerHealth);
                else if (!playerHealth.IsDead)
                    playerHealth.TakeDamage(10);
            }
            else if (reach is CarnivoreReach)
            {
                AnimalHealth animalHealth = ((CarnivoreReach)reach).HerbivoreInRange.GetComponentInChildren<AnimalHealth>();
                animalHealth.TakeDamageFrom(gameObject, 2);
            }

            attackTimer = 0;
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }
    # endregion
}
