﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerBehaviour : AnimalBehaviour
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
        else if (sight.playerInRange || sight.carnivoresInRange.Count > 0)
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
                Eat();
                break;
        }

        actionTimer = idleActionInterval;

    }


    public override void ChangeAnimation(AnimalAnimation newAnimation)
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