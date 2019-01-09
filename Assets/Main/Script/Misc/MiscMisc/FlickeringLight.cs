using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    public float lowBound = 0;
    public float highBound = 1;
    public float flickerAmount = 0.3f;

    public bool useBound = true;
    public bool highRate = false;
    public bool lowRate = false;
    public bool mediumRate = false;
    public bool customRate = false;
    public float customWaitTime = 0.1f;
    public bool randomFlickering = false;

    private Light myLight;
    private float intensity;
    private float curIntensity;
    private float waitTime;

    private void Start() {
        myLight = GetComponent<Light>();
        intensity = myLight.intensity;

        //Set the flicker range for the light
        if (useBound == true) {
            highBound = intensity + flickerAmount * 2;
            lowBound = intensity - flickerAmount * 2;
            lowBound = Mathf.Clamp(lowBound, 0, highBound);    
        }

        if (customRate == true) {
            waitTime = customWaitTime;
        } else if (highRate == true) {
            waitTime = 0.1f;
        } else if (mediumRate == true) {
            waitTime = 0.3f;
        } else if (lowRate == true) {
            waitTime = 0.5f;
        }

        StartCoroutine(Flickering());
    }

    IEnumerator Flickering() {
        if (randomFlickering == true) {
            myLight.intensity += Random.Range(-flickerAmount, flickerAmount);            
        } else {
            float value = Random.Range(0f, 1f);
            if (value < 0.5f) { //Equaly flicker
                myLight.intensity += flickerAmount;
            } else {
                myLight.intensity -= flickerAmount;
            }
        }

        if (useBound == true) { //Clamp the value
            myLight.intensity = Mathf.Clamp(myLight.intensity, lowBound, highBound);
        }

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(Flickering());
    }
}
