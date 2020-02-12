using UnityEngine;

public class TempGameManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("cursor lock state is " + Cursor.lockState.ToString());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked?
                CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
