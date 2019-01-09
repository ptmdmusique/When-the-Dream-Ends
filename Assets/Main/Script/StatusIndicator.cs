using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform statusRect;
    public Image statusImage;
    [SerializeField]
    private Text statusText;

    public Vector3 offset;
    public bool followTarget = false;
    public Transform target;
    public float lerpSpeed = 2;
    public string statusName;

    private void Update() {
        if (followTarget == false) {
            return;
        }

        transform.position = offset + target.position;
    }

    public void SetHealth(float curValue, float maxValue) {
        float value = curValue / maxValue;

        //healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        statusImage.fillAmount = Mathf.Lerp(statusImage.fillAmount, value, Time.deltaTime * lerpSpeed);

        if (statusText != null) { 
            statusText.text = curValue + "/" + maxValue + " " + statusName;
        }
    }
}
