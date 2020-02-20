using UnityEngine;


[RequireComponent(typeof(Light))]
public class SunController : MonoBehaviour
{
    # region Serialize Fields

    [SerializeField]
    private float dayTime = 60;

    [SerializeField]
    private float dimFactor = 3;
    
    # endregion

    # region Fields

    private float moveAngle = 0;

    private new Light light = null;

    # endregion

    # region MonoBehaviour Methods

    void Start()
    {
        if (dayTime != 0)
            moveAngle = 60/dayTime;
        
        light = GetComponent<Light>();
        light.intensity = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * moveAngle);
        light.color += (Color.red/(dayTime * dimFactor)) * Time.deltaTime;
    }

    # endregion
}
