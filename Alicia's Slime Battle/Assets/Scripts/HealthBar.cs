using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        // Debug.Log($"Health: {health}, Slider Value: {slider.value}, Max: {slider.maxValue}");

    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
