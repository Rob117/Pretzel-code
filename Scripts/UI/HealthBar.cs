using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
    public Slider slider;
    private float initialFillScale;

    public void SetFillAmount(float percentToFill) {
        slider.value = percentToFill;
    }
}
