using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public float dayTime;
    public float dimFactor;
    float moveAngle;
    Light light;
    // Start is called before the first frame update
    void Start()
    {
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
}
