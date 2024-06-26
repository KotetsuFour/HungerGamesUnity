using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;
    [SerializeField, Range(0, 60 * 30 / 14)] private float timeOfDay;
    public static float timeInADay = 60 * 30 / 14;
    [SerializeField] private float sunRotationSpeed;
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private Gradient moonColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay = (timeOfDay + (Arena.timePassed * sunRotationSpeed)) % timeInADay;

        updateSunRotation();
        updateLighting();
    }

    private void OnValidate()
    {
        updateSunRotation();
        updateLighting();
    }

    private void updateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-270, 90, timeOfDay / timeInADay);
        float moonRotation = sunRotation + 180;
        sun.transform.rotation = Quaternion.Euler(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z);
        moon.transform.rotation = Quaternion.Euler(moonRotation, moon.transform.rotation.y, moon.transform.rotation.z);
    }
    private void updateLighting()
    {
        float timeFraction = timeOfDay / timeInADay;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
        moon.color = moonColor.Evaluate(timeFraction);
    }
}
