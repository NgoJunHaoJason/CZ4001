using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    # region Properties

    public bool IsDead { get => isDead; }

    #endregion

    # region Fields

    private int currentHealth;

    private AudioSource playerAudioSource = null;

    private bool isDead = false;
    private bool isTakingDamage = false;

    # endregion

    # region Serialize Fields

    [SerializeField]
    private int startingHealth = 100;

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

    # endregion

    # region MonoBehaviour Methods

    void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        currentHealth = startingHealth;

        if (Debug.isDebugBuild)
        {
            if (healthValueText == null)
                Debug.LogError(
                    "Player healthValueText is not set in Unity editor", 
                    healthValueText
                );

            if (healthSlider == null)
                Debug.LogError(
                    "Player healthSlider is not set in Unity editor", 
                    healthSlider
                );

            if (deathAudioClip == null)
                Debug.LogError(
                    "Player deathAudioClip is not set in Unity editor", 
                    deathAudioClip
                );

            if (damageImage == null)
                Debug.LogError(
                    "Player damageImage is not set in Unity editor", 
                    damageImage
                );
        }
    }


    void FixedUpdate()
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

    # endregion

    # region Private Methods

    private void Die()
    {
        isDead = true;

        playerAudioSource.clip = deathAudioClip;
        playerAudioSource.Play();
    }

    # endregion

    # region Public Methods

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

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    # endregion
}

