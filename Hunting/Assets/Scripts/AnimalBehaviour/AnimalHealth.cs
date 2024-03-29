﻿using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody))]
public class AnimalHealth : MonoBehaviour
{
    # region Serialize Fields
    [SerializeField]
    private int maxHealth = 3;

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

    public bool IsDead { get => currentHealth <= 0; }

    public List<GameObject> AttachedArrows { get; private set; }
    # endregion

    #region Fields

    private bool recentlyDamaged = false;

    private int currentHealth = 3;
    
    private GameSettings game;
    # endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        currentHealth = maxHealth;
        AttachedArrows = new List<GameObject>();
        game = GameObject.FindGameObjectWithTag("GameController").
            GetComponent<GameSettings>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            GameObject arrowGameObject = collision.gameObject;

            // make arrow stop moving
            arrowGameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Rigidbody arrowRigidbody = arrowGameObject.GetComponentInChildren<Rigidbody>();
            arrowRigidbody.isKinematic = false; // apply physics to arrow
            arrowRigidbody.velocity = Vector3.zero; // make arrow stop moving

            // stop emitting trail
            collision.gameObject.GetComponentInChildren<TrailRenderer>().emitting = false;

            // get the specific collider of the animal that the arrow hit
            Collider collider = collision.GetContact(0).thisCollider;
            bool hitHead = collider.gameObject.CompareTag("Head");
            if (hitHead)
                game.addHeadshotBonus();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            TakeDamageFrom(player, hitHead ? maxHealth : 1); // OHKO if headshot

            // https://answers.unity.com/questions/871292/fixed-joint-rigidbody-colliding.html
            // make arrow stick on animal's body
            FixedJoint fixedJoint = collision.gameObject.AddComponent<FixedJoint>();
            fixedJoint.enableCollision = false;
            fixedJoint.connectedBody = this.GetComponent<Rigidbody>();

            AttachedArrows.Add(arrowGameObject);
        }
    }
    #endregion

    #region Public Methods
    public void TakeDamageFrom(GameObject attacker, int damage = 1)
    {
        currentHealth -= damage;

        recentlyDamaged = true;
        LastAttackedBy = attacker;
        AttackedByPlayer = attacker.CompareTag("Player");

        if (Debug.isDebugBuild)
            Debug.Log(this.gameObject.name + " took " + damage.ToString() + 
            " damage from " + attacker.name + "; current health: " + currentHealth.ToString());

        if (IsDead)
        {
            Rigidbody animalRigidbody = this.GetComponent<Rigidbody>();
            animalRigidbody.isKinematic = true; // ignore physics
            animalRigidbody.constraints = RigidbodyConstraints.None;
        }
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
