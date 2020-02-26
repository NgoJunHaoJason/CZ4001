using UnityEngine;


public class BearBehaviour : AggressiveAnimalBehaviour
{
    void FixedUpdate()
    {
        if (health.IsDead)
        {
            Die();
        }
        else if (health.IsRecentlyDamaged() && !sight.HasPlayerInRange)
        {
            Flee();
        }
        else if (((CarnivoreReach) reach).HasDeadAnimalInRange)
        {
            Eat();
        }
        else if (reach.HasPlayerInRange || ((CarnivoreReach) reach).HasHerbivoreInRange)
        {
            Attack(reach.HasPlayerInRange);  // prioritise attacking player
        }
        else if (sight.HasDeadAnimalInRange)
        {
            Chase(sight.DeadAnimalInRange);
        }
        else if (sight.HasHerbivoreInRange)
        {
            Chase(sight.FirstHerbivoreInRange);
        }
        else if (sight.HasPlayerInRange)
        {
            Chase(sight.PlayerInRange);
        }
        else if (!destinationReached)
        {
            if (fleeing)
                Run();
            else 
                Walk();
        }
        else
        {
            RandomIdle();
        }
    }

    protected override void RandomIdle()
    {
        if (actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
            return;
        }

        fleeing = false;
        switch (Random.Range(0, 2))
        {
            case 0:
                ChangeDestination(null, 1f);
                Walk();
                break;
            case 1:
                Idle();
                break;
        }
        chaseTimer = idleActionInterval;
        actionTimer = idleActionInterval;

    }

    protected override void ChangeAnimation(AnimalAnimation newAnimation)
    {
        if (currentAnimation != newAnimation)
        {
            currentAnimation = newAnimation;
            animator.SetBool("Attack", false);
            animator.SetBool("Eat", false);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);

            switch (currentAnimation)
            {
                case AnimalAnimation.ATTACK:
                    animator.SetBool("Attack", true);
                    if (!audioSource.isPlaying)
                        audioSource.PlayOneShot(audioSource.clip);
                    break;
                case AnimalAnimation.DIE:
                    animator.SetBool("Die", true);
                    break;
                case AnimalAnimation.EAT:
                    animator.SetBool("Eat", true);
                    break;
                case AnimalAnimation.RUN:
                    animator.SetBool("Run", true);
                    break;
                case AnimalAnimation.WALK:
                    animator.SetBool("Walk", true);
                    break;
                default:
                    break;
            }
        }
    }
}
