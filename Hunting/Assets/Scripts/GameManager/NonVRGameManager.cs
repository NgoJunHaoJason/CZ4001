using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NonVRGameManager : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private GameObject reticleGameObject = null;

    [SerializeField]
    private GameObject healthDisplayGameObject = null;

    [SerializeField]
    private GameObject scoreGameObject = null;

    [SerializeField]
    private GameObject endGameImageGameObject = null;

    [SerializeField]
    private Text endGameScoreText = null;
    # endregion

    #region MonoBehaviour Methods
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (Debug.isDebugBuild)
        {
            Debug.Log("cursor lock state is " + Cursor.lockState.ToString());

            if (reticleGameObject == null)
                Debug.LogError("Reticle GameObject is not set in Unity editor");

            if (healthDisplayGameObject == null)
                Debug.LogError("HealthDisplay GameObject is not set in Unity editor");

            if (scoreGameObject == null)
                Debug.LogError("Score GameObject is not set in Unity editor");

            if (endGameImageGameObject == null)
                Debug.LogError("EndGameImage GameObject is not set in Unity editor");

            if (endGameScoreText == null)
                Debug.LogError("EndGameScore Text is not set in Unity editor");
        }
            
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked?
                CursorLockMode.None : CursorLockMode.Locked;
            
            if (Debug.isDebugBuild)
                Debug.Log("cursor lock state is " + Cursor.lockState.ToString());
        }
    }
    # endregion

    #region Public Methods
    public void EndGame() // no infinity war though
    {
        endGameScoreText.text = scoreGameObject.GetComponent<Text>().text;
        ToggleUIGameObjects();
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ToggleUIGameObjects();
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Private Methods
    private void ToggleUIGameObjects()
    {
        reticleGameObject.SetActive(!reticleGameObject.activeSelf);
        healthDisplayGameObject.SetActive(!healthDisplayGameObject.activeSelf);
        scoreGameObject.SetActive(!scoreGameObject.activeSelf);
        endGameImageGameObject.SetActive(!endGameImageGameObject.activeSelf);
    }
    #endregion 
}
