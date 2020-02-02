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

    private AudioSource playerAudio = null;

    private bool isDead = false;
    private bool damaged = false;

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
    private Image damageImage;

    # endregion

    # region MonoBehaviour Methods

    void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        //playerMovement = GetComponent<PlayerMovement>();
        //playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void FixedUpdate()
    {
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    # endregion

    # region Private Methods

    private void Die()
    {
        isDead = true;

        playerAudio.clip = deathAudioClip;
        playerAudio.Play();
    }

    # endregion

    # region Public Methods

    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;
        healthValueText.text = currentHealth.ToString();

        playerAudio.Play();

        if (Debug.isDebugBuild)
            Debug.Log("Player took damage and lost <color=Red>" + 
                amount.ToString() + "</color> health.");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    # endregion
}

