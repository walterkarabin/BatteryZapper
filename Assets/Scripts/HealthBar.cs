using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxCharge(float charge)
    {
        slider.maxValue = charge;
    }
    public void SetCharge(float charge)
    {
        slider.value = charge;
        fill.color = gradient.Evaluate(slider.normalizedValue);

    }
    public float GetCharge()
    {
        return slider.value;
    }
}
