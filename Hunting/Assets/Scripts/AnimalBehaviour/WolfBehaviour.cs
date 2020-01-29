using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBehaviour : AnimalBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (currentAnimation == AnimalAnimation.DIE)
            return;

        if (health.IsDead())
        {
            Die();
        }
        else if (health.IsRecentlyDamaged() && !sight.playerInRange)
        {
            Flee();
        }
        else if (reach.deadAnimalInRange)
        {
            Eat();
        }
        else if (reach.herbivoreInRange)
        {
            Attack(false);
        }
        else if (reach.playerInRange)
        {
            Attack(true);
        }
        else if (sight.deadAnimalInRange)
        {
            Chase(sight.deadAnimalInRange);
        }
        else if (sight.herbivoresInRange.Count > 0)
        {
            Chase(sight.herbivoresInRange[0]);
        }
        else if (sight.playerInRange)
        {
            Chase(sight.playerInRange);
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

    public override void RandomIdle()
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

    public override void ChangeAnimation(AnimalAnimation newAnimation)
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

}
