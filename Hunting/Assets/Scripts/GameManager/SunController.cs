using UnityEngine;


[RequireComponent(typeof(Light))]
public class SunController : MonoBehaviour
{
    # region Serialize Fields

    [SerializeField]
    private float dayTime = 60;

    [SerializeField]
    private float currentTimeOfDay = 0.5f;

    public float secondsInFullDay = 120f;
    public float timeMultiplier = 0.5f;

    public MeshRenderer cloud;
    
    # endregion

    # region Fields


    private new Light sun;
    private new float sunInitialIntensity;

    private new Material high = null;

    private new float r = 1f;
    private new float g = 1f;
    private new float b = 1f;


    public System.Collections.Generic.List<Color> colors = new System.Collections.Generic.List<Color>();

    # endregion

    # region MonoBehaviour Methods

    void Start()
    {
        
        sun = GetComponent<Light>();

        sunInitialIntensity = sun.intensity;

        high = cloud.materials[0];
    }

    // Update is called once per frame
    void Update()
    {

        UpdateSun();
        
        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
 
        if (currentTimeOfDay >= 1) {
            currentTimeOfDay = 0;
        }

    }

    void UpdateSun() {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
 
        float intensityMultiplier = 1;
        if (currentTimeOfDay >= 0.8f || currentTimeOfDay <= 0.2f)  // Night time
        {
        	r = 0f;
        	g = 0f;
        	b = 0f;
    	    colors.Add(new Color(r,g,b));
        	high.SetColorArray("_CloudColor", colors);
			colors.Clear();    		
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.7 && currentTimeOfDay >= 0.3) // Day time 
        {
        	r = 1f;
        	g = 1f;
        	b = 1f;
    	    colors.Add(new Color(r,g,b));
        	high.SetColorArray("_CloudColor", colors);
			colors.Clear();   
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else // sunrise, sunset
        {
        	r = 0.9150943f;
        	g = 0.5568262f;
        	b = 0.5568262f;
    	    colors.Add(new Color(r,g,b));
        	high.SetColorArray("_CloudColor", colors);
			colors.Clear();   
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }
 
        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }

    # endregion
}
