using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHealth : MonoBehaviour
{
    public float maxHealth;
    
    public float fightFleeThreshold;
    public float damageDelay;

    public bool attackedByPlayer = false;
    public GameObject lastAttackedBy;

    private bool recentlyDamaged = false;
    private float currentHealth;
    private float damageTimer = 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            print("Arrow collided with " + gameObject.name);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            TakeDamageFrom(player);
        }

    }

    public void TakeDamageFrom(GameObject attacker)
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
                currentHealth -= 0.1f;
                damageTimer = damageDelay;
            }
        } else
        {
            recentlyDamaged = true;
            attackedByPlayer = true;
            currentHealth--;
        }

        lastAttackedBy = attacker;

    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public bool ShouldFlee()
    {
        return currentHealth < fightFleeThreshold;
    }

    public bool IsRecentlyDamaged()
    {
        if (recentlyDamaged)
        {
            recentlyDamaged = false;
            return true;
        }

        return recentlyDamaged;
    }

    public GameObject GetAttacker()
    {
        return lastAttackedBy;
    }


}
