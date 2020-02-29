using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    #region Properties
    public bool IsDead { get => isDead; }
    #endregion

    #region Fields
    private int currentHealth;

    private AudioSource playerAudioSource = null;

    private bool isDead = false;
    private bool isTakingDamage = false;
    #endregion

    #region Serialize Fields
    [SerializeField]
    private int startingHealth = 10;

    [SerializeField]
    private Text healthValueText = null;

    [SerializeField]
    private Slider healthSlider = null;

    [SerializeField]
    private AudioClip deathAudioClip = null;

    [SerializeField]
    private float flashSpeed = 5f;
    
    [SerializeField]
    private Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    [SerializeField]
    private Image damageImage = null;

    [SerializeField]
    private NonVRGameManager nonVRGameManager = null;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        playerAudioSource = this.GetComponent<AudioSource>();
        currentHealth = startingHealth;

        if (Debug.isDebugBuild)
        {
            if (healthValueText == null)
                Debug.LogError("HealthValueText is not set in Unity editor");

            if (healthSlider == null)
                Debug.LogError("HealthSlider is not set in Unity editor");

            if (deathAudioClip == null)
                Debug.LogError("DeathAudioClip is not set in Unity editor");

            if (damageImage == null)
                Debug.LogError("DamageImage is not set in Unity editor");

            if (nonVRGameManager == null)
                Debug.LogError("NonVRGameManager is not set in Unity editor");
        }
    }

    private void FixedUpdate()
    {
        if (isTakingDamage)
            damageImage.color = flashColour;
        else
        {
            damageImage.color = Color.Lerp(
                damageImage.color, 
                Color.clear, 
                flashSpeed * Time.deltaTime
            );
        }
        
        isTakingDamage = false;
    }
    #endregion

    #region Private Methods
    private void Die()
    {
        isDead = true;

        if (deathAudioClip != null)
        {
            playerAudioSource.clip = deathAudioClip;
            playerAudioSource.Play();
        }

        // may be better to implement observer pattern using event-delegates
        // not going to do it as there will only be 1 observer
        nonVRGameManager.EndGame();
    }
    #endregion

    #region Public Methods
    public void TakeDamage(int amount)
    {
        isTakingDamage = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;
        healthValueText.text = currentHealth.ToString();

        playerAudioSource.Play();

        if (currentHealth <= 0 && !isDead)
            Die();
    }
    #endregion
}

