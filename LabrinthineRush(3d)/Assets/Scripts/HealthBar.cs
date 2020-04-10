using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public void setHealth(float health)
        {
            slider.value = health;
        }

        public void setMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
        }

    }
}
