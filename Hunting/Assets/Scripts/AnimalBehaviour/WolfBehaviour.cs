using UnityEngine;


public class WolfBehaviour : AggressiveAnimalBehaviour
{
    void FixedUpdate()
    {
        if (currentAnimation == AnimalAnimation.DIE)
            return;

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
            Attack(reach.HasPlayerInRange); // prioritise attacking player
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
        switch (Random.Range(0, 3))
        {
            case 0:
                ChangeDestination(null, 1f);
                Walk();
                break;
            case 1:
                Idle();
                break;
            case 2:
                Howl();
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
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Eat", false);
            animator.SetBool("Howl", false);

            switch (currentAnimation)
            {
                case AnimalAnimation.HOWL:
                    animator.SetBool("Howl", true);
                    break;
                case AnimalAnimation.ATTACK:
                    animator.SetBool("Attack", true);
                    if (!audioSource.isPlaying)
                        audioSource.PlayOneShot(audioSource.clip);
                    break;
                case AnimalAnimation.DIE:
                    animator.SetBool("Die", true);
                    break;
                case AnimalAnimation.WALK:
                    animator.SetBool("Walk", true);
                    break;
                case AnimalAnimation.RUN:
                    animator.SetBool("Run", true);
                    break;
                case AnimalAnimation.EAT:
                    animator.SetBool("Eat", true);
                    break;
                default:
                    break;
            }
        }
    }

    private void Howl() => ChangeAnimation(AnimalAnimation.HOWL);

    // wolf can chase forever
    public new void Chase(GameObject target)
    {
        ChangeDestination(target, 1f);

        Run();
    }
}
