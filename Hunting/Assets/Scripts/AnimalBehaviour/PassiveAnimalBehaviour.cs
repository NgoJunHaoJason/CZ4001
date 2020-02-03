using UnityEngine;

public class PassiveAnimalBehaviour : AnimalBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentAnimation == AnimalAnimation.DIE)
            return;

        if (health.IsDead())
        {
            Die();
        }
        else if (sight.playerInRange != null || sight.carnivoresInRange.Count > 0)
        {
            Flee();
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
                Eat();
                break;
        }

        actionTimer = idleActionInterval;

    }

    protected override void ChangeAnimation(AnimalAnimation newAnimation)
    {
        if (currentAnimation != newAnimation)
        {
            currentAnimation = newAnimation;
            animator.SetBool("Eat", false);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);

            switch (currentAnimation)
            {
                case AnimalAnimation.DIE:
                    animator.SetBool("Die", true);
                    audioSource.PlayOneShot(audioSource.clip);
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
