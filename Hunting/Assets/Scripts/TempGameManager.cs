using UnityEngine;

public class TempGameManager : MonoBehaviour
{
    # region MonoBehaviour Methods

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (Debug.isDebugBuild)
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

    # endregion
}
