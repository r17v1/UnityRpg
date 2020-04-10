using UnityEngine;
using UnityEngine.UI;


namespace RPG.UI
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        public void setStamina(float stamina)
        {
            slider.value = stamina;
        }

        public void setMaxStamina(float maxStamina)
        {
            slider.maxValue = maxStamina;
        }
    }
}
