﻿using UnityEngine;


public class WolfBehaviour : AggressiveAnimalBehaviour
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
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
                Attack(false);
        }
        else if (reach.playerInRange)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
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
}
